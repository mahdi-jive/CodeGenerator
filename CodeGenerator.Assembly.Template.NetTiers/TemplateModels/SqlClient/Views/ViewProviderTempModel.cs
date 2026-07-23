using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient.Views
{
    internal class ViewProviderTempModel : ITemplateModel
    {

        public ViewProviderTempModel(IView view)
        {
            View = view;
        }
        public ISchemaObject View { get; private set; }
    }
}