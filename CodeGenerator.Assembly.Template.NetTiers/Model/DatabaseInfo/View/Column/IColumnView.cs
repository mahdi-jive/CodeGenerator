namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column
{
    public interface IColumnView : ISchemaObject
    {
        public string ViewName { get; }
        public int ViewObjectId { get; }
        public string DataType { get; }
        string CSharpType { get; }
        string DbType { get; }
        public string? Collation { get; }
        public bool IsNullable { get; }
    }
}
