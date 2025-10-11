using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Abstractions
{
    public interface IGenerator
    {
        IAssembly Assembly { get; }
        public Task Generate(ITemplateRenderer Renderer);
    }
}
