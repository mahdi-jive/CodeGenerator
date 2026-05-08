using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Abstractions
{
    public interface ICodeGeneratorBase
    {
        public Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, IContextModel contextModel);
    }
}
