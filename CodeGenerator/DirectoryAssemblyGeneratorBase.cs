using CodeGenerator.Abstractions;
using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator
{
    public abstract class DirectoryAssemblyGeneratorBase<TAssemblyGenerator, TDirectoryParent>
        : IDirectoryAssemblyGenerator<TAssemblyGenerator, TDirectoryParent>, IDirectoryGenerator
          where TAssemblyGenerator : IAssemblyGenerator
        where TDirectoryParent : IDirectoryGenerator
    {
        public TAssemblyGenerator AssemblyGenerator { get; private set; }
        public IDirectory Directory { get; private set; }

        public abstract string DirectoryName { get; }

        public async Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator)
        {
            await CreateDirectoryAssemblyGeneratorAsync((TAssemblyGenerator)assemblyGenerator);
            return this;
        }

        public async Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator, IDirectoryGenerator directoryParent)
        {
            await CreateDirectoryAssemblyGeneratorAsync((TAssemblyGenerator)assemblyGenerator, (TDirectoryParent)directoryParent);
            return this;
        }

        public async Task Generate()
        {
            var excludeType = GetType();
            var targetGenericDefinition = typeof(ICodeGenerator<,>);

            var codeGenerator = TypeFinder.FindWithGenericTypes(this.GetType().Assembly, targetGenericDefinition, GetType(), AssemblyGenerator.ContextModel.GetType())
                .Select(t => (ICodeGeneratorBase)Activator.CreateInstance(t));

            foreach (var generator in codeGenerator)
            {
                var codeFiles = await generator.Generate(AssemblyGenerator.Renderer, AssemblyGenerator.ContextModel);
                await AssemblyGenerator.SaveFileAsync(codeFiles, Directory);
            }

            var targetGenericDirectoryDefinition = typeof(IDirectoryAssemblyGenerator<,>);

            var directoryAssembly = TypeFinder.FindWithGenericTypes(this.GetType().Assembly, targetGenericDirectoryDefinition, this.AssemblyGenerator.GetType(), GetType())
                .Select(t => (IDirectoryAssemblyGeneratorBase)Activator.CreateInstance(t));

            foreach (var generator in directoryAssembly)
            {
                await generator.CreateDirectoryAssemblyGeneratorAsync(this.AssemblyGenerator, this);
            }
        }


        protected virtual async Task<IDirectoryAssemblyGenerator<TAssemblyGenerator, TDirectoryParent>> CreateDirectoryAssemblyGeneratorAsync(TAssemblyGenerator assemblyGenerator)
        {
            AssemblyGenerator = assemblyGenerator;
            await CreateDirectoryAsync(assemblyGenerator);
            await Generate();
            return this;
        }
        protected virtual async Task<IDirectoryAssemblyGenerator<TAssemblyGenerator, TDirectoryParent>> CreateDirectoryAssemblyGeneratorAsync(TAssemblyGenerator assemblyGenerator, TDirectoryParent directoryParent)
        {
            AssemblyGenerator = assemblyGenerator;
            await CreateDirectoryAsync(directoryParent);
            await Generate();
            return this;
        }
        protected virtual async Task CreateDirectoryAsync(TDirectoryParent directoryParent)
        {
            var directory = await directoryParent.Directory.AddDirectoryAsync(DirectoryName);
            Directory = directory;
        }
        protected virtual async Task CreateDirectoryAsync(TAssemblyGenerator directoryParent)
        {
            var directory = await directoryParent.Directory.AddDirectoryAsync(DirectoryName);
            Directory = directory;
        }

    }
}
