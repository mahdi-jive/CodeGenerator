namespace CodeGenerator.FileSystem.Abstractions
{
    public interface IDirectory : IFullPath, IRelativePath
    {
        string Name { get; }
        IReadOnlyList<IDirectory> Directories { get; }
        IReadOnlyList<IFile> Files { get; }

        Task<IDirectory> AddDirectoryAsync(string name);
        Task<IFile> AddFileAsync(string name, string content);
        Task<IFile> AddFileAsync(string name, byte[] content);
        Task<IFile> AddFileAsync(string name, Stream content);

        Task RenameAsync(string newName);
        Task DeleteAsync();

    }
}
