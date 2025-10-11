using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;

namespace CodeGenerator.Assembly.Template.NetTiers.Extensions
{
    public static class DataModelExtensions
    {
        public static async Task<string> ToColumnsFieldList(this ITable table, bool includeNullable = true)
        {
            var columns = await table.Columns;
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            return string.Join(", ",
                columns.Select(col =>
                {
                    var baseType = SqlToCSharpType.Map.TryGetValue(col.DataType, out var type)
                        ? type
                        : "System.Object";

                    // تعیین Nullable برای انواع Value Type
                    if (includeNullable &&
                        baseType != "System.String" &&
                        baseType != "System.Object" &&
                        !baseType.EndsWith("[]"))
                    {
                        baseType += "?";
                    }

                    return $"{baseType} _{col.NameCamel}";
                }));
        }
    }
}
