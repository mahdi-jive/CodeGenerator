using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class TableEntityGeneratedTemp : ICodeTemplate<EntitiesFactory, DatabaseInfoModel>
    {
        public async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, IContextModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var compilationUnit = await renderer.RenderAsync("TableEntityGeneratedTemp.cshtml", item);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}Base.generated.cs", item.NamePascal, compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
