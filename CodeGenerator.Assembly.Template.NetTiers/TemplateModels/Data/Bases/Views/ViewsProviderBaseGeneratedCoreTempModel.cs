using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data.Bases.Views
{
    public class ViewsProviderBaseGeneratedCoreTempModel : ITemplateModel
    {
        public ISchemaObject View { get => _view; }
        private IView _view { get; set; }
        public IEnumerable<IColumnView> Columns { get; private set; }

        public IEnumerable<IStoredProcedure> StoredProcedures { get; private set; }
        public IEnumerable<IStoredProcedure> StoredProceduresCustom { get => StoredProcedures.Where(q => q.Name.StartsWith("sp_")); }

        public async static Task<ViewsProviderBaseGeneratedCoreTempModel> CreateModel(IView view)
        {
            var columns = (await view.Columns);
            var storedProcedures = (await view.StoredProcedures).OrderByDescending(q => q.ObjectId);
            return new ViewsProviderBaseGeneratedCoreTempModel(view, columns, storedProcedures);
        }
        private ViewsProviderBaseGeneratedCoreTempModel(IView view, IEnumerable<IColumnView> columnViews,
            IEnumerable<IStoredProcedure> storedProcedure)
        {
            _view = view;
            Columns = columnViews;
            StoredProcedures = storedProcedure;
        }


        public Task<IOutputProcedure> GetOutputStoredProceduresAsync(IStoredProcedure storedProcedure)
        {
            return _view.GetOutputStoredProceduresAsync(storedProcedure);
        }
        public string GetMethodNameStoredProcedures(IStoredProcedure storedProcedure)
        {
            return _view.GetMethodNameStoredProcedures(storedProcedure);
        }
    }
}
