using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseInfoModel : IDatabaseTables, IDatabaseViews, IDatabaseEnums, IDatabaseStoredProcedure
    {
    }
}
