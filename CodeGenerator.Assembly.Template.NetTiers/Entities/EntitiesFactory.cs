using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48;
using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntitiesFactory : GeneratorBase<EntitiesFactory>
    {
        private static readonly string LayerNamePosfix = "Entities";
        private EntitiesFactory(IAssembly assembly)
            : base(assembly)
        {
        }

        public static async Task<EntitiesFactory> GenerateAssembly(IMediator mediator, IDirectory directory, string assemblyName)
        {
            IAssembly assembly = await AssemblyNetFx48.CreateAsync(mediator, directory, $"{assemblyName}.{LayerNamePosfix}");
            var entitiesFactory = new EntitiesFactory(assembly);
            return entitiesFactory;
        }

        public override async Task SaveFile()
        {
            foreach (var codeFile in Generate())
            {
                await Assembly.Directory.AddSourceFileAsync(Assembly, codeFile);
            }
        }
    }
}
