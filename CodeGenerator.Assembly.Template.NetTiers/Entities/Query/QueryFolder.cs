using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities.Query
{
    public class QueryFolder : DirectoryMarkerBase<EntitiesFactory, EntitiesContextModel>
    {
        public override async Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile)
        {
            await directory.AddSourceFileAsync(assembly, codeFile);
        }
    }
}
