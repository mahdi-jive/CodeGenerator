using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data.Bases.Views
{
    public class ViewsProviderBaseTempModel : ITemplateModel
    {
        public ViewsProviderBaseTempModel(ISchemaObject view)
        {
            View = view;
        }

        public ISchemaObject View { get; private set; }
    }
}
