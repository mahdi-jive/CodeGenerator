namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public class SchemaObject : ISchemaObject
    {
        public SchemaObject(string name, int objectId, string? description)
        {
            Name = name;
            ObjectId = objectId;
            Description = description;
        }

        public string Name { get; set; } = null!;

        public int ObjectId { get; set; }
        public string? Description { get; set; }
        public string? _Description { get; set; }
    }
}
