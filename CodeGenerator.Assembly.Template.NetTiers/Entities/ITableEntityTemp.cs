using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class ITableEntityTemp : CodeGeneratorBase<EntitiesFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var tempModel = await ITableEntityTempModel.CreateModel(item);
                    var compilationUnit = await renderer.RenderAsync("ITableEntityTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"I{item.NamePascal}.cs", $"I{item.NamePascal}", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
