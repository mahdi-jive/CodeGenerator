namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter
{
    public interface IParameterProcedure : ISchemaObject
    {
        public DataTypeSql DataType { get; }
        public bool IsTableType { get; }
        public int MaxLength { get; }
        public bool IsOutput { get; }
        public int ParameterOrder { get; }
        public bool IsRequired { get; }
        public string ToStringParamsWithoutType();
        public string ToStringParamsCSharpType();
        public string ToStringParamsSystemType();
    }
}
