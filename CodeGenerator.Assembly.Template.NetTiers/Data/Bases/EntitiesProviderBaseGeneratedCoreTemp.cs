using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.Enums;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntitiesProviderBaseGeneratedCoreTemp : CodeGeneratorBase<BasesDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var compilationUnit = await renderer.RenderAsync("Data/Bases/EntitiesProviderBaseGeneratedCoreTemp.cshtml", item);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}ProviderBase.generatedCore.cs", $"{item.NamePascal}ProviderBaseCore", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
