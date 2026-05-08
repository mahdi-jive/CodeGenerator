using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Data;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;
using CodeGenerator.Assembly.Template.NetTiers.ModelView.Data;
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
                var tables = new Lazy<Task<IEnumerable<ITable>>>(() => model.Tables);
                var views = new Lazy<Task<IEnumerable<IView>>>(() => model.Views);
                DataRepositoryModel dataRepositoryModel = new DataRepositoryModel(tables, views);
                var compilationUnit = await renderer.RenderAsync("Data/DataRepositoryTemp.cshtml", dataRepositoryModel);
                codeFiles.Add(new CodeFile($"{className}.cs", className, compilationUnit));

            }

            return codeFiles;
        }
    }
}
