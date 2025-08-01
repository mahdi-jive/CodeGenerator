using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public interface IStoredProcedure : ISchemaObject
    {
        public IReadOnlyCollection<IParameterProcedure> ParametersInfo { get; }
        public IReadOnlyCollection<IOutputProcedure> OutputColumn { get; }
    }


}
