using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data.Bases
{
    public class EntitiesProviderBaseGeneratedCoreTempModel : ITemplateModel
    {
        public ISchemaObject Table { get => _table; }
        private ITable _table { get; set; }
        public IEnumerable<IColumnTable> Columns { get; private set; }
        public IEnumerable<IColumnTable> ColumnsWithoutPK { get => Columns.Where(c => !c.IsPrimaryKey); }
        public IColumnTable PrimaryKey
        {
            get
            {
                return Columns.First(q => q.IsPrimaryKey); ;
            }
        }
        public IEnumerable<IRelationTable> Relations { get; private set; }
        public IEnumerable<IRelationTable> ReferencedBy { get; private set; }
        public IEnumerable<IIndexTable> Indexes { get; private set; }
        public IEnumerable<IIndexTable> IndexesWithoutPK { get => Indexes.Where(c => !c.IsPrimaryKey); }
        public IIndexTable IndexPrimary
        {
            get
            {
                return Indexes.First(q => q.IsPrimaryKey); ;
            }
        }
        public IEnumerable<IStoredProcedure> StoredProcedures { get; private set; }
        public IEnumerable<IStoredProcedure> StoredProceduresCustom { get => StoredProcedures.Where(q => q.Name.StartsWith("sp_")); }

        public async static Task<EntitiesProviderBaseGeneratedCoreTempModel> CreateModel(ITable table)
        {
            var columns = (await table.Columns).OrderBy(c => c.Name);
            var relations = (await table.Relations).OrderBy(q => q.Name);
            var referencedBy = (await table.ReferencedBy).OrderBy(q => q.Name);
            var Indexes = (await table.Indexs).OrderByDescending(q => q.Name);
            var storedProcedures = (await table.StoredProcedures).OrderByDescending(q => q.ObjectId);
            return new EntitiesProviderBaseGeneratedCoreTempModel(table, columns, relations, referencedBy, Indexes, storedProcedures);
        }
        private EntitiesProviderBaseGeneratedCoreTempModel(ITable table, IEnumerable<IColumnTable> columnTables,
            IEnumerable<IRelationTable> relations,
            IEnumerable<IRelationTable> referencedBy,
            IEnumerable<IIndexTable> indexes,
            IEnumerable<IStoredProcedure> storedProcedure)
        {
            _table = table;
            Columns = columnTables;
            Relations = relations;
            ReferencedBy = referencedBy;
            Indexes = indexes;
            StoredProcedures = storedProcedure;
        }
        public IColumnTable GetColumnRelation(IRelationTable relation)
        {
            return Columns.First(c => c.ObjectId == relation.ColumnId);
        }
        public Task<IEnumerable<IColumnTable>> GetColumnsIndexAsync(IIndexTable index)
        {
            return _table.GetColumnsIndexAsync(index);
        }

        public Task<IOutputProcedure> GetOutputStoredProceduresAsync(IStoredProcedure storedProcedure)
        {
            return _table.GetOutputStoredProceduresAsync(storedProcedure);
        }
        public string GetMethodNameStoredProcedures(IStoredProcedure storedProcedure)
        {
            return _table.GetMethodNameStoredProcedures(storedProcedure);
        }
    }
}
