using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public class View : SchemaObject, IView
    {
        public Task<IEnumerable<IColumnView>> Columns { get => _Columns.Value; }
        private Lazy<Task<IEnumerable<IColumnView>>> _Columns { get; set; }
        public Task<IEnumerable<IStoredProcedure>> StoredProcedures { get => _StoredProcedures.Value; }
        private Lazy<Task<IEnumerable<IStoredProcedure>>> _StoredProcedures { get; set; }
        public View(string name, int objectId, string? description, Lazy<Task<IEnumerable<IColumnView>>> columns, Lazy<Task<IEnumerable<IStoredProcedure>>> storedProcedures)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
        }
    }
}
