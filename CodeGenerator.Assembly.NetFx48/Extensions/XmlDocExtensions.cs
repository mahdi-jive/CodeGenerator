using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeGenerator.Assembly.NetFx48.Extensions
{

    public static class XmlDocExtensions
    {
        public static IEnumerable<T> WithRegion<T>(this IEnumerable<T> nodes, string regionText = "", bool endRegionHasText = false) where T : SyntaxNode
        {
            var list = nodes.ToList();
            if (list.Count == 0)
                return list;

            // #region
            var regionTrivia = Trivia(
                RegionDirectiveTrivia(true)
                    .WithEndOfDirectiveToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.EndOfDirectiveToken,
                            string.IsNullOrWhiteSpace(regionText)
                                ? TriviaList()
                                : TriviaList(PreprocessingMessage(" " + regionText))
                        )
                    )
            );

            // #endregion (با متن)
            var endRegionTrivia = Trivia(
                EndRegionDirectiveTrivia(true)
                    .WithEndOfDirectiveToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.EndOfDirectiveToken,
                            string.IsNullOrWhiteSpace(regionText)
                                ? TriviaList()
                                : TriviaList(PreprocessingMessage(endRegionHasText ? " " + regionText : string.Empty))
                        )
                    )
            );

            // اولین عضو → #region
            list[0] = list[0].WithLeadingTrivia(
                list[0].GetLeadingTrivia().Insert(0, regionTrivia)
            );

            // آخرین عضو → #endregion
            list[^1] = list[^1].WithTrailingTrivia(
                list[^1].GetTrailingTrivia().Add(endRegionTrivia)
            );

            return list;
        }

        public static IEnumerable<SyntaxToken> XmlDocumentTreeSlash(this string text)
        {
            foreach (var item in text.Replace("\r", "").Split('\n'))
            {
                yield return XmlTextLiteral
                    (
                        XmlTreeSlash(),
                        $"\t{item}",
                        $"\t{item}",
                        TriviaList()
                    );
            }
            yield return XmlTextLiteral(XmlTreeSlash(), " ", " ", TriviaList());
        }
        private static SyntaxTriviaList XmlTreeSlash()
        {
            return TriviaList(DocumentationCommentExterior("\t///"));
        }

        public static IEnumerable<XmlNodeSyntax> ToSummaryXml(this string text)
        {
            return "summary".ToTagXml(text);
        }
        public static IEnumerable<XmlNodeSyntax> ToTagXml(this string tag, string text)
        {
            return new XmlNodeSyntax[]{
                XmlText()
                .WithTextTokens(TokenList(XmlLiteralTreeSlash())),
                XmlExampleElement(SingletonList<XmlNodeSyntax>(
                    XmlText().WithTextTokens(TokenList(text.XmlDocumentTreeSlash()))
                    ))
                .WithStartTag(XmlElementStartTag(XmlName(Identifier(tag))))
                .WithEndTag(XmlElementEndTag(XmlName(Identifier(tag)))),
                XmlText()
                .WithTextTokens(TokenList(new[]{XmlNewLine()}))
            };
        }
        public static SyntaxToken XmlLiteralTreeSlash()
        {
            return XmlTextLiteral(XmlTreeSlash(), " ", " ", TriviaList());
        }

        public static SyntaxToken XmlNewLine()
        {
            return XmlTextNewLine(TriviaList(), Environment.NewLine, Environment.NewLine, TriviaList());
        }
        public static T WithSummary<T>(this T node, string text)
    where T : MemberDeclarationSyntax
        {
            var xml = text.ToSummaryXml();

            // ساخت DocumentationCommentTriviaSyntax
            var docComment = DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                List<XmlNodeSyntax>(xml)
            );

            // تبدیل به SyntaxTrivia
            var trivia = SyntaxFactory.Trivia(docComment);

            return node.WithLeadingTrivia(TriviaList(trivia));
        }


        public static IEnumerable<XmlNodeSyntax> ToRemarksXml(this string text)
        {
            return "remarks".ToTagXml(text);
        }

        public static T WithRemarks<T>(this T node, string text)
            where T : MemberDeclarationSyntax
        {
            var xml = text.ToRemarksXml();

            var docComment = DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                List(xml)
            );

            var trivia = SyntaxFactory.Trivia(docComment);

            return node.WithLeadingTrivia(TriviaList(trivia));
        }
    }

}
