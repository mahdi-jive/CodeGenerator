using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Data.SqlClient
{
    internal class EntitiesProviderTemp : CodeGeneratorBase<SqlClientFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    var tempModel = new EntitiesProviderTempModel(item);
                    var compilationUnit = await renderer.RenderAsync("SqlClient/EntitiesProviderTemp.cshtml", tempModel);
                    codeFiles.Add(new CodeFile($"Sql{item.NamePascal}Provider.cs", $"Sql{item.NamePascal}Provider", compilationUnit));
                }

            }

            return codeFiles;
        }
    }
}
