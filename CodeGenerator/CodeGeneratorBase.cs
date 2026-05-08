using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator
{
    public abstract class CodeGeneratorBase<TDirectoryGenerator, TContextModel> : ICodeGenerator<TDirectoryGenerator, TContextModel>
        where TDirectoryGenerator : IDirectoryGenerator
        where TContextModel : IContextModel
    {
        public abstract Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, TContextModel contextModel);

        public async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, IContextModel contextModel)
        {
            var codeFiles = await Generate(renderer, (TContextModel)contextModel);
            return codeFiles;
        }
    }
}
