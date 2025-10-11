namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public static class SqlToDbType
    {
        public static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        // عددی
        { "tinyint", "DbType.Byte" },
        { "smallint", "DbType.Int16" },
        { "int", "DbType.Int32" },
        { "bigint", "DbType.Int64" },
        { "bit", "DbType.Boolean" },
        { "decimal", "DbType.Decimal" },
        { "numeric", "DbType.Decimal" },
        { "money", "DbType.Currency" },
        { "smallmoney", "DbType.Currency" },
        { "float", "DbType.Double" },
        { "real", "DbType.Single" },

        // متنی
        { "char", "DbType.AnsiStringFixedLength" },
        { "varchar", "DbType.AnsiString" },
        { "text", "DbType.AnsiString" },
        { "nchar", "DbType.StringFixedLength" },
        { "nvarchar", "DbType.String" },
        { "ntext", "DbType.String" },

        // زمانی
        { "date", "DbType.Date" },
        { "datetime", "DbType.DateTime" },
        { "datetime2", "DbType.DateTime2" },
        { "smalldatetime", "DbType.DateTime" },
        { "time", "DbType.Time" },
        { "datetimeoffset", "DbType.DateTimeOffset" },

        // باینری
        { "binary", "DbType.Binary" },
        { "varbinary", "DbType.Binary" },
        { "image", "DbType.Binary" },
        { "rowversion", "DbType.Binary" },
        { "timestamp", "DbType.Binary" },

        // سایر
        { "uniqueidentifier", "DbType.Guid" },
        { "xml", "DbType.Xml" },
        { "sql_variant", "DbType.Object" },
        { "hierarchyid", "DbType.String" },
        { "geography", "DbType.String" },
        { "geometry", "DbType.String" }
    };

        public static string GetDbType(string sqlType)
        {
            if (Map.TryGetValue(sqlType, out var dbType))
                return dbType;

            return "DbType.Object"; // پیش‌فرض برای نوع ناشناخته
        }
    }
}
