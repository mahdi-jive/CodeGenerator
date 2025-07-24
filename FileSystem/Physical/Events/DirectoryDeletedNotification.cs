using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class DirectoryDeletedNotification : INotification
    {
        public IDirectory Directory { get; private set; }

        public DirectoryDeletedNotification(IDirectory directory)
        {
            Directory = directory;
        }
    }
}
