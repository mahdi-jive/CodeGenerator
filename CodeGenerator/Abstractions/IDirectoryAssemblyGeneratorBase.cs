namespace CodeGenerator.Abstractions
{
    public interface IDirectoryAssemblyGeneratorBase : IGeneratorBase
    {
        public Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator);
        public Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator, IDirectoryGenerator directoryParent);
        public string DirectoryName { get; }
    }
}
