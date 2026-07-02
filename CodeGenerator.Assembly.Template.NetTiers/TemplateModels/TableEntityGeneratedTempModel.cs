using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels
{
    public class TableEntityGeneratedTempModel : ITemplateModel
    {


        //       var summaryText = string.IsNullOrEmpty(Model.Description)
        //           ? $"An object representation of the '{Model.Name}' table. [No description found in the database]"
        //           : Model.Description?.Replace("\r", "").Replace("\n", "\n			///");

        //       var columns = (await Model.Columns).DistinctBy(c => c.Name);
        //       var columnsWithoutPK = columns.Where(c => !c.IsPrimaryKey);
        //       var relations = (await Model.Relations).OrderBy(q => q.TableName);
        //       var referencedBy = (await Model.ReferencedBy).OrderBy(q => q.ObjectId);
        //       var primaryKey = columns.FirstOrDefault(q => q.IsPrimaryKey);
        //if (primaryKey==null)
        //{
        //	throw new NullReferenceException(Model.Name);
        //}
        public ISchemaObject Table { get => _table; }
        private ITable _table { get; set; }
        public IEnumerable<IColumnTable> Columns { get; private set; }
        public IEnumerable<IColumnTable> ColumnsWithoutPK { get => Columns.Where(c => !c.IsPrimaryKey); }
        public IEnumerable<IColumnTable> ColumnsWithoutIdentity { get => Columns.Where(c => !c.IsIdentity); }
        public IEnumerable<IColumnTable> ColumnsWithoutHasLength { get => Columns.Where(c => !c.HasLength); }
        public IColumnTable PrimaryKey
        {
            get
            {
                return Columns.First(q => q.IsPrimaryKey); ;
            }
        }
        public IEnumerable<IRelationTable> Relations { get; private set; }
        public IEnumerable<IRelationTable> ReferencedBy { get; private set; }
        public async static Task<TableEntityGeneratedTempModel> CreateModel(ITable table)
        {
            var columns = (await table.Columns).OrderBy(c => c.Name);
            var relations = (await table.Relations).OrderBy(q => q.Name);
            var referencedBy = (await table.ReferencedBy).OrderBy(q => q.Name);
            var Indexes = (await table.Indexs).OrderByDescending(q => q.Name);
            var storedProcedures = (await table.StoredProcedures).OrderByDescending(q => q.ObjectId);
            return new TableEntityGeneratedTempModel(table, columns, relations, referencedBy, Indexes, storedProcedures);
        }
        private TableEntityGeneratedTempModel(ITable table, IEnumerable<IColumnTable> columnTables,
            IEnumerable<IRelationTable> relations,
            IEnumerable<IRelationTable> referencedBy,
            IEnumerable<IIndexTable> indexes,
            IEnumerable<IStoredProcedure> storedProcedure)
        {
            _table = table;
            Columns = columnTables;
            Relations = relations;
            ReferencedBy = referencedBy;
        }
        public IColumnTable GetColumnRelation(IRelationTable relation)
        {
            return Columns.First(c => c.ObjectId == relation.ColumnId);
        }
        public Task<IEnumerable<IColumnTable>> GetColumnsIndexAsync(IIndexTable index)
        {
            return _table.GetColumnsIndexAsync(index);
        }
        public string? SummaryText
        {
            get
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(Table.Description))
                {
                    result = $"An object representation of the '{Table.Name}' table. [No description found in the database]";
                }
                else
                {
                    result = Table.Description?.Replace("\r", "").Replace("\n", "\n			///");
                }
                return result;
            }
        }
    }
}
