using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.StaticFile;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntityLocatorTemp : ICodeTemplate<EntitiesFactory, DatabaseInfoModel>
    {
        private async Task<CompilationUnitSyntax> GetCompilationUnit(DatabaseInfoModel model, string className)
        {
            var tree = CSharpSyntaxTree.ParseText(StaticFileResource.EntityLocator);
            var compilationUnitSyntax = (CompilationUnitSyntax)await tree.GetRootAsync();
            return compilationUnitSyntax;
        }
        public async Task<IEnumerable<ICodeFile>> Generate(IContextModel contextModel)
        {
            string className = "EntityLocator";
            var model = contextModel as DatabaseInfoModel;
            List<ICodeFile> codeFiles = new List<ICodeFile>() { new CodeFile($"{className}.cs", className, await GetCompilationUnit(model, className)) };
            return codeFiles;
        }
    }
}
