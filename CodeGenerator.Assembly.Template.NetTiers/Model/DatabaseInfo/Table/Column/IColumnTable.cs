namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column
{
    public interface IColumnTable : ISchemaObject
    {
        string TableName { get; }
        int TableObjectId { get; }
        DataTypeSql DataType { get; }
        int MaxLength { get; }
        bool HasLength { get; }
        int Precision { get; }
        int Scale { get; }
        string? Collation { get; }
        bool IsNullable { get; }
        bool IsIdentity { get; }
        bool IsComputed { get; }
        bool IsRowGuid { get; }
        bool IsSparse { get; }
        string? GeneratedAlwaysType { get; }
        bool IsPrimaryKey { get; }
        string? DefaultValue { get; }
    }
}
