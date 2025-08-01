using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseInfoModel : IContextModel
    {
        IReadOnlyCollection<ITable> Tables { get; }
        IReadOnlyCollection<IView> Views { get; }
        IReadOnlyCollection<ITableEnum> TableEnums { get; }
        IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; }
    }
}
