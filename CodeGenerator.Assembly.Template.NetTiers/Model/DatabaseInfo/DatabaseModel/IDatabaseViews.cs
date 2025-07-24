using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseViews : IDataModel
    {
        IReadOnlyCollection<string> SelectedViews { get; }
        IViewCollection Views { get; }
    }


}
