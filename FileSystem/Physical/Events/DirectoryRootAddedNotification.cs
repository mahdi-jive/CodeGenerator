using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class DirectoryRootAddedNotification : INotification
    {
        public IDirectory Directory { get; private set; }

        public DirectoryRootAddedNotification(IDirectory directory)
        {
            Directory = directory;
        }
    }
}
