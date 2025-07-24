using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class FileDeletedNotification : INotification
    {
        public IFile File { get; private set; }

        public FileDeletedNotification(IFile file)
        {
            File = file;
        }
    }
}
