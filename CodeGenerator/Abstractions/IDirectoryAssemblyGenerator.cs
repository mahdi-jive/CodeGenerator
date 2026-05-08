using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface IDirectoryAssemblyGenerator<TAssemblyGenerator, TDirectoryParent> : IGeneratorBase, IDirectoryAssemblyGeneratorBase
        where TAssemblyGenerator : IAssemblyGenerator, TDirectoryParent
        where TDirectoryParent : IDirectoryGenerator
    {
        public TAssemblyGenerator AssemblyGenerator { get; }
        public IDirectory Directory { get; }
        public string DirectoryName { get; }
    }
}
