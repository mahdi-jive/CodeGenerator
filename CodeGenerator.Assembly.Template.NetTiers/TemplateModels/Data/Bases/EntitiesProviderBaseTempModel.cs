using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data.Bases
{
    public class EntitiesProviderBaseTempModel : ITemplateModel
    {
        public EntitiesProviderBaseTempModel(ISchemaObject table)
        {
            Table = table;
        }

        public ISchemaObject Table { get; private set; }
    }
}
