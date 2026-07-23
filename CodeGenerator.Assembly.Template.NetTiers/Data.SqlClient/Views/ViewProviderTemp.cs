using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient.Views;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Data.SqlClient.Views
{
    internal class ViewProviderTemp : CodeGeneratorBase<ViewsDirectory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel;
            if (model != null)
            {
                foreach (var item in await model.Views)
                {
                    var tempModel = new ViewProviderTempModel(item);
                    var compilationUnit = await renderer.RenderAsync("SqlClient/Views/ViewProviderTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"Sql{item.NamePascal}Provider.cs", $"Sql{item.NamePascal}Provider", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
