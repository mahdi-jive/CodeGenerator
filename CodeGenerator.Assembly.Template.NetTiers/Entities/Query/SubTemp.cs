using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities.Query
{
    public class SubTemp : ICodeTemplate<QueryFolder, EntitiesContextModel>
    {
        CompilationUnitSyntax compilationUnit = CompilationUnit()
    .WithUsings(
        List(new UsingDirectiveSyntax[] {
            UsingDirective(IdentifierName("System")),
            UsingDirective(QualifiedName(IdentifierName("System"), IdentifierName("ComponentModel"))),
            UsingDirective(IdentifierName("System.Collections")),
            UsingDirective(QualifiedName(
                QualifiedName(IdentifierName("System"), IdentifierName("Xml")),
                IdentifierName("Serialization"))
            ),
            UsingDirective(QualifiedName(
                IdentifierName("System"),
                IdentifierName("Runtime.Serialization"))
            )
        })
    )
    .WithMembers(
    SingletonList<MemberDeclarationSyntax>(
            NamespaceDeclaration(QualifiedName(IdentifierName("Behsaz"), IdentifierName("Entities")))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    ClassDeclaration("Sub")
                    .WithModifiers(TokenList(new[]{
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AbstractKeyword),
                        Token(SyntaxKind.PartialKeyword)
                    }))
                    .WithBaseList(
                        BaseList(SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(IdentifierName("EntityBaseCore"))
                        ))
                    )
                    .WithAttributeLists(
                        SingletonList(
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(IdentifierName("Serializable"))
                                )
                            )
                        )
                    )
                    .WithLeadingTrivia(
                        Trivia(
                            DocumentationCommentTrivia(SyntaxKind.SingleLineDocumentationCommentTrivia)
                            .WithContent(List(new XmlNodeSyntax[]
                            {
                                XmlText("/// "),
                                XmlElement(
                                    XmlElementStartTag(XmlName("summary")),
                                    XmlElementEndTag(XmlName("summary"))
                                ).WithContent(SingletonList<XmlNodeSyntax>(
                                    XmlText("The base object for each database table entity.")
                                )),
                                XmlText("\n")
                            }))
                        )
                    )
                    .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken))
                )
            )
        )
    )
    .NormalizeWhitespace();
        public IEnumerable<ICodeFile> Generate(IContextModel contextModel)
        {
            List<ICodeFile> codeFiles = new List<ICodeFile>() { new CodeFile("Sub", compilationUnit) };
            return codeFiles;
        }
    }
}
