using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Data.Bases;
using CodeGenerator.Assembly.Template.NetTiers.Entities.StaticFile;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Infrastructure;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntityViewProviderBaseCoreGeneratedTemp : CodeGeneratorBase<BasesDirectory, DatabaseInfoModel>
    {
        private async Task<CompilationUnitSyntax> GetCompilationUnit(DatabaseInfoModel model, string className)
        {
            var tree = CSharpSyntaxTree.ParseText(StaticFileResource.EntityViewProviderBaseCore_generated);
            var compilationUnitSyntax = (CompilationUnitSyntax)await tree.GetRootAsync();
            return compilationUnitSyntax;
        }
        public override async Task<IEnumerable<ICodeFile>> Generate(ITemplateRenderer renderer, DatabaseInfoModel contextModel)
        {
            string className = "EntityViewProviderBaseCore";
            var model = contextModel as DatabaseInfoModel;
            List<ICodeFile> codeFiles = new List<ICodeFile>() { new CodeFile($"{className}.generated.cs", className, await GetCompilationUnit(model, className)) };
            return codeFiles;
        }
    }
}
