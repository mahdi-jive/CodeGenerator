using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities.Enums
{
    internal class EnumEntityListTemp : CodeGeneratorBase<EnumsDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            if (contextModel != null)
            {
                foreach (var item in await contextModel.TableEnums)
                {
                    var compilationUnit = await renderer.RenderAsync("Enums/EnumEntityListTemp.cshtml", item);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}List.cs", $"{item.NamePascal}List", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
