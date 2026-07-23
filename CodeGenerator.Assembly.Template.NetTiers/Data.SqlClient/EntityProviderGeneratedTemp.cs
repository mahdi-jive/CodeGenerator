using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.SqlClient;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Data.SqlClient
{
    internal class EntityProviderGeneratedTemp : CodeGeneratorBase<SqlClientFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel;
            if (model != null)
            {
                foreach (var item in await model.Tables)
                {
                    try
                    {
                        var tempModel = await EntityProviderGeneratedTempModel.CreateModel(item);
                        var compilationUnit = await renderer.RenderAsync("SqlClient/EntitiesProviderGeneratedTemp.cshtml", tempModel);
                        codeFiles.Add(new CodeFile($"Sql{item.NamePascal}ProviderBase.generated.cs", $"Sql{item.NamePascal}ProviderBase", compilationUnit));
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }

            return codeFiles;
        }
    }
}
