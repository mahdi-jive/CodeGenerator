using CodeGenerator.Abstractions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.Infrastructure;
using MediatR;

namespace CodeGenerator
{
    public class CodeGenerationManager : ICodeGenerationManager
    {
        public CodeGenerationManager(System.Reflection.Assembly assembly, IMediator mediator, IDirectory directory, IContextModel contextModel, string templatesRoot)
        {
            Assembly = assembly;
            Mediator = mediator;
            Directory = directory;
            ContextModel = contextModel;
            TemplatesRoot = templatesRoot;
        }

        public System.Reflection.Assembly Assembly { get; private set; }
        public IMediator Mediator { get; private set; }
        public IContextModel ContextModel { get; private set; }
        public string TemplatesRoot { get; private set; }
        public IDirectory Directory { get; private set; }

        public async Task GenerateAllAsync()
        {
            //Path.Combine(Assembly.GetCurrentDirectory(), "Templates")
            ITemplateRenderer Renderer = new TemplateRenderer(TemplatesRoot);
            var AssemblyGeneratorsTypes = TypeFinder.FindWithTypes(Assembly, typeof(IAssemblyGenerator));
            var AssemblyGenerators = AssemblyGeneratorsTypes
                .Select(type => (IAssemblyGenerator)Activator.CreateInstance(type))
                .ToList();
            //foreach (var assemblyGenerator in AssemblyGenerators)
            //{
            //    await assemblyGenerator.GenerateAssemblyAsync(Mediator, Renderer, Directory, null);
            //}
            var tasks = AssemblyGenerators
                .Select(generator =>
                Task.Run(async () =>
                {
                    // await generator.Generate(Renderer);
                    await generator.GenerateAssemblyAsync(Mediator, Renderer, Directory, null);
                })
                );
            await Task.WhenAll(tasks);


        }
    }
}
