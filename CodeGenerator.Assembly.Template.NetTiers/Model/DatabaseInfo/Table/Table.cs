using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public class Table : SchemaObject, ITable
    {
        public IEnumerable<IColumnTable> Columns { get => _Columns.Value; }
        private Lazy<IEnumerable<IColumnTable>> _Columns { get; set; }
        public IEnumerable<IStoredProcedure> StoredProcedures { get => _StoredProcedures.Value; }
        private Lazy<IEnumerable<IStoredProcedure>> _StoredProcedures { get; set; }
        public Table(string name, int objectId, string? description, Lazy<IEnumerable<IColumnTable>> columns, Lazy<IEnumerable<IStoredProcedure>> storedProcedures)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
        }
    }
}
