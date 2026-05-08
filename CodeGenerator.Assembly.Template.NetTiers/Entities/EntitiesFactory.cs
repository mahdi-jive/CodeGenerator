using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48;
using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntitiesFactory : AssemblyGeneratorBase
    {
        private static readonly string LayerNamePosfix = "Entities";
        public override async Task<IAssembly> CreateAssemblyAsync(IMediator mediator, IDirectory directory)
        {
            IAssembly assembly = await AssemblyNetFx48.CreateAsync(mediator, directory, $"{directory.Name}.{LayerNamePosfix}");
            return assembly;
        }

        public override async Task<IContextModel> CreateContextModelAsync()
        {
            return await Task.FromResult(DataLayerGeneratorFactory.Generate(null));
        }

        public override async Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles)
        {

            await SaveFileAsync(codeFiles, Directory);
        }

        public override async Task SaveFileAsync(IEnumerable<ICodeFile> codeFiles, IDirectory directory)
        {
            foreach (var item in codeFiles)
            {
                await directory.AddSourceFileAsync(Assembly, item);
            }

        }
    }
}
