using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.Infrastructure;
using MediatR;

namespace CodeGenerator.Abstractions
{
    public interface IAssemblyGenerator : IDirectoryGenerator, IGeneratorBase
    {
        public IAssembly Assembly { get; }
        public IMediator Mediator { get; }
        public ITemplateRenderer Renderer { get; }
        public IContextModel ContextModel { get; }
        public abstract Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles);
        public abstract Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles, IDirectory directory);
        public Task<IAssemblyGenerator> GenerateAssemblyAsync(IMediator mediator, ITemplateRenderer renderer, IDirectory directory, IContextModel contextModel = null);
    }
}
