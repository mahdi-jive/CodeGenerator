namespace CodeGenerator.FileSystem.Abstractions
{
    public interface IFile : IFullPath, IRelativePath
    {
        string Name { get; }
        string Extension { get; }
        Task RenameAsync(string newName);
        Task UpdateContentAsync(string content);
        Task UpdateContentAsync(Stream content);
        Task UpdateContentAsync(byte[] content);
        Task DeleteAsync();
        Task<Stream> OpenReadAsync();

    }


}
