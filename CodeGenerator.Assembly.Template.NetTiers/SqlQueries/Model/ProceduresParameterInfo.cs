using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class ProceduresParameterInfo : SchemaObject
    {

        public string ParameterType { get; set; }
        public bool IsTableType { get; set; }
        public short MaxLength { get; set; }
        public bool IsOutput { get; set; }
        public int ParameterOrder { get; set; }
        public string? ParameterDescription { get; set; }
    }
}
