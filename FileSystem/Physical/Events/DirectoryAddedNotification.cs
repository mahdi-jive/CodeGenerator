using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class DirectoryAddedNotification : INotification
    {
        public IDirectory Directory { get; private set; }

        public DirectoryAddedNotification(IDirectory directory)
        {
            Directory = directory;
        }
    }
}
