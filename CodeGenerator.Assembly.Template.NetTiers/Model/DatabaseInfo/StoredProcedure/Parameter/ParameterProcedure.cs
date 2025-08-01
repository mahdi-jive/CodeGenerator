namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter
{
    public class ParameterProcedure : SchemaObject, IParameterProcedure
    {
        public ParameterProcedure(string name, int objectId, string? description, string parameterType, bool isTableType, int maxLength, bool isOutput, int parameterOrder)
            : base(name, objectId, description)
        {
            ParameterType = parameterType;
            IsTableType = isTableType;
            MaxLength = maxLength;
            IsOutput = isOutput;
            ParameterOrder = parameterOrder;
        }

        public string ParameterType { get; private set; }
        public bool IsTableType { get; private set; }
        public int MaxLength { get; private set; }
        public bool IsOutput { get; private set; }
        public int ParameterOrder { get; private set; }
    }
}
