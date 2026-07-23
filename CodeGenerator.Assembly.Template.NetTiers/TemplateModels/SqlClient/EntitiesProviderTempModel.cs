using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient
{
    public class EntitiesProviderTempModel : ITemplateModel
    {

        public EntitiesProviderTempModel(ITable table)
        {
            Table = table;
        }
        public ISchemaObject Table { get; private set; }
    }
}
