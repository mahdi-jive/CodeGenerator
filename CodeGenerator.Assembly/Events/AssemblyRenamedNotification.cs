using CodeGenerator.Assembly.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Events
{
    // File Events
    public class AssemblyRenamedNotification : INotification
    {
        public AssemblyRenamedNotification(IAssembly assembly, string oldName, string newName)
        {
            Assembly = assembly;
            OldName = oldName;
            NewName = newName;
        }

        public IAssembly Assembly { get; private set; }
        public string OldName { get; private set; }
        public string NewName { get; private set; }

    }
}
