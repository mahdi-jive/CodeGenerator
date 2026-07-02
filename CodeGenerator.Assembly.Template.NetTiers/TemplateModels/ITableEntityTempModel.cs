using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels
{
    public class ITableEntityTempModel : ITemplateModel
    {
        //        @model CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.ITable
        //@{

        //            var summaryText = string.IsNullOrEmpty(Model.Description)
        //                ? $"An object representation of the '{Model.Name}' table. [No description found in the database]"
        //                : Model.Description;

        //            var columns = (await Model.Columns).DistinctBy(q => q.Name);
        //            var referencedBy = (await Model.ReferencedBy).OrderBy(q => q.TableName);
        //            var primaryKey = columns.FirstOrDefault(q => q.IsPrimaryKey);
        //            if (primaryKey == null)
        //            {
        //                throw new NullReferenceException(Model.Name);
        //            }

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
        public IEnumerable<IRelationTable> ReferencedBy { get; private set; }

        public async static Task<ITableEntityTempModel> CreateModel(ITable table)
        {
            var columns = (await table.Columns).OrderBy(c => c.Name);
            var referencedBy = (await table.ReferencedBy).OrderBy(q => q.Name);
            return new ITableEntityTempModel(table, columns, referencedBy);
        }
        private ITableEntityTempModel(ITable table, IEnumerable<IColumnTable> columnTables,
            IEnumerable<IRelationTable> referencedBy)
        {
            _table = table;
            Columns = columnTables;
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
    }
}
