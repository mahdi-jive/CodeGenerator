using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Index;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public interface ITable : ISchemaObject
    {
        Task<IEnumerable<IColumnTable>> Columns { get; }
        Task<IEnumerable<IStoredProcedure>> StoredProcedures { get; }
        Task<IEnumerable<IRelationTable>> Relations { get; }
        Task<IEnumerable<IRelationTable>> ReferencedBy { get; }
        Task<IEnumerable<IIndexTable>> Indexs { get; }

        Task<IEnumerable<IColumnTable>> GetColumnsIndexAsync(IIndexTable index);
        Task<IOutputProcedure> GetOutputStoredProceduresAsync(IStoredProcedure storedProcedure);
        string GetMethodNameStoredProcedures(IStoredProcedure storedProcedure);

    }


}
