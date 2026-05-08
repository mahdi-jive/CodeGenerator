namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column
{
    public interface IColumnView : ISchemaObject
    {
        public string ViewName { get; }
        public int ViewObjectId { get; }
        int MaxLength { get; }
        bool HasLength { get; }
        public DataTypeSql DataType { get; }
        public string? Collation { get; }
        public bool IsNullable { get; }
        public string ToStringParamsSystemType();
    }
}
