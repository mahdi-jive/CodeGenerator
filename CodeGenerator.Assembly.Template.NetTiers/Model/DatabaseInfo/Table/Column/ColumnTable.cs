namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column
{
    public class ColumnTable : SchemaObject, IColumnTable
    {
        public ColumnTable(string name, int objectId, string? description, string tableName, int tableObjectId, string dataType, int maxLength, int precision, int scale, string? collation, bool isNullable, bool isIdentity, bool isComputed, bool isRowGuid, bool isSparse, string? generatedAlwaysType, bool isPrimaryKey, string? defaultValue)
            : base(name, objectId, description)
        {
            Name = name;
            ObjectId = objectId;
            Description = description;
            TableName = tableName;
            TableObjectId = tableObjectId;
            DataType = dataType;
            MaxLength = maxLength;
            Precision = precision;
            Scale = scale;
            Collation = collation;
            IsNullable = isNullable;
            IsIdentity = isIdentity;
            IsComputed = isComputed;
            IsRowGuid = isRowGuid;
            IsSparse = isSparse;
            GeneratedAlwaysType = generatedAlwaysType;
            IsPrimaryKey = isPrimaryKey;
            DefaultValue = defaultValue;
        }
        public string TableName { get; private set; } = null!;
        public int TableObjectId { get; private set; }
        public string DataType { get; private set; } = null!;
        public string CSharpType { get => SqlToCSharpType.GetCSharpType(DataType, IsNullable); }
        public string DbType { get => SqlToDbType.GetDbType(DataType); }
        public int MaxLength { get; private set; }
        public int Precision { get; private set; }
        public int Scale { get; private set; }
        public string? Collation { get; private set; }
        public bool IsNullable { get; private set; }
        public bool IsIdentity { get; private set; }
        public bool IsComputed { get; private set; }
        public bool IsRowGuid { get; private set; }
        public bool IsSparse { get; private set; }
        public string? GeneratedAlwaysType { get; private set; }
        public bool IsPrimaryKey { get; private set; }
        public string? DefaultValue { get; private set; }

    }
}
