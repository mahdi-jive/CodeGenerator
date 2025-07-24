using CodeGenerator.Assembly.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Events
{
    public class AssemblyDeletedNotification : INotification
    {
        public IAssembly Assembly { get; private set; }

        public AssemblyDeletedNotification(IAssembly assembly)
        {
            Assembly = assembly;
        }
    }
}
