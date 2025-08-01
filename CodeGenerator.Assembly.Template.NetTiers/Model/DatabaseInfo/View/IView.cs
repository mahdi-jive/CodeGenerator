using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public interface IView : ISchemaObject
    {
        IReadOnlyCollection<IColumnView> Columns { get; }
        IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    }


}
