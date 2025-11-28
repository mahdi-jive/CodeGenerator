using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using CodeGenerator.Assembly.Template.NetTiers.SqlQueries;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using StoredProcedureInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.StoredProcedure;
using TableInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Table;
using ViewInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.View;


namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    internal class DatabaseModelBuilder : IDatabaseModelBuilder
    {
        private readonly IUserConfiguration userConfiguration;

        private Lazy<Task<IEnumerable<ITable>>> TablesInfo { get; set; }
        private Lazy<Task<IEnumerable<IView>>> ViewsInfo { get; set; }
        private Lazy<Task<IEnumerable<ITableEnum>>> TablesEnumInfo { get; set; }
        public DatabaseModelBuilder(IUserConfiguration userConfiguration)
        {
            this.userConfiguration = userConfiguration;

        }
        private async Task<IEnumerable<IStoredProcedure>> GetStoredProcedures(string contain = null)
        {
            List<IStoredProcedure> storedProcedures = new List<IStoredProcedure>();
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {

                connection.Open();

                string query = @$"
                SELECT
                    p.name AS Name,
                    p.object_id AS ObjectId,
                    ep.value AS Description
                FROM sys.procedures p
                LEFT JOIN sys.extended_properties ep
                    ON ep.major_id = p.object_id
                    AND ep.minor_id = 0
                    AND ep.name = 'MS_Description'
                WHERE p.is_ms_shipped = 0
                     AND (LOWER(p.name)  LIKE '{contain}_%' OR LOWER(p.name) LIKE '{userConfiguration.CustomProcedureStartsWith}_{contain}_%')
                ORDER BY p.name;";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString()!;
                        int objectId = Convert.ToInt32(reader["ObjectId"]);
                        string description = reader["Description"] != DBNull.Value
                                ? reader["Description"].ToString()
                                : null;

                        var parametersInfo = new Lazy<Task<IEnumerable<IParameterProcedure>>>(() => GetParameterProcedures(name));
                        // اگر ستون‌های خروجی رو بعدا استخراج می‌کنی، فعلاً لیست خالی بفرست
                        var spInfo = new StoredProcedureInfo(
                            name: name,
                            objectId: objectId,
                            description: description,
                            parametersInfo: parametersInfo,
                            outputColumn: new Lazy<Task<IEnumerable<IOutputProcedure>>>(() => GetOutputProcedure(userConfiguration.ConnectionString, name, parametersInfo))
                        );

                        Console.WriteLine(spInfo.Name);
                        storedProcedures.Add(spInfo);
                    }
                }
            }
            return storedProcedures;

        }
        private async Task<IEnumerable<IParameterProcedure>> GetParameterProcedures(string storedProcedureName)
        {
            List<IParameterProcedure> parameterProcedures = new List<IParameterProcedure>();
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                connection.Open();

                string query = @"SELECT
    p.name AS StoredProcedureName,
    p.object_id AS StoredProcedureObjectID,
    pr.name AS ParameterName,
    ty.name AS ParameterType,
    pr.max_length AS MaxLength,
    pr.is_output AS IsOutput,
    pr.parameter_id AS ParameterOrder,
    ep.value AS Description,
    CASE WHEN ty.is_table_type = 1 THEN 1 ELSE 0 END AS IsTableType
FROM sys.procedures p
JOIN sys.parameters pr ON pr.object_id = p.object_id
LEFT JOIN sys.types ty ON ty.user_type_id = pr.user_type_id
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = p.object_id
    AND ep.minor_id = pr.parameter_id
    AND ep.name = 'MS_Description'
WHERE p.name = @StoredProcedureName
ORDER BY pr.parameter_id;";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StoredProcedureName", storedProcedureName);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var parameterProcedure = new ParameterProcedure(
                        name: reader["ParameterName"].ToString()!,
                        objectId: Convert.ToInt32(reader["StoredProcedureObjectID"]),
                        description: reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        parameterType: reader["ParameterType"].ToString()!,
                        isTableType: Convert.ToBoolean(reader["IsTableType"]),
                        maxLength: Convert.ToInt16(reader["MaxLength"]),
                        isOutput: Convert.ToBoolean(reader["IsOutput"]),
                        parameterOrder: Convert.ToInt32(reader["ParameterOrder"])
                    );
                    Console.WriteLine($"{storedProcedureName}.{parameterProcedure.Name}");
                    parameterProcedures.Add(parameterProcedure);
                }
                return parameterProcedures;
            }

        }
        private async Task<IEnumerable<IOutputProcedure>> GetOutputProcedure(string connectionString, string storedProcedureName, Lazy<Task<IEnumerable<IParameterProcedure>>> parameterProceduresLazy)
        {
            var parameterProcedures = parameterProceduresLazy.Value;
            var analyzer = new StoredProcedureAnalyzer(connectionString);
            var outputColumn = await analyzer.AnalyzeBasicAsync(storedProcedureName, (await parameterProcedures).Select(parameter => parameter.ToSqlParameter()).ToList());
            return outputColumn;
        }
        public IFluentViewStep LoadTable()
        {
            TablesInfo = new Lazy<Task<IEnumerable<ITable>>>(() => GetTable());
            return this;
        }
        private async Task<IEnumerable<ITable>> GetTable()
        {
            List<ITable> tables = new List<ITable>();
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                connection.Open();

                string query = @"
                SELECT
                    t.name AS Name,
                    t.object_id AS ObjectID,
                    ep.value AS Description
                FROM sys.tables t
                LEFT JOIN sys.extended_properties ep
                    ON ep.major_id = t.object_id
                    AND ep.minor_id = 0
                    AND ep.name = 'MS_Description'
                WHERE t.schema_id = 1
                ORDER BY t.name;";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString()!;
                        int objectID = Convert.ToInt32(reader["ObjectID"]);
                        string description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null;

                        var tableInfo = new TableInfo(name,
                                                  objectID,
                                                   description,
                                                   new Lazy<Task<IEnumerable<IColumnTable>>>(() => GetColumnTable(name)),
                                                   new Lazy<Task<IEnumerable<IStoredProcedure>>>(() => GetStoredProcedures(name)),
                                                   new Lazy<Task<IEnumerable<IRelationTable>>>(() => GetRelations(name)),
                                                   new Lazy<Task<IEnumerable<IRelationTable>>>(() => GetReferencedBy(name)));
                        Console.WriteLine(tableInfo.Name);
                        tables.Add(tableInfo);
                    }
                }
            }
            return tables.Where(q => !q.Name.Contains("Mapping") && !q.Name.Contains("TMP"));
        }
        private async Task<IEnumerable<IColumnTable>> GetColumnTable(string tableName)
        {
            List<IColumnTable> columns = new List<IColumnTable>();
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                connection.Open();

                string query = @"SELECT
    c.name AS ColumnName,
    c.column_id AS ColumnID,
    ep.value AS Description,
    t.name AS TableName,
    t.object_id AS TableObjectID,
    isnull(ty.name,ty2.name) AS DataType,
    isnull(isc.CHARACTER_MAXIMUM_LENGTH, c.max_length) AS MaxLength,
    c.precision AS Precision,
    c.scale AS Scale,
    c.collation_name AS Collation,
    c.is_nullable AS IsNullable,
    c.is_identity AS IsIdentity,
    c.is_computed AS IsComputed,
    c.is_rowguidcol AS IsRowGuid,
    c.is_sparse AS IsSparse,
    c.generated_always_type AS GeneratedAlwaysType,
    CASE WHEN i.is_primary_key = 1 THEN 1 ELSE 0 END AS IsPrimaryKey,
    dc.definition AS DefaultValue
FROM sys.columns c
JOIN sys.tables t ON t.object_id = c.object_id
JOIN INFORMATION_SCHEMA.COLUMNS isc
    ON isc.TABLE_NAME = t.name
   AND isc.COLUMN_NAME = c.name
   AND isc.TABLE_SCHEMA = SCHEMA_NAME(t.schema_id)
LEFT JOIN sys.types ty ON c.system_type_id = ty.user_type_id
LEFT JOIN sys.types ty2 ON c.user_type_id = ty2.user_type_id
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = c.object_id
    AND ep.minor_id = c.column_id
    AND ep.name = 'MS_Description'
LEFT JOIN sys.index_columns ic
    ON ic.object_id = c.object_id
    AND ic.column_id = c.column_id
LEFT JOIN sys.indexes i
    ON i.object_id = ic.object_id
    AND i.index_id = ic.index_id
    AND i.is_primary_key = 1
LEFT JOIN sys.default_constraints dc
    ON dc.parent_object_id = c.object_id
    AND dc.parent_column_id = c.column_id
WHERE t.name = @TableName
ORDER BY c.column_id;";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableName", tableName);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var colInfo = new ColumnTable
                      (
                          name: reader["ColumnName"].ToString()!,
                          objectId: Convert.ToInt32(reader["ColumnID"]),
                          description: reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                          tableName: reader["TableName"].ToString()!,
                          tableObjectId: Convert.ToInt32(reader["TableObjectID"]),
                          dataType: reader["DataType"].ToString()!,
                          maxLength: Convert.ToInt16(reader["MaxLength"]),
                          precision: Convert.ToByte(reader["Precision"]),
                          scale: Convert.ToByte(reader["Scale"]),
                          collation: reader["Collation"] != DBNull.Value ? reader["Collation"].ToString() : null,
                          isNullable: Convert.ToBoolean(reader["IsNullable"]),
                          isIdentity: Convert.ToBoolean(reader["IsIdentity"]),
                          isComputed: Convert.ToBoolean(reader["IsComputed"]),
                          isRowGuid: Convert.ToBoolean(reader["IsRowGuid"]),
                          isSparse: Convert.ToBoolean(reader["IsSparse"]),
                          generatedAlwaysType: reader["GeneratedAlwaysType"].ToString()!,
                          isPrimaryKey: Convert.ToBoolean(reader["IsPrimaryKey"]),
                          defaultValue: reader["DefaultValue"] != DBNull.Value ? reader["DefaultValue"].ToString() : null
                      );

                    Console.WriteLine($"{colInfo.TableName}.{colInfo.Name}");
                    columns.Add(colInfo);
                }
            }
            return columns;
        }

        private async Task<IEnumerable<IRelationTable>> GetRelations(string referencedTableName)
        {
            List<IRelationTable> relations = new List<IRelationTable>();
            var query = @"
            SELECT  
                fk.name AS Name,
                fk.object_id AS ObjectId,
                ep.value AS Description,
                tr.name AS TableName,
                tr.object_id AS TableId,
                cp.name AS ColumnName,
                cp.column_id AS ColumnId
            FROM sys.foreign_keys fk
            INNER JOIN sys.foreign_key_columns fkc 
                ON fk.object_id = fkc.constraint_object_id
            INNER JOIN sys.tables tp 
                ON fkc.parent_object_id = tp.object_id
            INNER JOIN sys.columns cp 
                ON fkc.parent_object_id = cp.object_id 
               AND fkc.parent_column_id = cp.column_id
            INNER JOIN sys.tables tr 
                ON fkc.referenced_object_id = tr.object_id
LEFT JOIN sys.extended_properties ep
    ON ep.major_id = fk.object_id
   AND ep.minor_id = 0
   AND ep.name = 'MS_Description'
            WHERE tp.name = @TableName;";

            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", referencedTableName);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        relations.Add(new RelationTable(
                             Name: reader["Name"] as string,
                             objectId: (int)reader["ObjectId"],
                             description: reader["Description"] as string,
                             tableName: reader["TableName"] as string,
                             tableId: (int)reader["TableId"],
                             columnName: reader["ColumnName"] as string,
                             columnId: (int)reader["ColumnId"]
                         ));
                    }
                }
            }
            return relations;
        }
        private async Task<IEnumerable<IRelationTable>> GetReferencedBy(string referencedTableName)
        {
            List<IRelationTable> relations = new List<IRelationTable>();
            var query = @"SELECT  
                            fk.name AS Name,
                            fk.object_id AS ObjectId,
                            ep.value AS Description,
                            tp.name AS TableName,
                            tp.object_id AS TableId,
                            cp.name AS ColumnName,
                            cp.column_id AS ColumnId
                            FROM sys.foreign_keys fk
                            INNER JOIN sys.foreign_key_columns fkc 
                                ON fk.object_id = fkc.constraint_object_id
                            INNER JOIN sys.tables tp          -- جدول فرزند
                                ON fkc.parent_object_id = tp.object_id
                            INNER JOIN sys.columns cp         -- ستون‌های جدول فرزند
                                ON fkc.parent_object_id = cp.object_id
                               AND fkc.parent_column_id = cp.column_id
                            INNER JOIN sys.tables tr          -- جدول والد (هدف)
                                ON fkc.referenced_object_id = tr.object_id
                            LEFT JOIN sys.extended_properties ep
                                ON ep.major_id = fk.object_id
                               AND ep.minor_id = 0
                               AND ep.name = 'MS_Description'
                            WHERE tr.name = @TableName;";

            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", referencedTableName);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        relations.Add(new RelationTable(
                             Name: reader["Name"] as string,
                             objectId: (int)reader["ObjectId"],
                             description: reader["Description"] as string,
                             tableName: reader["TableName"] as string,
                             tableId: (int)reader["TableId"],
                             columnName: reader["ColumnName"] as string,
                             columnId: (int)reader["ColumnId"]
                         ));
                    }
                }
            }
            return relations;
        }

        private async Task<IEnumerable<IColumnView>> GetColumnView(string viewName)
        {
            var columns = new List<IColumnView>();

            var query = @"
        SELECT  
            c.name AS ColumnName,
            c.column_id AS ObjectId,
            ep.value AS Description,
            v.name AS ViewName,
            v.object_id AS ViewObjectId,
            t.name AS DataType,
            c.collation_name AS Collation,
            c.is_nullable AS IsNullable
        FROM sys.columns c
        JOIN sys.views v ON c.object_id = v.object_id
        JOIN sys.types t ON c.user_type_id = t.user_type_id
        LEFT JOIN sys.extended_properties ep 
               ON ep.major_id = c.object_id 
              AND ep.minor_id = c.column_id 
              AND ep.name = 'MS_Description'
        WHERE v.name = @ViewName";

            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ViewName", viewName);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var colInfo = new ColumnView(
                            name: reader["ColumnName"].ToString(),
                            objectId: (int)reader["ObjectId"],
                            description: reader["Description"]?.ToString(),
                            viewName: reader["ViewName"].ToString(),
                            viewObjectId: (int)reader["ViewObjectId"],
                            dataType: reader["DataType"].ToString(),
                            collation: reader["Collation"] as string,
                            isNullable: (bool)reader["IsNullable"]
                        );
                        columns.Add(colInfo);
                    }
                }
            }

            return columns;
        }
        public IFluentEnumStep LoadViews()
        {
            ViewsInfo = new Lazy<Task<IEnumerable<IView>>>(() => GetViews());
            return this;
        }
        public async Task<IEnumerable<IView>> GetViews()
        {
            var views = new List<IView>();

            var query = @"
        SELECT 
            v.name AS ViewName,
            v.object_id AS ViewId,
            ep.value AS Description
        FROM sys.views v
        LEFT JOIN sys.extended_properties ep 
               ON ep.major_id = v.object_id 
              AND ep.minor_id = 0 
              AND ep.name = 'MS_Description'
        WHERE v.is_ms_shipped = 0";

            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var viewName = reader["ViewName"].ToString();
                        var viewId = (int)reader["ViewId"];
                        var description = reader["Description"]?.ToString();

                        // Regex مشابه کدی که داشتی
                        var regex = new Regex(
                            $"^{Regex.Escape(userConfiguration.CustomProcedureStartsWith)}_({Regex.Escape(viewName)})_.+$",
                            RegexOptions.IgnoreCase);

                        var viewInfo = new ViewInfo(
                            viewName,
                            viewId,
                            description,
                            new Lazy<Task<IEnumerable<IColumnView>>>(() => GetColumnView(viewName)),
                            new Lazy<Task<IEnumerable<IStoredProcedure>>>(() => GetStoredProcedures(viewName))
                        );

                        views.Add(viewInfo);
                    }
                }
            }

            return views;
        }
        public IFluentBuildStep LoadTableEnums()
        {
            TablesEnumInfo = new Lazy<Task<IEnumerable<ITableEnum>>>(() => GetTableEnums());
            return this;
        }
        private async Task<IEnumerable<ITableEnum>> GetTableEnums()
        {
            var enums = new List<ITableEnum>();

            var query = @"
        SELECT 
            t.name AS TableName,
            t.object_id AS TableId,
            ep.value AS Description
        FROM sys.tables t
        LEFT JOIN sys.extended_properties ep 
               ON ep.major_id = t.object_id 
              AND ep.minor_id = 0 
              AND ep.name = 'MS_Description'
        WHERE t.is_ms_shipped = 0
          AND (
                SELECT COUNT(*) 
                FROM sys.columns c 
                WHERE c.object_id = t.object_id
              ) >= 3
          AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.name = 'Id')
          AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.name = 'Name')
          AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.name = 'Title')
          AND EXISTS (
                SELECT 1
                FROM sys.indexes i
                JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                WHERE i.object_id = t.object_id
                  AND i.is_unique = 1
                  AND c.name = 'Name'
              )";

            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var tableName = reader["TableName"].ToString();
                        var tableId = (int)reader["TableId"];
                        var description = reader["Description"]?.ToString();

                        enums.Add(new TablesEnum(
                            tableName,
                            tableId,
                            description,
                            new Lazy<Task<IEnumerable<IEnumItem>>>(() => GetEnumItems(tableName))
                        ));
                    }
                }
            }

            return enums;
        }


        private async Task<IEnumerable<IEnumItem>> GetEnumItems(string tableName)
        {
            List<IEnumItem> items = new List<IEnumItem>();
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                string query = $"SELECT [ID], [Name], [Title] FROM {tableName}";
                using (var cmd = new SqlCommand(query, connection))
                {

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            string name = Convert.ToString(reader["Title"]) ?? "";
                            string title = Convert.ToString(reader["Title"]) ?? "";

                            Console.WriteLine($"  ➡️ Id: {id}, Name: {name}, Title: {title}");
                            items.Add(new EnumItem(id, name, title));
                        }
                    }
                }

            }
            return items;
        }
        public DatabaseInfoModel Build()
        {
            return new DatabaseInfoModel(
                 userConfiguration.CustomProcedureStartsWith,
                 userConfiguration.CompanyName,
                 userConfiguration.CompanyURL,
                 userConfiguration.RootNameSpace,
                 TablesInfo,
                 ViewsInfo,
                 TablesEnumInfo
             );
        }
    }
    public interface IDatabaseModelBuilder :
    IFluentTableStep,
    IFluentViewStep,
    IFluentEnumStep,
    IFluentBuildStep
    {

    }

    public interface IFluentTableStep
    {
        IFluentViewStep LoadTable();
    }

    public interface IFluentViewStep
    {
        IFluentEnumStep LoadViews();
    }

    public interface IFluentEnumStep
    {
        IFluentBuildStep LoadTableEnums();
    }

    public interface IFluentBuildStep
    {
        DatabaseInfoModel Build();
    }

}
