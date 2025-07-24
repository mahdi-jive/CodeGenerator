using CodeGenerator.Assembly.Abstractions;

namespace CodeGenerator.Abstractions
{
    public interface ICodeTemplate<TGenerator> where TGenerator : IGenerator
    {
        IEnumerable<ICodeFile> Generate();
    }
}
