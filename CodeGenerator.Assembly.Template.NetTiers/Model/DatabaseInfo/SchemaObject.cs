using CodeGenerator.Assembly.Template.NetTiers.Extensions;

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
        public string NamePascal { get => Name.PascalCaseCustom(); }
        public string NameCamel { get => Name.GetCamelCaseName(); }

        public int ObjectId { get; set; }
        public string? Description { get; set; }
        public string? _Description { get; set; }
    }
}
