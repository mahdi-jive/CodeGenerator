using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class FileAddedNotification : INotification
    {
        public IFile File { get; private set; }

        public FileAddedNotification(IFile file)
        {
            File = file;
        }
    }
}
