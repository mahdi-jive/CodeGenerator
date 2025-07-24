using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.FileSystem.Physical.Events
{
    // File Events
    public class FileRenamedNotification : INotification
    {
        public FileRenamedNotification(IFile file, string oldName, string newName)
        {
            File = file;
            OldName = oldName;
            NewName = newName;
        }

        public IFile File { get; private set; }
        public string OldName { get; private set; }
        public string NewName { get; private set; }

    }
}
