using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.NetFx48.Extensions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.Query;
using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class Q2Folder : DirectoryMarkerBase<QueryFolder, EntitiesContextModel>
    {
        public override async Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile)
        {
            await directory.AddSourceFileAsync(assembly, codeFile);
        }
    }
}
