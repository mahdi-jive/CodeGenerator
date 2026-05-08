using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public class StoredProcedure : SchemaObject, IStoredProcedure
    {
        public StoredProcedure(string name, int objectId, string? description, string procedureText, Lazy<Task<IEnumerable<IParameterProcedure>>> parametersInfo, Lazy<Task<IEnumerable<IOutputColumnProcedure>>> outputColumn)
            : base(name, objectId, description)
        {
            ProcedureText = procedureText;
            _ParametersInfo = parametersInfo;
            _OutputColumn = outputColumn;
        }
        public Task<IEnumerable<IParameterProcedure>> ParametersInfo { get => _ParametersInfo.Value; }
        private Lazy<Task<IEnumerable<IParameterProcedure>>> _ParametersInfo { get; set; }
        public Task<IEnumerable<IOutputColumnProcedure>> OutputColumn { get => _OutputColumn.Value; }
        private Lazy<Task<IEnumerable<IOutputColumnProcedure>>> _OutputColumn { get; set; }

        public string ProcedureText { get; private set; }





    }
}
