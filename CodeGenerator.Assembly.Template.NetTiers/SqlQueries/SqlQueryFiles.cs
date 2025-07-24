using Microsoft.Data.SqlClient;
using System.Data;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries
{
    public class SqlQueryService : ISqlQueryService
    {
        private readonly SqlConnection _connection;

        public SqlQueryService(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public SqlCommand GetTables(IReadOnlyCollection<string> tables)
        {
            string fileName = "GetTables.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            SqlParameter selectedTablesParameter = new SqlParameter("@SelectedTables", SqlDbType.NVarChar);
            selectedTablesParameter.Value = string.Join(',', tables);
            sqlCommand.Parameters.Add(selectedTablesParameter);
            return sqlCommand;
        }

        public SqlCommand GetViews(IReadOnlyCollection<string> views)
        {
            throw new NotImplementedException();
        }

        public SqlCommand GetTablesEnumAndItems(IReadOnlyCollection<string> tables)
        {
            string fileName = "GetTablesEnumAndItems.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            SqlParameter selectedTablesParameter = new SqlParameter("@SelectedTables", SqlDbType.NVarChar);
            selectedTablesParameter.Value = string.Join(',', tables);
            sqlCommand.Parameters.Add(selectedTablesParameter);
            return sqlCommand;
        }

        public SqlCommand GetStoredProcedures()
        {
            string fileName = "GetStoredProcedures.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            return sqlCommand;
        }
        public SqlCommand GetStoredProceduresAndParameters()
        {
            string fileName = "GetStoredProceduresAndParameters.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            return sqlCommand;
        }

        public SqlCommand GetTablesColumns(IReadOnlyCollection<string> tables)
        {
            string fileName = "GetTablesColumns.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            SqlParameter selectedTablesParameter = new SqlParameter("@SelectedTables", SqlDbType.NVarChar);
            selectedTablesParameter.Value = string.Join(',', tables);
            sqlCommand.Parameters.Add(selectedTablesParameter);
            return sqlCommand;
        }
        public SqlCommand GetViewsColumns(IReadOnlyCollection<string> views)
        {
            string fileName = "GetViewsColumns.sql";
            string sql = SqlQueryLoader.Load(fileName);
            var sqlCommand = new SqlCommand(sql, _connection);
            SqlParameter selectedTablesParameter = new SqlParameter("@SelectedViews", SqlDbType.NVarChar);
            selectedTablesParameter.Value = string.Join(',', views);
            sqlCommand.Parameters.Add(selectedTablesParameter);
            return sqlCommand;
        }
    }
    internal static class SqlQueryLoader
    {
        public static string Load(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlQueries", "Files", fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"SQL file not found: {path}");

            return File.ReadAllText(path);
        }
    }
}

