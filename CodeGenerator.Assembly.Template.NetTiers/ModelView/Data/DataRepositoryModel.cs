using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.ModelView.Data
{
    public class DataRepositoryModel
    {
        public DataRepositoryModel(Lazy<Task<IEnumerable<ITable>>> tables, Lazy<Task<IEnumerable<IView>>> views)
        {
            _Tables = tables;
            _Views = views;
        }

        private Lazy<Task<IEnumerable<ITable>>> _Tables { get; set; }
        private Lazy<Task<IEnumerable<IView>>> _Views { get; set; }
        public Task<IEnumerable<ITable>> Tables => _Tables.Value;
        public Task<IEnumerable<IView>> Views => _Views.Value;
    }
}
