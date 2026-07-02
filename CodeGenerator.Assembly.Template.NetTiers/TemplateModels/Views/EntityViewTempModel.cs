using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Views
{
    public class EntityViewTempModel
    {

        public EntityViewTempModel(IView view)
        {
            View = view;
        }
        public IView View { get; private set; }
        public string? SummaryText
        {
            get
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(View.Description))
                {
                    result = $"An object representation of the '{View.Name}' table. [No description found in the database]";
                }
                else
                {
                    result = View.Description?.Replace("\r", "").Replace("\n", "\n			///");
                }
                return result;
            }
        }
    }
}
