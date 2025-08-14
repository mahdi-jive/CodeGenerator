using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public interface IDatabaseInfoModel : IContextModel
    {
        string CustomProcedureStartsWith { get; }
        string CompanyName { get; }
        string CompanyURL { get; }
        string RootNameSpace { get; }
        IEnumerable<ITable> Tables { get; }
        IEnumerable<IView> Views { get; }
        IEnumerable<ITableEnum> TableEnums { get; }
    }
}
