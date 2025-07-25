using CodeGenerator.Assembly.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface IGenerator
    {
        IAssembly Assembly { get; }
        public Task Generate();
    }
}
