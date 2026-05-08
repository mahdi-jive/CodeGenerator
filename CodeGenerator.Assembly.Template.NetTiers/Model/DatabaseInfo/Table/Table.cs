using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;
using System.Text.RegularExpressions;

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

        public Task<IEnumerable<IRelationTable>> ReferencedBy { get => _ReferencedBy.Value; }
        private Lazy<Task<IEnumerable<IRelationTable>>> _ReferencedBy { get; set; }

        public Task<IEnumerable<IIndexTable>> Indexs { get => _Indexs.Value; }
        private Lazy<Task<IEnumerable<IIndexTable>>> _Indexs { get; set; }


        public Table(string name, int objectId, string? description,
            Lazy<Task<IEnumerable<IColumnTable>>> columns,
            Lazy<Task<IEnumerable<IStoredProcedure>>> storedProcedures,
            Lazy<Task<IEnumerable<IRelationTable>>> relations,
            Lazy<Task<IEnumerable<IRelationTable>>> referencedBy,
            Lazy<Task<IEnumerable<IIndexTable>>> indexs)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
            _Relations = relations;
            _ReferencedBy = referencedBy;
            _Indexs = indexs;
        }

        public async Task<IEnumerable<IColumnTable>> GetColumnsIndexAsync(IIndexTable index)
        {
            var columns = (await Columns).DistinctBy(c => c.Name);

            var columnsIndex = columns.Where(q => index.ColumnsId.Any(i => i == q.ObjectId)).OrderBy(q => q.ObjectId);
            return columnsIndex;
        }
        public async Task<IOutputProcedure> GetOutputStoredProceduresAsync(IStoredProcedure storedProcedure)
        {
            IOutputProcedure result;
            var outputProcedures = await storedProcedure.OutputColumn;
            if (outputProcedures.Any())
            {
                var columns = (await Columns);
                var outputNameList = outputProcedures.Select(q => q.Name.ToLower());
                var columnsNameList = columns.Select(q => q.Name.ToLower());
                var exceptOutputColumns = outputNameList.Except(columnsNameList);
                if (exceptOutputColumns.Any())
                {
                    result = OutputProcedure.DataSet();
                }
                else
                {
                    result = OutputProcedure.Enumerable(this);
                }
            }
            else
            {
                result = OutputProcedure.Void();
            }
            return result;
        }
        public string GetMethodNameStoredProcedures(IStoredProcedure storedProcedure)
        {
            return Regex.Replace(storedProcedure.Name, $"sp_{this.Name}_", string.Empty);
        }
    }
}
