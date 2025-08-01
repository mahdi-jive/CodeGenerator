using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using CodeGenerator.Assembly.Template.NetTiers.SqlQueries;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;
using ColumnSmo = Microsoft.SqlServer.Management.Smo.Column;
using IndexSmo = Microsoft.SqlServer.Management.Smo.Index;
using StoredProcedureInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.StoredProcedure;
using StoredProcedureSmo = Microsoft.SqlServer.Management.Smo.StoredProcedure;
using TableInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Table;
using TableSmo = Microsoft.SqlServer.Management.Smo.Table;
using ViewInfo = CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.View;
using ViewSmo = Microsoft.SqlServer.Management.Smo.View;


namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    internal class DatabaseModelBuilder : IDatabaseModelBuilder
    {
        private readonly IUserConfiguration userConfiguration;
        private readonly Database database;


        private IReadOnlyCollection<TableInfo> TablesInfo { get; set; } = Array.Empty<TableInfo>();
        private IReadOnlyCollection<ViewInfo> ViewsInfo { get; set; } = Array.Empty<ViewInfo>();
        private IReadOnlyCollection<TablesEnum> TablesEnumInfo { get; set; } = Array.Empty<TablesEnum>();
        private IReadOnlyCollection<StoredProcedureInfo> StoredProcedureInfo { get; set; } = Array.Empty<StoredProcedureInfo>();
        public DatabaseModelBuilder(IUserConfiguration userConfiguration)
        {
            this.userConfiguration = userConfiguration;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                var serverConnection = new ServerConnection(connection);
                database = new Server(serverConnection).Databases[connection.Database];
            }
        }

        public IFluentTableStep LoadStoredProcedures()
        {
            List<StoredProcedureInfo> storedProcedures = new List<StoredProcedureInfo>();

            foreach (StoredProcedureSmo storedProcedure in database.StoredProcedures)
            {
                // Ignore system stored procedures
                if (storedProcedure.IsSystemObject)
                    continue;

                string name = storedProcedure.Name;
                int objectId = storedProcedure.ID;
                string? description = storedProcedure.ExtendedProperties["MS_Description"]?.Value?.ToString();


                var parametersInfo = new List<ParameterProcedure>();
                foreach (StoredProcedureParameter parameter in storedProcedure.Parameters)
                {
                    string resolvedType;
                    bool isTableType = parameter.DataType.SqlDataType == SqlDataType.UserDefinedTableType;

                    if (parameter.DataType.SqlDataType == SqlDataType.UserDefinedDataType)
                    {
                        // برای user-defined data type باید از دیتابیس معادل سیستمیشو بگیریم
                        var userType = database.UserDefinedDataTypes[parameter.DataType.Name];
                        resolvedType = userType != null ? userType.SystemType : parameter.DataType.Name;
                    }
                    else if (isTableType)
                        resolvedType = parameter.DataType.Name;
                    else
                        resolvedType = parameter.DataType.Name;

                    var descriptionParameter = parameter.ExtendedProperties["MS_Description"]?.Value?.ToString();
                    parametersInfo.Add(new ParameterProcedure
                    (
                        name: parameter.Name,
                        objectId: storedProcedure.ID,
                        description: descriptionParameter,
                        parameterType: resolvedType,
                        isTableType: isTableType,
                        maxLength: parameter.DataType.MaximumLength,
                        isOutput: parameter.IsOutputParameter,
                        parameterOrder: parameter.ID
                    ));
                }
                var analyzer = new StoredProcedureAnalyzer(userConfiguration.ConnectionString);
                var outputColumn = analyzer.AnalyzeBasicAsync(storedProcedure.Name, parametersInfo.Select(parameter => parameter.ToSqlParameter()).ToList());
                // اگر ستون‌های خروجی رو بعدا استخراج می‌کنی، فعلاً لیست خالی بفرست
                var spInfo = new StoredProcedureInfo(
                    name: storedProcedure.Name,
                    objectId: storedProcedure.ID,
                    description: description,
                    parametersInfo: parametersInfo,
                    outputColumn: outputColumn
                );

                storedProcedures.Add(spInfo);
            }
            StoredProcedureInfo = storedProcedures;
            return this;
        }

        public IFluentViewStep LoadTable()
        {
            var tablesInfo = new List<TableInfo>();
            foreach (TableSmo table in database.Tables)
            {
                if (table.IsSystemObject)
                    continue;

                List<IColumnTable> columnTables = new List<IColumnTable>();
                foreach (ColumnSmo column in table.Columns)
                {

                    string resolvedType;

                    if (column.DataType.SqlDataType == SqlDataType.UserDefinedDataType)
                    {
                        // برای user-defined data type باید از دیتابیس معادل سیستمیشو بگیریم
                        var userType = database.UserDefinedDataTypes[column.DataType.Name];
                        resolvedType = userType != null ? userType.SystemType : column.DataType.Name;
                    }
                    else
                        resolvedType = column.DataType.Name;

                    bool isPrimaryKey = false;
                    // بررسی اینکه ستون کلید اصلی هست یا نه
                    foreach (IndexSmo index in table.Indexes)
                    {
                        if (index.IndexKeyType == IndexKeyType.DriPrimaryKey && index.IndexedColumns.Contains(column.Name))
                        {
                            isPrimaryKey = true;
                            break;
                        }
                    }

                    var colInfo = new ColumnTable(
                    name: column.Name,
                    objectId: column.ID,
                    description: column.ExtendedProperties["MS_Description"]?.Value?.ToString(),
                    tableName: table.Name,
                    tableObjectId: table.ID,
                    dataType: column.DataType.Name,
                    maxLength: column.DataType.MaximumLength,
                    precision: column.DataType.NumericPrecision,
                    scale: column.DataType.NumericScale,
                    collation: column.Collation,
                    isNullable: column.Nullable,
                    isIdentity: column.Identity,
                    isComputed: column.Computed,
                    isRowGuid: column.RowGuidCol,
                    isSparse: column.IsSparse,
                    generatedAlwaysType: column.GeneratedAlwaysType.ToString(),
                    isPrimaryKey: isPrimaryKey,
                    defaultValue: column.Default == null ? null : column.Default
                );

                    columnTables.Add(colInfo);
                }
                var spTable = StoredProcedureInfo.Where(sp =>
                {
                    var match = Regex.Match(sp.Name, @$"^{userConfiguration.CustomProcedureStartsWith}_([^_]+)_", RegexOptions.IgnoreCase);
                    return match.Success && match.Groups[1].Value.Equals(table.Name, StringComparison.OrdinalIgnoreCase);
                }).ToList();
                var tableInfo = new TableInfo(table.Name, table.ID, table.ExtendedProperties["MS_Description"]?.Value?.ToString(), columnTables.ToList(), spTable);
                tablesInfo.Add(tableInfo);
            }
            TablesInfo = tablesInfo;
            return this;
        }

        public IFluentEnumStep LoadViews()
        {
            var viewsInfo = new List<ViewInfo>();
            foreach (ViewSmo view in database.Views)
            {
                if (view.IsSystemObject)
                    continue;

                var columnTables = new List<ColumnView>();
                foreach (ColumnSmo column in view.Columns)
                {

                    string resolvedType;

                    if (column.DataType.SqlDataType == SqlDataType.UserDefinedDataType)
                    {
                        // برای user-defined data type باید از دیتابیس معادل سیستمیشو بگیریم
                        var userType = database.UserDefinedDataTypes[column.DataType.Name];
                        resolvedType = userType != null ? userType.SystemType : column.DataType.Name;
                    }
                    else
                        resolvedType = column.DataType.Name;

                    var colInfo = new ColumnView(
                    name: column.Name,
                    objectId: column.ID,
                    description: column.ExtendedProperties["MS_Description"]?.Value?.ToString(),
                    viewName: view.Name,
                    viewObjectId: view.ID,
                    dataType: column.DataType.Name,
                    collation: column.Collation,
                    isNullable: column.Nullable
                );

                    columnTables.Add(colInfo);
                }
                var spTable = StoredProcedureInfo.Where(sp =>
                {
                    var match = Regex.Match(sp.Name, @$"^{userConfiguration.CustomProcedureStartsWith}_([^_]+)_", RegexOptions.IgnoreCase);
                    return match.Success && match.Groups[1].Value.Equals(view.Name, StringComparison.OrdinalIgnoreCase);
                }).ToList();
                var viewInfo = new ViewInfo(view.Name, view.ID, view.ExtendedProperties["MS_Description"]?.Value?.ToString(), columnTables, spTable);
                viewsInfo.Add(viewInfo);
            }
            ViewsInfo = viewsInfo;
            return this;
        }

        public IFluentBuildStep LoadTableEnums()
        {
            var tablesEnumInfo = new List<TablesEnum>();
            foreach (Microsoft.SqlServer.Management.Smo.Table table in database.Tables)
            {
                // رد کردن جداول سیستمی
                if (table.IsSystemObject)
                    continue;

                // بررسی تعداد ستون‌ها
                if (table.Columns.Count < 3)
                    continue;

                // بررسی نام ستون‌ها
                var columnNames = table.Columns.Cast<ColumnSmo>().Select(c => c.Name).ToList();
                var requiredNames = new[] { "Id", "Name", "Title" };
                if (!requiredNames.All(name => columnNames.Contains(name)))
                    continue;

                // بررسی اینکه ستون Name ایندکس یونیک دارد
                bool isNameUnique = false;
                foreach (IndexSmo index in table.Indexes)
                {
                    if (index.IsUnique && index.IndexedColumns.Cast<IndexedColumn>().Any(ic => ic.Name == "Name"))
                    {
                        isNameUnique = true;
                        break;
                    }
                }
                if (!isNameUnique)
                    continue;

                var enumItems = new List<EnumItem>();
                using (var connection = new SqlConnection(userConfiguration.ConnectionString))
                {
                    string query = $"SELECT [ID], [Name], [Title] FROM {table.Name}";
                    using (var cmd = new SqlCommand(query, connection))
                    {

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["ID"]);
                                string name = Convert.ToString(reader["Title"]) ?? "";
                                string title = Convert.ToString(reader["Title"]) ?? "";

                                Console.WriteLine($"  ➡️ Id: {id}, Name: {name}, Title: {title}");
                                enumItems.Add(new EnumItem(id, name, title));
                            }
                        }
                    }

                }
                string? description = table.ExtendedProperties["MS_Description"]?.Value?.ToString();
                tablesEnumInfo.Add(new TablesEnum(table.Name, table.ID, description, enumItems));
            }

            return this;
        }

        public DatabaseInfoModel Build()
        {
            return new DatabaseInfoModel(TablesInfo, ViewsInfo, TablesEnumInfo, StoredProcedureInfo);
        }
    }
    public interface IDatabaseModelBuilder : IFluentStoredProcedureStep,
    IFluentTableStep,
    IFluentViewStep,
    IFluentEnumStep,
    IFluentBuildStep
    {

    }
    public interface IFluentStoredProcedureStep
    {
        IFluentTableStep LoadStoredProcedures();
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
