namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column
{
    public interface IRelationTable
    {
        public string TableName { get; }
        public int TableId { get; }
        public string ColumnName { get; }
        public int ColumnId { get; }
    }
}
