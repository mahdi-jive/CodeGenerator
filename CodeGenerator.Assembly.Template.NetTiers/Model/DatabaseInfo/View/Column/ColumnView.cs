namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column
{
    public class ColumnView : SchemaObject, IColumnView
    {
        public ColumnView(string name, int objectId, string? description, string viewName, int viewObjectId, int maxLength, string dataType, string? collation, bool isNullable)
            : base(name, objectId, description)
        {
            ViewName = viewName;
            ViewObjectId = viewObjectId;
            MaxLength = maxLength;
            DataType = new DataTypeSql(dataType);
            Collation = collation;
            IsNullable = isNullable;
        }

        public string ViewName { get; private set; } = null!;
        public int ViewObjectId { get; private set; }
        public DataTypeSql DataType { get; private set; }
        public string? Collation { get; private set; }
        public bool IsNullable { get; private set; }

        public int MaxLength { get; private set; }

        public bool HasLength
        {
            get
            {
                return (DataType.DataTypeSqlEnum is
                    DataTypeSqlEnum.charSql or
                    DataTypeSqlEnum.varchar or
                    DataTypeSqlEnum.nchar or
                    DataTypeSqlEnum.nvarchar or
                    DataTypeSqlEnum.binary or
                    DataTypeSqlEnum.varbinary) && MaxLength != -1;
            }
        }

        public string ToStringParamsSystemType()
        {
            var baseType = DataType.SystemType;
            if (baseType != "System.String" &&
                baseType != "System.Object" &&
                !baseType.EndsWith("[]") &&
                 IsNullable)
            {
                baseType += "?";
            }

            return $"{baseType} _{NameCamel}";

        }
    }
}
