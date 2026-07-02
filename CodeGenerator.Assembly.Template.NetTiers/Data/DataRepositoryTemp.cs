using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Data;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Data;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class DataRepositoryTemp : CodeGeneratorBase<DataFactory, DatabaseInfoModel>
    {
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            string className = "DataRepository";
            List<CodeFile> codeFiles = new List<CodeFile>();
            var model = contextModel as DatabaseInfoModel;
            if (model != null)
            {
                List<ISchemaObject> schemaObjects = new List<ISchemaObject>();

                var tables = await model.Tables;
                var views = await model.Views;
                schemaObjects.AddRange(tables);
                schemaObjects.AddRange(views);
                var dataRepositoryModel = new DataRepositoryTempModel(schemaObjects);
                var compilationUnit = await renderer.RenderAsync("Data/DataRepositoryTemp.cshtml", dataRepositoryModel);
                codeFiles.Add(new CodeFile($"{className}.cs", className, compilationUnit));

            }

            return codeFiles;
        }
    }
}
