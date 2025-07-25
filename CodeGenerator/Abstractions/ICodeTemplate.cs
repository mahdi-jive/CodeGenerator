using CodeGenerator.Assembly.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface ICodeTemplate<TDirectoryParent, TContextModel> : ICodeTemplateBase where TDirectoryParent : IDirectoryMarkerParent where TContextModel : IContextModel
    {
    }
    public interface ICodeTemplateBase
    {
        IEnumerable<ICodeFile> Generate(IContextModel contextModel);
    }
}
