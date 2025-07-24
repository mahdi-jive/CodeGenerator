using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Tables;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseTables : IDataModel
    {
        IReadOnlyCollection<string> SelectedTables { get; }
        ITableCollection Tables { get; }
    }


}
