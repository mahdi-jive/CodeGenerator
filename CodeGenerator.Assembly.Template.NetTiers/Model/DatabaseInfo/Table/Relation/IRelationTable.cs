namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column
{
    public interface IRelationTable : ISchemaObject
    {
        public string TableName { get; }
        public string TableNamePascal { get; }
        public string TableNameCamel { get; }
        public int TableId { get; }
        public string ColumnName { get; }
        public string ColumnNamePascal { get; }
        public string ColumnNameCamel { get; }
        public int ColumnId { get; }
    }
}
