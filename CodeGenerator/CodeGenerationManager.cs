using CodeGenerator.Abstractions;
using CodeGenerator.Infrastructure;

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
            ITemplateRenderer Renderer = new TemplateRenderer(Path.Combine(Directory.GetCurrentDirectory(), "Templates"));
            var tasks = Generators
                .Select(generator =>
                Task.Run(async () =>
                {
                    await generator.Generate(Renderer);
                })
                );
            await Task.WhenAll(tasks);
        }
    }
}
