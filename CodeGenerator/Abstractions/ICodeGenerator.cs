using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Abstractions
{
    public interface ICodeGenerator<TDirectoryGenerator, TContextModel> : ICodeGeneratorBase
        where TDirectoryGenerator : IDirectoryGenerator
        where TContextModel : IContextModel
    {
        public Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, TContextModel contextModel);
    }
}
