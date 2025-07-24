using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class StoredProcedureInfo : SchemaObject
    {

        public IReadOnlyCollection<ProceduresParameterInfo> ParametersInfo { get; set; }
        public IReadOnlyCollection<StoredProcedureOutputColumn> OutputColumn { get; set; }
        public string Body { get; set; }


    }
}
