using CodeGenerator.Assembly.Abstractions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Assembly
{
    public class CodeFile : ICodeFile
    {
        public CompilationUnitSyntax CompilationUnit { get; private set; }
        public string ClassName { get; private set; }

        public CodeFile(string className, CompilationUnitSyntax compilationUnit)
        {
            ClassName = className;
            CompilationUnit = compilationUnit;
        }
    }
}
