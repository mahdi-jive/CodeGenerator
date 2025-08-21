using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public class Table : SchemaObject, ITable
    {
        public Task<IEnumerable<IColumnTable>> Columns { get => _Columns.Value; }
        private Lazy<Task<IEnumerable<IColumnTable>>> _Columns { get; set; }
        public Task<IEnumerable<IStoredProcedure>> StoredProcedures { get => _StoredProcedures.Value; }
        private Lazy<Task<IEnumerable<IStoredProcedure>>> _StoredProcedures { get; set; }

        public Task<IEnumerable<IRelationTable>> Relations { get => _Relations.Value; }
        private Lazy<Task<IEnumerable<IRelationTable>>> _Relations { get; set; }

        public Table(string name, int objectId, string? description, Lazy<Task<IEnumerable<IColumnTable>>> columns, Lazy<Task<IEnumerable<IStoredProcedure>>> storedProcedures, Lazy<Task<IEnumerable<IRelationTable>>> relations)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
            _Relations = relations;
        }
    }
}
