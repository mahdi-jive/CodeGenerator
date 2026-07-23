using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient.Views
{
    internal class ViewProviderGeneratedTempModel : ITemplateModel
    {
        public ISchemaObject View { get => _view; }
        private IView _view { get; set; }
        public IEnumerable<IColumnView> Columns { get; private set; }
        public IEnumerable<IStoredProcedure> StoredProcedures { get; private set; }
        public IEnumerable<IStoredProcedure> StoredProceduresCustom { get => StoredProcedures.Where(q => q.Name.StartsWith("sp_")).OrderByDescending(q => q.ObjectId); }

        public async static Task<ViewProviderGeneratedTempModel> CreateModel(IView view)
        {
            var columns = (await view.Columns).OrderBy(c => c.ObjectId);
            var storedProcedures = (await view.StoredProcedures).OrderBy(q => q.ObjectId);
            return new ViewProviderGeneratedTempModel(view, columns, storedProcedures);
        }
        private ViewProviderGeneratedTempModel(IView view, IEnumerable<IColumnView> columnViews,
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
