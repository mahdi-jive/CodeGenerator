using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
namespace CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions
{
    public interface IDatabaseStoredProcedure
    {
        public IStoredProcedureCollection StoredProcedures { get; }
    }
}
