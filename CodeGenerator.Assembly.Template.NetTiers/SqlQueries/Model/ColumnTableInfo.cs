using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class ColumnTableInfo : SchemaObject
    {
        public string TableName { get; set; } = null!;
        public int TableObjectId { get; set; }
        public string DataType { get; set; } = null!;
        public short MaxLength { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public string? Collation { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
        public bool IsRowGuid { get; set; }
        public bool IsSparse { get; set; }
        public string? GeneratedAlwaysType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string? DefaultValue { get; set; }
    }
}
