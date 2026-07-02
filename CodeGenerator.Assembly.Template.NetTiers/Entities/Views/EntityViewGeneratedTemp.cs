using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.Enums;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Views;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities.Views
{
    internal class EntityViewGeneratedTemp : CodeGeneratorBase<ViewsDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Views)
                {
                    var tempModel = await EntityViewGeneratedTempModel.CreateModel(item);
                    var compilationUnit = await renderer.RenderAsync("Views/EntityViewGeneratedTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}Base.generated.cs", $"{item.NamePascal}Base", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
