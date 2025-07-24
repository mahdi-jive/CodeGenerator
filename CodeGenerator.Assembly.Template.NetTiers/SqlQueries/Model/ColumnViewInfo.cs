using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class ColumnViewInfo : SchemaObject
    {
        public string ViewName { get; set; } = null!;
        public int ViewObjectId { get; set; }
        public string DataType { get; set; } = null!;
        public string? Collation { get; set; }
        public bool IsNullable { get; set; }
    }
}
