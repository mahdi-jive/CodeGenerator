namespace CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions
{
    public class SchemaObject : ISchemaObject
    {
        public string Name { get; set; } = null!;

        public int ObjectId { get; set; }
        public string? Description { get; set; }
    }
}
