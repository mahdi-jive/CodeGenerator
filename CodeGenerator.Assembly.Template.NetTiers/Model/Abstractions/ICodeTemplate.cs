using CodeGenerator.Assembly.Abstractions;
namespace CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions
{
    public interface ICodeTemplate : ICodeFile
    {
        IDataModel DataModel { get; }
    }
}
