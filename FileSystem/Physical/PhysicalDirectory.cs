using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.FileSystem.Physical.Events;
using MediatR;

namespace CodeGenerator.FileSystem.Physical
{
    public class PhysicalDirectory : IDirectory
    {
        public string Name { get; private set; }
        public IMediator Mediator { get; private set; }
        public string FullPath { get; private set; }
        public string RelativePath { get; private set; }

        private readonly List<PhysicalDirectory> _directories = new();
        private readonly List<PhysicalFile> _files = new();

        public IReadOnlyList<IDirectory> Directories => _directories.AsReadOnly();
        public IReadOnlyList<IFile> Files => _files.AsReadOnly();

        protected PhysicalDirectory(IMediator mediator, string fullPath, string relativePath)
        {
            Mediator = mediator;
            FullPath = fullPath;
            RelativePath = relativePath;
            Name = Path.GetFileName(fullPath);

        }

        // فقط یک ریشه ایجاد می‌کنیم، بعد زیر مجموعه‌ها از این ریشه ساخته می‌شوند
        public static async Task<PhysicalDirectory> CreateRootAsync(IMediator mediator, string basePath, string rootDirectoryName)
        {
            string rootFullPath = Path.Combine(basePath, rootDirectoryName);
            Directory.CreateDirectory(rootFullPath);
            var physicalDirectory = new PhysicalDirectory(mediator, rootFullPath, rootDirectoryName);
            await mediator.Publish(new DirectoryRootAddedNotification(physicalDirectory));
            return physicalDirectory;
        }
        public async Task<IDirectory> AddDirectoryAsync(string name)
        {
            string newFullPath = Path.Combine(FullPath, name);
            Directory.CreateDirectory(newFullPath);

            string newRelativePath = Path.Combine(RelativePath, name);
            var physicalDirectory = new PhysicalDirectory(Mediator, newFullPath, newRelativePath);

            _directories.Add(physicalDirectory);
            await Mediator.Publish(new DirectoryAddedNotification(physicalDirectory));
            return physicalDirectory;
        }

        public async Task<IFile> AddFileAsync(string fileName, string content)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(content);
            return await AddFileAsync(fileName, bytes);
        }

        public async Task<IFile> AddFileAsync(string fileName, byte[] content)
        {
            string fullPath = Path.Combine(FullPath, fileName);
            await File.WriteAllBytesAsync(fullPath, content);
            PhysicalFile physicalFile = new(Mediator, FullPath, fileName);
            await Mediator.Publish(new FileAddedNotification(physicalFile));
            return physicalFile;
        }

        public async Task<IFile> AddFileAsync(string fileName, Stream content)
        {
            if (content.CanSeek)
                content.Position = 0;

            string filePath = Path.Combine(FullPath, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await content.CopyToAsync(fileStream);
            }
            PhysicalFile physicalFile = new(Mediator, FullPath, fileName);
            await Mediator.Publish(new FileAddedNotification(physicalFile));
            return physicalFile;
        }

        public async Task RenameAsync(string newName)
        {
            string oldName = Name;
            string parentDir = Path.GetDirectoryName(FullPath) ?? throw new InvalidOperationException("Parent directory not found.");
            string newFullPath = Path.Combine(parentDir, newName);

            Directory.Move(FullPath, newFullPath);
            FullPath = newFullPath;
            Name = newName;
            RelativePath = Path.Combine(Path.GetDirectoryName(RelativePath) ?? "", newName);
            await Mediator.Publish(new DirectoryRenamedNotification(this, oldName, newName));
        }

        public async Task DeleteAsync()
        {
            if (Directory.Exists(FullPath))
                Directory.Delete(FullPath, true);

            await Mediator.Publish(new DirectoryDeletedNotification(this));
        }

    }
}
