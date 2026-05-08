using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.Infrastructure;
using MediatR;

namespace CodeGenerator
{
    public abstract class AssemblyGeneratorBase : IAssemblyGenerator
    {

        public IAssembly Assembly { get; private set; }

        public IMediator Mediator { get; private set; }

        public ITemplateRenderer Renderer { get; private set; }

        public IContextModel ContextModel { get; private set; }

        public IDirectory Directory { get; private set; }

        public virtual async Task<IAssemblyGenerator> GenerateAssemblyAsync(IMediator mediator, ITemplateRenderer renderer, IDirectory directory, IContextModel contextModel = null)
        {
            ArgumentNullException.ThrowIfNull(directory);
            Mediator = mediator;
            Renderer = renderer;
            Assembly = await CreateAssemblyAsync(mediator, directory);

            ContextModel = contextModel ?? await CreateContextModelAsync();


            Directory = Assembly.Directory;
            await Generate();
            return this;
        }
        public abstract Task<IAssembly> CreateAssemblyAsync(IMediator mediator, IDirectory directory);
        public abstract Task<IContextModel> CreateContextModelAsync();
        public async Task Generate()
        {
            var excludeType = GetType();
            var targetGenericDefinition = typeof(IDirectoryAssemblyGenerator<,>);

            var directoryAssembly = TypeFinder.FindWithGenericTypes(this.GetType().Assembly, targetGenericDefinition, GetType(), GetType())
                .Select(t => (IDirectoryAssemblyGeneratorBase)Activator.CreateInstance(t));

            foreach (var generator in directoryAssembly)
            {
                await generator.CreateDirectoryAssemblyGeneratorAsync(this);
            }
            var excludeTypeCode = GetType();
            var targetGenericDefinitionCode = typeof(ICodeGenerator<,>);

            var codeAssembly = TypeFinder.FindWithGenericTypes(this.GetType().Assembly, targetGenericDefinitionCode, GetType(), ContextModel.GetType())
                .Select(t => (ICodeGeneratorBase)Activator.CreateInstance(t));

            foreach (var generator in codeAssembly)
            {
                var codeFiles = await generator.Generate(Renderer, ContextModel);
                await SaveFileAsync(codeFiles);
            }
        }

        public abstract Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles);

        public abstract Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles, IDirectory directory);
    }
}
