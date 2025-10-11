using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Abstractions
{
    public interface ICodeTemplate<TDirectoryParent, TContextModel> : ICodeTemplateBase where TDirectoryParent : IDirectoryMarkerParent where TContextModel : IContextModel
    {
    }
    public interface ICodeTemplateBase
    {
        Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer Renderer, IContextModel contextModel);
    }
}
