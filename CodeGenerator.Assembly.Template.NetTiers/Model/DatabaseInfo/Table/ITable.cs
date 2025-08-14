using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table
{
    public interface ITable : ISchemaObject
    {
        IEnumerable<IColumnTable> Columns { get; }
        IEnumerable<IStoredProcedure> StoredProcedures { get; }
    }


}
