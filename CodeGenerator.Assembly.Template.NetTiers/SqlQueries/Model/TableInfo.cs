using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class TableInfo : SchemaObject
    {
        public IReadOnlyCollection<ColumnTableInfo> ColumnInfoTable { get; private set; }

        public TableInfo(IReadOnlyCollection<ColumnTableInfo> columnInfoTable)
        {
            ColumnInfoTable = columnInfoTable;
        }
    }
}
