namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public static class SqlToCSharpType
    {
        public static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            // عددی
            { "tinyint", "System.Byte" },
            { "smallint", "System.Int16" },
            { "int", "System.Int32" },
            { "bigint", "System.Int64" },
            { "bit", "System.Boolean" },
            { "decimal", "System.Decimal" },
            { "numeric", "System.Decimal" },
            { "money", "System.Decimal" },
            { "smallmoney", "System.Decimal" },
            { "float", "System.Double" },
            { "real", "System.Single" },

            // متنی
            { "char", "System.String" },
            { "varchar", "System.String" },
            { "text", "System.String" },
            { "nchar", "System.String" },
            { "nvarchar", "System.String" },
            { "ntext", "System.String" },

            // زمانی
            { "date", "System.DateTime" },
            { "datetime", "System.DateTime" },
            { "datetime2", "System.DateTime" },
            { "smalldatetime", "System.DateTime" },
            { "time", "System.TimeSpan" },
            { "datetimeoffset", "System.DateTimeOffset" },

            // باینری
            { "binary", "System.Byte[]" },
            { "varbinary", "System.Byte[]" },
            { "image", "System.Byte[]" },
            { "rowversion", "System.Byte[]" },
            { "timestamp", "System.Byte[]" },

            // سایر
            { "uniqueidentifier", "System.Guid" },
            { "xml", "System.String" },
            { "sql_variant", "System.Object" },
            { "hierarchyid", "System.String" },
            { "geography", "System.String" },
            { "geometry", "System.String" }
        };
        public static string GetCSharpType(string sqlType, bool isNullable)
        {
            if (!SqlToCSharpType.Map.TryGetValue(sqlType, out var csharpType))
                csharpType = "object";

            if (isNullable && csharpType != "string" && csharpType != "object" && !csharpType.EndsWith("[]"))
                csharpType += "?";

            return csharpType;
        }
    }
}
