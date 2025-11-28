using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Extensions
{
    public static class DataModelExtensions
    {
        public static string ToStringParamsCSharpType(this IEnumerable<IColumnTable> columns)
        {
            return string.Join(", ",
                columns.Select(col =>
                {
                    var baseType = col.DataType.CSharpType;
                    if (baseType != "string" &&
                        baseType != "object" &&
                        !baseType.EndsWith("[]") &&
                        col.IsNullable)
                    {
                        baseType += "?";
                    }

                    return $"{baseType} _{col.NameCamel}";
                }));
        }
        public static string ToStringParamsSystemType(this IEnumerable<IColumnTable> columns)
        {
            return string.Join(", ",
                columns.Select(col =>
                {
                    var baseType = col.DataType.SystemType;
                    if (baseType != "System.String" &&
                        baseType != "System.Object" &&
                        !baseType.EndsWith("[]") &&
                         col.IsNullable)
                    {
                        baseType += "?";
                    }

                    return $"{baseType} _{col.NameCamel}";
                }));
        }
    }
}
