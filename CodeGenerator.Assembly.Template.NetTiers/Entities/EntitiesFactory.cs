using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48;
using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.FileSystem.Abstractions;
using MediatR;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntitiesFactory : DirectoryMarkerBase<EntitiesFactory, EntitiesContextModel>, IGenerator
    {
        private static readonly string LayerNamePosfix = "Entities";
        private EntitiesFactory(IAssembly assembly)
        {
            Assembly = assembly;
        }
        public EntitiesFactory() { }
        public IAssembly Assembly { get; private set; }

        public static async Task<EntitiesFactory> GenerateAssembly(IMediator mediator, IDirectory directory)
        {
            IAssembly assembly = await AssemblyNetFx48.CreateAsync(mediator, directory, $"{directory.Name}.{LayerNamePosfix}");
            var entitiesFactory = new EntitiesFactory(assembly);
            return entitiesFactory;
        }

        public async Task Generate()
        {
            await Generate(Assembly.Directory, Assembly, new EntitiesContext().GetContextModel());
        }

        //public async Task Generate()
        //{

        //    await Generate(Assembly.Directory);
        //}


        public override async Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile)
        {
            await directory.AddSourceFileAsync(assembly, codeFile);
        }
    }
    public class EntitiesContextModel : IContextModel
    {
        public string Test { get => "testData"; }
    }
    public class EntitiesContext : ITemplateContext<EntitiesContextModel>
    {
        public EntitiesContextModel GetContextModel()
        {
            return new EntitiesContextModel();
        }
    }
}
