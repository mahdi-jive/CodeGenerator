namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index
{
    public interface IIndexTable : ISchemaObject
    {
        public bool IsPrimaryKey { get; }
        public ICollection<int> ColumnsId { get; }
    }
}
