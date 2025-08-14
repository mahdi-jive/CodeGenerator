using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public interface IView : ISchemaObject
    {
        IEnumerable<IColumnView> Columns { get; }
        IEnumerable<IStoredProcedure> StoredProcedures { get; }
    }


}
