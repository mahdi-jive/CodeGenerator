using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Assembly.Abstractions
{
    public interface ICodeFile
    {
        CompilationUnitSyntax CompilationUnit { get; }
        string ClassName { get; }
        string FileName { get; }
    }
}
