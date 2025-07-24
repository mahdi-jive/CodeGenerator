using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class FileContentUpdatedNotification : INotification
    {
        public IFile File { get; private set; }

        public FileContentUpdatedNotification(IFile file)
        {
            File = file;
        }
    }
}
