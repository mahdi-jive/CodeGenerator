using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Views
{
    public class EntityViewGeneratedTempModel
    {
        public async static Task<EntityViewGeneratedTempModel> CreateModel(IView view)
        {
            var columns = (await view.Columns).OrderBy(c => c.Name);
            return new EntityViewGeneratedTempModel(view, columns);
        }
        public EntityViewGeneratedTempModel(IView view, IOrderedEnumerable<IColumnView> columns)
        {
            _view = view;
            Columns = columns;
        }
        public IEnumerable<IColumnView> Columns { get; private set; }
        public ISchemaObject View { get => _view; }
        private IView _view { get; set; }
        public string? SummaryText
        {
            get
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(View.Description))
                {
                    result = $"An object representation of the '{_view.Name}' view. [No description found in the database]";
                }
                else
                {
                    result = View.Description;
                }
                return result;
            }
        }
    }
}
