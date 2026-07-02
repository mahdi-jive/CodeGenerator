using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.Enums;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Views;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities.Views
{
    internal class EntityViewTemp : CodeGeneratorBase<ViewsDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Views)
                {

                    var tempModel = new EntityViewTempModel(item);
                    var compilationUnit = await renderer.RenderAsync("Views/EntityViewTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}.cs", $"{item.NamePascal}", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
