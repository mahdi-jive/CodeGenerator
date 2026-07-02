using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data
{
    public class DataRepositoryTempModel : ITemplateModel
    {
        public DataRepositoryTempModel(List<ISchemaObject> tablesAndViews)
        {
            TablesAndViews = tablesAndViews;
        }

        public List<ISchemaObject> TablesAndViews { get; private set; }
    }
}
