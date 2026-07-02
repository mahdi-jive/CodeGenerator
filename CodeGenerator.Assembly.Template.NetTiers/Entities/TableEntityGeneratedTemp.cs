using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class TableEntityGeneratedTemp : CodeGeneratorBase<EntitiesFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var tempModel = await TableEntityGeneratedTempModel.CreateModel(item);
                    var compilationUnit = await renderer.RenderAsync("TableEntityGeneratedTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"{item.NamePascal}Base.generated.cs", item.NamePascal, compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
