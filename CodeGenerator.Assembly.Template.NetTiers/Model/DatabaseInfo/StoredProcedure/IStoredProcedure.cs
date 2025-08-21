using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure
{
    public interface IStoredProcedure : ISchemaObject
    {
        public Task<IEnumerable<IParameterProcedure>> ParametersInfo { get; }
        public Task<IEnumerable<IOutputProcedure>> OutputColumn { get; }
    }


}
