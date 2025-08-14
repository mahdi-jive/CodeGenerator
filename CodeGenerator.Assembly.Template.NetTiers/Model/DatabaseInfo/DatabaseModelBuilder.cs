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
        private readonly Regex regexSp;

        private Lazy<IEnumerable<ITable>> TablesInfo { get; set; }
        private Lazy<IEnumerable<IView>> ViewsInfo { get; set; }
        private Lazy<IEnumerable<ITableEnum>> TablesEnumInfo { get; set; }
        public DatabaseModelBuilder(IUserConfiguration userConfiguration)
        {
            this.userConfiguration = userConfiguration;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                var serverConnection = new ServerConnection(connection);
                database = new Server(serverConnection).Databases[connection.Database];
            }
            regexSp = new Regex(@$"^{userConfiguration.CustomProcedureStartsWith}_([^_]+)_");
        }
        private IEnumerable<IStoredProcedure> GetStoredProcedures(Regex regex = null)
        {
            foreach (StoredProcedureSmo storedProcedure in database.StoredProcedures)
            {
                // Ignore system stored procedures
                if (storedProcedure.IsSystemObject && (regex == null || !regex.IsMatch(storedProcedure.Name)))
                    continue;

                string name = storedProcedure.Name;
                int objectId = storedProcedure.ID;
                string? description = storedProcedure.ExtendedProperties["MS_Description"]?.Value?.ToString();



                var parametersInfo = new Lazy<IEnumerable<IParameterProcedure>>(GetParameterProcedures(storedProcedure));
                // اگر ستون‌های خروجی رو بعدا استخراج می‌کنی، فعلاً لیست خالی بفرست
                var spInfo = new StoredProcedureInfo(
                    name: storedProcedure.Name,
                    objectId: storedProcedure.ID,
                    description: description,
                    parametersInfo: parametersInfo,
                    outputColumn: new Lazy<IEnumerable<IOutputProcedure>>(GetOutputProcedure(userConfiguration.ConnectionString, storedProcedure.Name, parametersInfo.Value))
                );

                Console.WriteLine(spInfo.Name);
                yield return spInfo;
            }
        }
        private IEnumerable<IParameterProcedure> GetParameterProcedures(StoredProcedureSmo storedProcedure)
        {
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
                yield return new ParameterProcedure
                (
                    name: parameter.Name,
                    objectId: storedProcedure.ID,
                    description: descriptionParameter,
                    parameterType: resolvedType,
                    isTableType: isTableType,
                    maxLength: parameter.DataType.MaximumLength,
                    isOutput: parameter.IsOutputParameter,
                    parameterOrder: parameter.ID
                );
            }
        }
        private IEnumerable<IOutputProcedure> GetOutputProcedure(string connectionString, string storedProcedureName, IEnumerable<IParameterProcedure> parameterProcedures)
        {
            var analyzer = new StoredProcedureAnalyzer(connectionString);
            var outputColumn = analyzer.AnalyzeBasicAsync(storedProcedureName, parameterProcedures.Select(parameter => parameter.ToSqlParameter()).ToList());
            return outputColumn;
        }
        public IFluentViewStep LoadTable()
        {
            TablesInfo = new Lazy<IEnumerable<ITable>>(GetTable);
            return this;
        }
        private IEnumerable<ITable> GetTable()
        {
            foreach (TableSmo table in database.Tables)
            {
                if (table.IsSystemObject)
                    continue;

                Regex regexSp = new Regex(@$"^{userConfiguration.CustomProcedureStartsWith}_([^_]+)_");
                var tableInfo = new TableInfo(table.Name,
                                                table.ID,
                                                table.ExtendedProperties["MS_Description"]?.Value?.ToString(),
                                                new Lazy<IEnumerable<IColumnTable>>(GetColumnTable(table)),
                                                new Lazy<IEnumerable<IStoredProcedure>>(GetStoredProcedures(regexSp)));
                Console.WriteLine(tableInfo.Name);
                yield return tableInfo;
            }
        }
        private IEnumerable<IColumnTable> GetColumnTable(TableSmo table)
        {
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

                yield return colInfo;
            }
        }

        private IEnumerable<IColumnView> GetColumnView(ViewSmo view)
        {
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

                yield return colInfo;
            }
        }
        public IFluentEnumStep LoadViews()
        {
            ViewsInfo = new Lazy<IEnumerable<IView>>(GetViews);
            return this;
        }
        public IEnumerable<ViewInfo> GetViews()
        {
            foreach (ViewSmo view in database.Views)
            {
                if (view.IsSystemObject)
                    continue;

                var viewInfo = new ViewInfo(view.Name,
                    view.ID,
                    view.ExtendedProperties["MS_Description"]?.Value?.ToString(),
                    new Lazy<IEnumerable<IColumnView>>(GetColumnView(view)),
                    new Lazy<IEnumerable<IStoredProcedure>>(GetStoredProcedures(regexSp)));
                yield return viewInfo;
            }
        }
        public IFluentBuildStep LoadTableEnums()
        {
            TablesEnumInfo = new Lazy<IEnumerable<ITableEnum>>(GetTableEnums);
            return this;
        }
        public IEnumerable<TablesEnum> GetTableEnums()
        {
            foreach (TableSmo table in database.Tables)
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


                string? description = table.ExtendedProperties["MS_Description"]?.Value?.ToString();
                yield return new TablesEnum(table.Name,
                    table.ID,
                    description,
                    new Lazy<IEnumerable<IEnumItem>>(GetEnumItems(table.Name)));
            }

        }

        private IEnumerable<IEnumItem> GetEnumItems(string tableName)
        {
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                string query = $"SELECT [ID], [Name], [Title] FROM {tableName}";
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
                            yield return new EnumItem(id, name, title);
                        }
                    }
                }

            }
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
