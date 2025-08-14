using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public class StoredProcedure : SchemaObject, IStoredProcedure
    {
        public StoredProcedure(string name, int objectId, string? description, Lazy<IEnumerable<IParameterProcedure>> parametersInfo, Lazy<IEnumerable<IOutputProcedure>> outputColumn)
            : base(name, objectId, description)
        {
            _ParametersInfo = parametersInfo;
            _OutputColumn = outputColumn;
        }
        public IEnumerable<IParameterProcedure> ParametersInfo { get => _ParametersInfo.Value; }
        private Lazy<IEnumerable<IParameterProcedure>> _ParametersInfo { get; set; }
        public IEnumerable<IOutputProcedure> OutputColumn { get => _OutputColumn.Value; }
        public Lazy<IEnumerable<IOutputProcedure>> _OutputColumn { get; set; }


    }
}
