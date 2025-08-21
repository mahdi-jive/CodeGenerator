using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public class StoredProcedure : SchemaObject, IStoredProcedure
    {
        public StoredProcedure(string name, int objectId, string? description, Lazy<Task<IEnumerable<IParameterProcedure>>> parametersInfo, Lazy<Task<IEnumerable<IOutputProcedure>>> outputColumn)
            : base(name, objectId, description)
        {
            _ParametersInfo = parametersInfo;
            _OutputColumn = outputColumn;
        }
        public Task<IEnumerable<IParameterProcedure>> ParametersInfo { get => _ParametersInfo.Value; }
        private Lazy<Task<IEnumerable<IParameterProcedure>>> _ParametersInfo { get; set; }
        public Task<IEnumerable<IOutputProcedure>> OutputColumn { get => _OutputColumn.Value; }
        public Lazy<Task<IEnumerable<IOutputProcedure>>> _OutputColumn { get; set; }


    }
}
