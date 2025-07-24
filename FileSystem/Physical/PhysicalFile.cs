using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.FileSystem.Physical.Events;
using MediatR;

namespace CodeGenerator.FileSystem.Physical
{
    public class PhysicalFile : IFile
    {
        public string Name { get; private set; }
        public string Extension => Path.GetExtension(Name);

        public IMediator Mediator { get; private set; }
        public string FullPath { get; private set; }
        public string RelativePath { get; private set; }

        internal PhysicalFile(IMediator mediator, string fullPath, string relativePath)
        {
            Mediator = mediator;
            FullPath = fullPath;
            RelativePath = relativePath;
            Name = Path.GetFileName(fullPath);
        }


        public async Task RenameAsync(string newName)
        {
            string oldName = Name;
            string newPath = Path.Combine(Path.GetDirectoryName(FullPath)!, newName);
            File.Move(FullPath, newPath);
            FullPath = newPath;
            Name = newName;
            RelativePath = Path.Combine(Path.GetDirectoryName(RelativePath) ?? "", newName);
            await Mediator.Publish(new FileRenamedNotification(this, oldName, newName));
        }

        public async Task DeleteAsync()
        {
            if (File.Exists(FullPath))
                File.Delete(FullPath);
            await Mediator.Publish(new FileDeletedNotification(this));
        }
        public async Task<Stream> OpenReadAsync()
        {
            using (var fs = new FileStream(FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                return await Task.FromResult(fs);
            }
        }

        public async Task UpdateContentAsync(string content)
        {
            using (var writer = new StreamWriter(FullPath, false))
            {
                await writer.WriteAsync(content);
            }
            await Mediator.Publish(new FileContentUpdatedNotification(this));
        }

        public async Task UpdateContentAsync(Stream content)
        {
            if (content.CanSeek)
                content.Seek(0, SeekOrigin.Begin);

            using (var fileStream = new FileStream(FullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await content.CopyToAsync(fileStream);
            }
            await Mediator.Publish(new FileContentUpdatedNotification(this));
        }

        public async Task UpdateContentAsync(byte[] content)
        {
            await File.WriteAllBytesAsync(FullPath, content);
            await Mediator.Publish(new FileContentUpdatedNotification(this));
        }


    }
}
