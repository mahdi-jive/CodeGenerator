using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public interface ITable : ISchemaObject
    {
        Task<IEnumerable<IColumnTable>> Columns { get; }
        Task<IEnumerable<IStoredProcedure>> StoredProcedures { get; }
        Task<IEnumerable<IRelationTable>> Relations { get; }
    }


}
