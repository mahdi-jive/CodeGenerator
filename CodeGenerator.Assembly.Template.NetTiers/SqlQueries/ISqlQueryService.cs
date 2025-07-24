using Microsoft.Data.SqlClient;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries
{
    public interface ISqlQueryService
    {
        public SqlCommand GetTables(IReadOnlyCollection<string> tables);
        public SqlCommand GetViews(IReadOnlyCollection<string> views);
        public SqlCommand GetTablesEnumAndItems(IReadOnlyCollection<string> tables);
        public SqlCommand GetTablesColumns(IReadOnlyCollection<string> tables);
        public SqlCommand GetViewsColumns(IReadOnlyCollection<string> tables);
        public SqlCommand GetStoredProcedures();
        public SqlCommand GetStoredProceduresAndParameters();

    }
}