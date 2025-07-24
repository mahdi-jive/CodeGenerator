using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseEnums : IDataModel
    {
        IReadOnlyCollection<string> SelectedTableEnums { get; }
        ITableEnumCollection TableEnums { get; }
    }


}
