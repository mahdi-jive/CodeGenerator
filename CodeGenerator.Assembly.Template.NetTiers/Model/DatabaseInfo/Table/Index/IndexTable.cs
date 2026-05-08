namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index
{
    public class IndexTable : SchemaObject, IIndexTable
    {
        public IndexTable(string name, int objectId, string? description, bool isPrimaryKey, ICollection<int> columnsId) : base(name, objectId, description)
        {
            ColumnsId = columnsId;
            IsPrimaryKey = isPrimaryKey;
        }

        public ICollection<int> ColumnsId { get; private set; }

        public bool IsPrimaryKey { get; private set; }
    }
}
