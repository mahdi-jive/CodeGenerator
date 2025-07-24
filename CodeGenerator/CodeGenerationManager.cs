using CodeGenerator.Abstractions;

namespace CodeGenerator
{
    public class CodeGenerationManager : ICodeGenerationManager
    {
        public IReadOnlyCollection<IGenerator> Generators { get; private set; }

        public CodeGenerationManager(IReadOnlyCollection<IGenerator> generators)
        {
            Generators = generators;
        }

        public async Task GenerateAllAsync()
        {
            var tasks = Generators
                .Select(generator =>
                Task.Run(() =>
                {
                    generator.Generate();
                    generator.SaveFile();
                })
                );
            await Task.WhenAll(tasks);
        }
    }
}
