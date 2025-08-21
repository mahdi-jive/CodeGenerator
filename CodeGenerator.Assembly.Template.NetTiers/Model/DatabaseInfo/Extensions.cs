using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public static class Extensions
    {
        public static string MapSqlTypeToCSharp(this IColumnTable column)
        {
            return column.DataType.MapSqlTypeToCSharp(column.IsNullable);
        }
        public static string MapSqlTypeToCSharp(this IColumnView column)
        {
            return column.DataType.MapSqlTypeToCSharp(column.IsNullable);
        }
        public static string MapSqlTypeToCSharp(this string sqlType, bool isNullable = false)
        {
            string csharpType = sqlType.ToLower() switch
            {
                "bigint" => "System.Int64",
                "int" => "System.Int32",
                "smallint" => "System.Int16",
                "tinyint" => "System.Byte",
                "bit" => "System.Boolean",
                "decimal" => "System.Decimal",
                "numeric" => "System.Decimal",
                "money" => "System.Decimal",
                "smallmoney" => "System.Decimal",
                "float" => "System.Double",
                "real" => "System.Single",
                "date" => "System.DateTime",
                "datetime" => "System.DateTime",
                "datetime2" => "System.DateTime",
                "smalldatetime" => "System.DateTime",
                "datetimeoffset" => "System.DateTimeOffset",
                "time" => "System.TimeSpan",
                "char" => "System.String",
                "nchar" => "System.String",
                "varchar" => "System.String",
                "nvarchar" => "System.String",
                "text" => "System.String",
                "ntext" => "System.String",
                "binary" => "byte[]",
                "varbinary" => "byte[]",
                "image" => "byte[]",
                "uniqueidentifier" => "System.Guid",
                "xml" => "System.String",
                _ => "System.Object" // fallback برای نوع ناشناخته
            };

            // اگر نوع nullable است و value type است، ? اضافه کن
            if (isNullable && csharpType.StartsWith("System.") &&
                csharpType != "System.String" && csharpType != "byte[]" && csharpType != "System.Object")
            {
                csharpType += "?";
            }

            return csharpType;
        }

    }
}
