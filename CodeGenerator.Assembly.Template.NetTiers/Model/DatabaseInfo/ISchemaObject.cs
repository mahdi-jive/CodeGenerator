namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public interface ISchemaObject
    {
        public string Name { get; }
        public string NamePascal { get; }
        public string NameCamel { get; }
        public int ObjectId { get; }
        public string? Description { get; }
    }
}
