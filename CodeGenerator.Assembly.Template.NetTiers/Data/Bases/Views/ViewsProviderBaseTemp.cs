using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data.Bases.Views;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Data.Bases.Views
{
    public class ViewsProviderBaseTemp : CodeGeneratorBase<ViewsDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel;
            if (model != null)
            {
                foreach (var item in await model.Views)
                {
                    var tempModel = new ViewsProviderBaseTempModel(item);
                    var compilationUnit = await renderer.RenderAsync("Data/Bases/Views/ViewsProviderBaseTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}ProviderBase.cs", $"{item.NamePascal}ProviderBase", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
