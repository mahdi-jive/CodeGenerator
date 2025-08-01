using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public class View : SchemaObject, IView
    {
        public IReadOnlyCollection<IColumnView> Columns { get; private set; }
        public IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; private set; }
        public View(string name, int objectId, string? description, IReadOnlyCollection<IColumnView> columns, IReadOnlyCollection<IStoredProcedure> storedProcedures)
            : base(name, objectId, description)
        {
            Columns = columns;
            StoredProcedures = storedProcedures;
        }
    }
}
