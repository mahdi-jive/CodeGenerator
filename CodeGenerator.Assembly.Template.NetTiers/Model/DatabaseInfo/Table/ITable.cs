using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public interface ITable : ISchemaObject
    {
        IReadOnlyCollection<IColumnTable> Columns { get; }
        IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    }


}
