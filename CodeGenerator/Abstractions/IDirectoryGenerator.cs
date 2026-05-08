using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface IDirectoryGenerator
    {
        IDirectory Directory { get; }
    }
}
