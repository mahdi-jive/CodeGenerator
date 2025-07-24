using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Column;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions
{
    public interface IView : ISchemaObject
    {
        IColumnCollection Columns { get; }
        IStoredProcedureCollection StoredProcedures { get; }
    }


}
