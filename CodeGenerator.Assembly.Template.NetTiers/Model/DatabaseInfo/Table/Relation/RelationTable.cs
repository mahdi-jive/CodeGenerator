namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column
{
    public class RelationTable : SchemaObject, IRelationTable
    {
        public RelationTable(string Name, int objectId, string? description, string tableName, int tableId, string columnName, int columnId)
            : base(Name, objectId, string.Empty)
        {
            TableName = tableName;
            TableId = tableId;
            ColumnName = columnName;
            ColumnId = columnId;
        }
        public string TableName { get; private set; }
        public int TableId { get; private set; }
        public string ColumnName { get; private set; }
        public int ColumnId { get; private set; }
    }
}
