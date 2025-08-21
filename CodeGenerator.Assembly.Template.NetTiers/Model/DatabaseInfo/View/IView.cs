using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public interface IView : ISchemaObject
    {
        Task<IEnumerable<IColumnView>> Columns { get; }
        Task<IEnumerable<IStoredProcedure>> StoredProcedures { get; }
    }


}
