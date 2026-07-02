using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels
{
    public class TableEntityTempModel : ITemplateModel
    {

        public TableEntityTempModel(ITable table)
        {
            Table = table;
        }
        public ITable Table { get; private set; }
        public string? SummaryText
        {
            get
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(Table.Description))
                {
                    result = $"An object representation of the '{Table.Name}' table. [No description found in the database]";
                }
                else
                {
                    result = Table.Description?.Replace("\r", "").Replace("\n", "\n			///");
                }
                return result;
            }
        }
    }
}
