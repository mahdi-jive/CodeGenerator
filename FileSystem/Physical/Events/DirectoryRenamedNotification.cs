using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    public class DirectoryRenamedNotification : INotification
    {
        public DirectoryRenamedNotification(IDirectory directory, string oldName, string newName)
        {
            Directory = directory;
            OldName = oldName;
            NewName = newName;
        }

        public IDirectory Directory { get; private set; }
        public string OldName { get; private set; }
        public string NewName { get; private set; }

    }
}
