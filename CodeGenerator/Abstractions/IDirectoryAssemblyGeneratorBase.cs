using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface IDirectoryAssemblyGeneratorBase : IGeneratorBase
    {
        public Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator);
        public Task<IDirectoryAssemblyGeneratorBase> CreateDirectoryAssemblyGeneratorAsync(IAssemblyGenerator assemblyGenerator, IDirectory directoryParent);
        public string DirectoryName { get; }
    }
}
