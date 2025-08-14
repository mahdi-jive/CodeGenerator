using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Entities.StaticFile;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class EntityFactoryTemp : ICodeTemplate<EntitiesFactory, DatabaseInfoModel>
    {
        private CompilationUnitSyntax GetCompilationUnit(DatabaseInfoModel model, string className)
        {
            var tree = CSharpSyntaxTree.ParseText(StaticFileResource.EntityFactory);
            var compilationUnitSyntax = (CompilationUnitSyntax)tree.GetRoot();
            return compilationUnitSyntax;
        }
        public IEnumerable<ICodeFile> Generate(IContextModel contextModel)
        {
            string className = "EntityFactory";
            var model = contextModel as DatabaseInfoModel;
            List<ICodeFile> codeFiles = new List<ICodeFile>() { new CodeFile($"{className}.cs", className, GetCompilationUnit(model, className)) };
            return codeFiles;
        }
    }
}
