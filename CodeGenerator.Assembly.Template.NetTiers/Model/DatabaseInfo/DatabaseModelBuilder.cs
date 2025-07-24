using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.SqlQueries;
using CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    internal class DatabaseModelBuilder :
    IFluentStoredProcedureStep,
    IFluentTableStep,
    IFluentViewStep,
    IFluentEnumStep,
    IFluentBuildStep
    {
        private readonly IUserConfiguration userConfiguration;
        private readonly ISqlQueryService sqlQueryService;


        private IReadOnlyCollection<TableInfo> TablesInfo { get; set; } = Array.Empty<TableInfo>();
        private IReadOnlyCollection<ViewInfo> ViewsInfo { get; set; } = Array.Empty<ViewInfo>();
        private IReadOnlyCollection<TablesEnumInfo> TablesEnumInfo { get; set; } = Array.Empty<TablesEnumInfo>();
        private IReadOnlyCollection<StoredProcedureInfo> StoredProcedureInfo { get; set; } = Array.Empty<StoredProcedureInfo>();
        public DatabaseModelBuilder(IUserConfiguration userConfiguration, ISqlQueryService sqlQueryService)
        {
            this.userConfiguration = userConfiguration;
            this.sqlQueryService = sqlQueryService;
        }

        public IFluentTableStep LoadStoredProcedures()
        {
            IReadOnlyCollection<StoredProcedureInfo> storedProcedures;
            IReadOnlyCollection<StoredProceduresAndParametersInfo> storedProceduresAndParameters;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                using (var sqlCommand = sqlQueryService.GetStoredProcedures())
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                    .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    storedProcedures = connection.Query<StoredProcedureInfo>(sql, parameters).ToList();
                }
                using (var sqlCommand = sqlQueryService.GetStoredProceduresAndParameters())
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                    .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    storedProceduresAndParameters = connection.Query<StoredProceduresAndParametersInfo>(sql, parameters).ToList();
                }
            }

            foreach (var procedure in storedProcedures)
            {
                IReadOnlyCollection<ProceduresParameterInfo> parameters = storedProceduresAndParameters
                                .Where(p => p.ProcedureId == procedure.ObjectId)
                                .Select(parameter => parameter).ToList();
                var analyzer = new StoredProcedureAnalyzer(userConfiguration.ConnectionString);
                procedure.OutputColumn = analyzer.AnalyzeBasicAsync(procedure.Name, parameters.Select(parameter => parameter.ToSqlParameter()).ToList());
                procedure.ParametersInfo = parameters;
            }
            StoredProcedureInfo = storedProcedures;
            return this;
        }

        public IFluentViewStep LoadTable()
        {
            IReadOnlyCollection<SchemaObject> tablesObject;
            IReadOnlyCollection<ColumnTableInfo> columnsTableInfo;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                using (var sqlCommand = sqlQueryService.GetTables(userConfiguration.SelectedTables))
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                                         .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    tablesObject = connection.Query<SchemaObject>(sql, parameters).ToList();

                }
                using (var sqlCommand = sqlQueryService.GetTablesColumns(userConfiguration.SelectedTables))
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                                         .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    columnsTableInfo = connection.Query<ColumnTableInfo>(sql, parameters).ToList();
                }
            }
            TablesInfo = tablesObject.Select((table) =>
            {
                TableInfo tableInfo = null;
                var tableColumn = columnsTableInfo.Where(col => col.TableName == table.Name);
                if (tableColumn.Any())
                    tableInfo = new TableInfo(tableColumn.ToList());
                return tableInfo;
            }).Where(info => info is not null)
              .Cast<TableInfo>().ToList().AsReadOnly();

            return this;
        }

        public IFluentEnumStep LoadViews()
        {
            IReadOnlyCollection<ColumnViewInfo> columnsViewInfo;
            IReadOnlyCollection<SchemaObject> viewsObject;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                using (var sqlCommand = sqlQueryService.GetViews(userConfiguration.SelectedViews))
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                                         .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    viewsObject = connection.Query<SchemaObject>(sql, parameters).ToList();

                }
                using (var sqlCommand = sqlQueryService.GetViewsColumns(userConfiguration.SelectedViews))
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                                         .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    columnsViewInfo = connection.Query<ColumnViewInfo>(sql, parameters).ToList();
                }
            }
            ViewsInfo = viewsObject.Select((view) =>
            {
                ViewInfo tableInfo = null;
                var viewColumn = columnsViewInfo.Where(col => col.ViewName == view.Name);
                if (viewColumn.Any())
                    tableInfo = new ViewInfo(viewColumn.ToList());
                return tableInfo;
            }).Where(info => info is not null)
            .Cast<ViewInfo>().ToList().AsReadOnly();

            return this;
        }

        public IFluentBuildStep LoadTableEnums()
        {
            IReadOnlyCollection<TablesEnumAndItemsInfo> tableEnums;
            using (var connection = new SqlConnection(userConfiguration.ConnectionString))
            {
                using (var sqlCommand = sqlQueryService.GetTablesEnumAndItems(userConfiguration.SelectedTableEnums))
                {
                    string sql = sqlCommand.CommandText;
                    var parameters = sqlCommand.Parameters.Cast<SqlParameter>()
                    .ToDictionary(p => p.ParameterName, p => (object)p.Value);

                    tableEnums = connection.Query<TablesEnumAndItemsInfo>(sql, parameters).ToList();

                }
            }
            TablesEnumInfo = tableEnums.GroupBy(table => new SchemaObject()
            {
                ObjectId = table.ObjectId,
                Name = table.TableName,
                Description = table.Description
            }).Select((group) =>
                {
                    TablesEnumInfo tablesEnumInfo = new TablesEnumInfo();
                    tablesEnumInfo.ObjectId = group.Key.ObjectId;
                    tablesEnumInfo.Name = group.Key.Name;
                    tablesEnumInfo.Description = group.Key.Description;
                    tablesEnumInfo.EnumItemsInfos = group.Select(items => new EnumItemsInfo()
                    {
                        Id = items.Id,
                        Name = items.Name,
                        Title = items.Title

                    }).Where(info => info is not null)
                    .Cast<EnumItemsInfo>().ToList().AsReadOnly();
                    return tablesEnumInfo;
                }).Where(info => info is not null)
                .Cast<TablesEnumInfo>().ToList().AsReadOnly();
            return this;
        }

        public DatabaseInfoModel Build()
        {
            return new DatabaseInfoModel(null, null, null);
        }
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
