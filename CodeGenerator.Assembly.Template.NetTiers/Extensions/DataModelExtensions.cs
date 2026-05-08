using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using System.Text.RegularExpressions;

namespace CodeGenerator.Assembly.Template.NetTiers.Extensions
{
    public static class DataModelExtensions
    {
        public static string ToStringParamsWithoutType(this IEnumerable<IColumnTable> columns)
        {
            return string.Join(", ",
                columns.Select(col => col.ToStringParamsWithoutType()));

        }
        public static string ToStringParamsCSharpType(this IEnumerable<IColumnTable> columns)
        {
            return string.Join(", ",
                columns.Select(col => col.ToStringParamsCSharpType()));
        }
        public static string ToStringParamsSystemType(this IEnumerable<IColumnTable> columns)
        {
            return string.Join(", ",
                columns.Select(col => col.ToStringParamsSystemType()));
        }
        public static string ToStringParamsSystemType(this IEnumerable<IColumnView> columns)
        {
            return string.Join(", ",
                columns.Select(col => col.ToStringParamsSystemType()));
        }

        public static string ToStringParamsWithoutType(this IEnumerable<IParameterProcedure> parameters)
        {
            return string.Join(", ",
                parameters.Select(param => param.ToStringParamsWithoutType()));

        }
        public static string ToStringParamsCSharpType(this IEnumerable<IParameterProcedure> parameters)
        {
            return string.Join(", ",
                parameters.Select(param => param.ToStringParamsCSharpType()));
        }
        public static string ToStringParamsSystemType(this IEnumerable<IParameterProcedure> parameters)
        {
            return string.Join(", ",
                parameters.Select(param => param.ToStringParamsSystemType()));
        }
        public static string RemoveSQlComments(this string text)
        {
            // حذف کامنت‌های چند خطی /* ... */
            string noBlockComments = Regex.Replace(text, @"/\*.*?\*/", "", RegexOptions.Singleline);

            // حذف کامنت‌های تک خطی --
            string noLineComments = Regex.Replace(noBlockComments, @"--[^\r\n]*", "");

            return noLineComments;
        }
    }
}
