using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Infrastructure;
using Humanizer;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class TableEntityTemp :  CodeGeneratorBase<EntitiesFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var compilationUnit = await renderer.RenderAsync("TableEntityTemp.cshtml", item);
                    codeFiles.Add(new CodeFile($"{item.Name.Pascalize()}.cs", item.Name.Pascalize(), compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
