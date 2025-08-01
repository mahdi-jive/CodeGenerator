namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter
{
    public interface IParameterProcedure : ISchemaObject
    {
        public string ParameterType { get; }
        public bool IsTableType { get; }
        public int MaxLength { get; }
        public bool IsOutput { get; }
        public int ParameterOrder { get; }
    }
}
