using CodeGenerator.Assembly.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Events
{
    public class AssemblyContentUpdatedNotification : INotification
    {
        public IAssembly Assembly { get; private set; }

        public AssemblyContentUpdatedNotification(IAssembly assembly)
        {
            Assembly = assembly;
        }
    }
}
