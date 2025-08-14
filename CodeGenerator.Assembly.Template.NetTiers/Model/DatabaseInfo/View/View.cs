using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public class View : SchemaObject, IView
    {
        public IEnumerable<IColumnView> Columns { get => _Columns.Value; }
        private Lazy<IEnumerable<IColumnView>> _Columns { get; set; }
        public IEnumerable<IStoredProcedure> StoredProcedures { get => _StoredProcedures.Value; }
        private Lazy<IEnumerable<IStoredProcedure>> _StoredProcedures { get; set; }
        public View(string name, int objectId, string? description, Lazy<IEnumerable<IColumnView>> columns, Lazy<IEnumerable<IStoredProcedure>> storedProcedures)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
        }
    }
}
