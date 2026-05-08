using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Template.NetTiers
{
    public static class GenerateNetTiersFactory
    {
        public static async Task Generated(IMediator mediator, IDirectory directory, IUserConfiguration userConfiguration)
        {
            var currentAssembly = typeof(GenerateNetTiersFactory).Assembly;
            var contextModel = DataLayerGeneratorFactory.Generate(userConfiguration);
            var codeGeneration = new CodeGenerationManager(currentAssembly, mediator, directory, contextModel, Path.Combine(Directory.GetCurrentDirectory(), "Templates"));
            await codeGeneration.GenerateAllAsync();

        }
    }
}
