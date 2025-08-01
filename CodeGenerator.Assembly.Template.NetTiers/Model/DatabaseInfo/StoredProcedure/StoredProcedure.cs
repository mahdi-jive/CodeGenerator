using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public class StoredProcedure : SchemaObject, IStoredProcedure
    {
        public StoredProcedure(string name, int objectId, string? description, IReadOnlyCollection<IParameterProcedure> parametersInfo, IReadOnlyCollection<IOutputProcedure> outputColumn)
            : base(name, objectId, description)
        {
            ParametersInfo = parametersInfo;
            OutputColumn = outputColumn;
        }
        public IReadOnlyCollection<IParameterProcedure> ParametersInfo { get; set; }
        public IReadOnlyCollection<IOutputProcedure> OutputColumn { get; set; }


    }
}
