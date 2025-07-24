using CodeGenerator.Assembly.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Events
{
    public class AssemblyAddedNotification : INotification
    {
        public IAssembly Assembly { get; private set; }

        public AssemblyAddedNotification(IAssembly assembly)
        {
            Assembly = assembly;
        }
    }
}
