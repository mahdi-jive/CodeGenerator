using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public class Table : SchemaObject, ITable
    {
        public IReadOnlyCollection<IColumnTable> Columns { get; private set; }
        public IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; private set; }
        public Table(string name, int objectId, string? description, IReadOnlyCollection<IColumnTable> columns, IReadOnlyCollection<IStoredProcedure> storedProcedures)
            : base(name, objectId, description)
        {
            Columns = columns;
            StoredProcedures = storedProcedures;
        }
    }
}
