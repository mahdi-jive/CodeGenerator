using CodeGenerator.Assembly.Template.NetTiers.Extensions;
using System.Text.RegularExpressions;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter
{
    public class ParameterProcedure : SchemaObject, IParameterProcedure
    {
        public ParameterProcedure(string name, int objectId, string? description, DataTypeSql dataType, bool isTableType, int maxLength, bool isOutput, int parameterOrder, string procedureText)
            : base(name, objectId, description)
        {
            DataType = dataType;
            IsTableType = isTableType;
            MaxLength = maxLength;
            IsOutput = isOutput;
            ParameterOrder = parameterOrder;

            IsRequired = IsParameterRequired(procedureText, name);
        }
        public bool IsTableType { get; private set; }
        public int MaxLength { get; private set; }
        public bool IsOutput { get; private set; }
        public int ParameterOrder { get; private set; }
        public DataTypeSql DataType { get; private set; }
        public bool IsRequired { get; private set; }

        public string ToStringParamsWithoutType()
        {
            return $"{NameCamel}";
        }
        public string ToStringParamsCSharpType()
        {
            var baseType = DataType.CSharpType;
            if (baseType != "string" &&
                baseType != "object" &&
                !baseType.EndsWith("[]") &&
                !IsRequired)
            {
                baseType += "?";
            }

            return $"{baseType} {NameCamel}";
        }
        public string ToStringParamsSystemType()
        {
            var baseType = DataType.SystemType;
            if (baseType != "System.String" &&
                baseType != "System.Object" &&
                !baseType.EndsWith("[]") &&
                 !IsRequired)
            {
                baseType += "?";
            }

            return $"{baseType} {NameCamel}";
        }
        public bool IsParameterRequired(string procedureText, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(procedureText) || string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("متن SP و نام پارامتر نمی‌تواند خالی باشد");

            string cleanText = procedureText.RemoveSQlComments();

            string pattern = $@"{Regex.Escape(parameterName)}\s+\w+(?:\([^)]*\))?\s*=\s*(?:DEFAULT|NULL|[^,\s]+)";

            bool hasDefaultValue = Regex.IsMatch(cleanText, pattern, RegexOptions.IgnoreCase);

            return !hasDefaultValue;
        }
    }
}
