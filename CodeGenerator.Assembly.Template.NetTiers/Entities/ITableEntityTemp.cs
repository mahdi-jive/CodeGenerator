using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.Column;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeGenerator.Assembly.Template.NetTiers.Entities
{
    public class ITableEntityTemp : ICodeTemplate<EntitiesFactory, DatabaseInfoModel>
    {
        private SyntaxList<UsingDirectiveSyntax> GetUsing()
        {
            return List<UsingDirectiveSyntax>(
         new UsingDirectiveSyntax[]{
            UsingDirective(
                IdentifierName("System")),
            UsingDirective(
                QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("ComponentModel")))});
        }
        private NamespaceDeclarationSyntax GetNamespace(string RootNamespace)
        {
            return NamespaceDeclaration(
            QualifiedName(
                IdentifierName(RootNamespace),
                IdentifierName("Entities")));
        }
        private InterfaceDeclarationSyntax GetInterface(string name)
        {
            return InterfaceDeclaration($"I{name.Pascalize()}");//TbasBuildingGroup
        }
        private SyntaxTriviaList GetDocumentInterface(string name)
        {
            return TriviaList(
                                Trivia(
                                    DocumentationCommentTrivia(
                                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                                        List<XmlNodeSyntax>(
                                            new XmlNodeSyntax[]{
                                                XmlText()
                                                .WithTextTokens(
                                                    TokenList(
                                                        XmlTextLiteral(
                                                            TriviaList(
                                                                DocumentationCommentExterior("///")),
                                                            " ",
                                                            " ",
                                                            TriviaList()))),
                                                XmlExampleElement(
                                                    SingletonList<XmlNodeSyntax>(
                                                        XmlText()
                                                        .WithTextTokens(
                                                            TokenList(
                                                                new []{
                                                                    XmlTextNewLine(
                                                                        TriviaList(),
                                                                        Environment.NewLine,
                                                                        Environment.NewLine,
                                                                        TriviaList()),
                                                                    XmlTextLiteral(
                                                                        TriviaList(
                                                                            DocumentationCommentExterior("\t///")),
                                                                        $"\t\tThe data structure representation of the '{name}' table via interface.",//TBASBuildingGroup
                                                                        $"\t\tThe data structure representation of the '{name}' table via interface.",//TBASBuildingGroup
                                                                        TriviaList()),
                                                                    XmlTextNewLine(
                                                                        TriviaList(),
                                                                        Environment.NewLine,
                                                                        Environment.NewLine,
                                                                        TriviaList()),
                                                                    XmlTextLiteral(
                                                                        TriviaList(
                                                                            DocumentationCommentExterior("\t///")),
                                                                        " ",
                                                                        " ",
                                                                        TriviaList())}))))
                                                .WithStartTag(
                                                    XmlElementStartTag(
                                                        XmlName(
                                                            Identifier("summary"))))
                                                .WithEndTag(
                                                    XmlElementEndTag(
                                                        XmlName(
                                                            Identifier("summary")))),
                                                XmlText()
                                                .WithTextTokens(
                                                    TokenList(
                                                        new []{
                                                            XmlTextNewLine(
                                                                TriviaList(),
                                                                Environment.NewLine,
                                                                Environment.NewLine,
                                                                TriviaList()),
                                                            XmlTextLiteral(
                                                                TriviaList(
                                                                    DocumentationCommentExterior("\t///")),
                                                                " ",
                                                                " ",
                                                                TriviaList())})),
                                                XmlExampleElement(
                                                    SingletonList<XmlNodeSyntax>(
                                                        XmlText()
                                                        .WithTextTokens(
                                                            TokenList(
                                                                new []{
                                                                    XmlTextNewLine(
                                                                        TriviaList(),
                                                                        Environment.NewLine,
                                                                        Environment.NewLine,
                                                                        TriviaList()),
                                                                    XmlTextLiteral(
                                                                        TriviaList(
                                                                            DocumentationCommentExterior("\t///")),
                                                                        " \tThis struct is generated by a tool and should never be modified.",
                                                                        " \tThis struct is generated by a tool and should never be modified.",
                                                                        TriviaList()),
                                                                    XmlTextNewLine(
                                                                        TriviaList(),
                                                                        Environment.NewLine,
                                                                        Environment.NewLine,
                                                                        TriviaList()),
                                                                    XmlTextLiteral(
                                                                        TriviaList(
                                                                            DocumentationCommentExterior("\t///")),
                                                                        " ",
                                                                        " ",
                                                                        TriviaList())}))))
                                                .WithStartTag(
                                                    XmlElementStartTag(
                                                        XmlName(
                                                            Identifier("remarks"))))
                                                .WithEndTag(
                                                    XmlElementEndTag(
                                                        XmlName(
                                                            Identifier("remarks")))),
                                                XmlText()
                                                .WithTextTokens(
                                                    TokenList(
                                                        XmlTextNewLine(
                                                            TriviaList(),
                                                            Environment.NewLine,
                                                            Environment.NewLine,
                                                            TriviaList())))}))));
        }

        private async Task<IEnumerable<PropertyDeclarationSyntax>> GetPropertyColumn(ITable table)
        {
            List<PropertyDeclarationSyntax> properties = new List<PropertyDeclarationSyntax>();
            foreach (var column in await table.Columns)
            {
                SyntaxTriviaList document = column.IsPrimaryKey ? GetDocumentPropertyPrimaryKeyColumn(column, table.Name) : GetDocumentPropertyColumn(column);
                var property = PropertyDeclaration(ParseTypeName(column.MapSqlTypeToCSharp()), column.Name)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors
                    (
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                        AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                    ).WithLeadingTrivia(document);
                properties.Add(property);
            }
            return properties;
        }
        private SyntaxTriviaList GetDocumentPropertyPrimaryKeyColumn(IColumnTable column, string tableName)
        {
            return TriviaList(
                                                Trivia(
                                                    DocumentationCommentTrivia(
                                                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                                                        List<XmlNodeSyntax>(
                                                            new XmlNodeSyntax[]{
                                                                XmlText()
                                                                .WithTextTokens(
                                                                    TokenList(
                                                                        XmlTextLiteral(
                                                                            TriviaList(
                                                                                DocumentationCommentExterior("///")),
                                                                            " ",
                                                                            " ",
                                                                            TriviaList()))),
                                                                XmlExampleElement(
                                                                    SingletonList<XmlNodeSyntax>(
                                                                        XmlText()
                                                                        .WithTextTokens(
                                                                            TokenList(
                                                                                new []{
                                                                                    XmlTextLiteral(
                                                                                        TriviaList(),
                                                                                        "\t\t\t",
                                                                                        "\t\t\t",
                                                                                        TriviaList()),
                                                                                    XmlTextNewLine(
                                                                                        TriviaList(),
                                                                                        Environment.NewLine,
                                                                                        Environment.NewLine,
                                                                                        TriviaList()),
                                                                                    XmlTextLiteral(
                                                                                        TriviaList(
                                                                                            DocumentationCommentExterior("\t\t///")),
                                                                                          $" {column.Name} : { column.Description?.Replace("\n","\n\t/// \t\t/// ")??""} ",
                                                                                          $" {column.Name} : { column.Description?.Replace("\n","\n\t/// \t\t/// ")??""} ",
                                                                                        TriviaList()),
                                                                                    XmlTextNewLine(
                                                                                        TriviaList(),
                                                                                        Environment.NewLine,
                                                                                        Environment.NewLine,
                                                                                        TriviaList()),
                                                                                    XmlTextLiteral(
                                                                                        TriviaList(
                                                                                            DocumentationCommentExterior("\t\t///")),
                                                                                        " ",
                                                                                        " ",
                                                                                        TriviaList())}))))
                                                                .WithStartTag(
                                                                    XmlElementStartTag(
                                                                        XmlName(
                                                                            Identifier("summary"))))
                                                                .WithEndTag(
                                                                    XmlElementEndTag(
                                                                        XmlName(
                                                                            Identifier("summary")))),
                                                                XmlText()
                                                                .WithTextTokens(
                                                                    TokenList(
                                                                        new []{
                                                                            XmlTextNewLine(
                                                                                TriviaList(),
                                                                                Environment.NewLine,
                                                                                Environment.NewLine,
                                                                                TriviaList()),
                                                                            XmlTextLiteral(
                                                                                TriviaList(
                                                                                    DocumentationCommentExterior("\t\t///")),
                                                                                " ",
                                                                                " ",
                                                                                TriviaList())})),
                                                                XmlExampleElement(
                                                                    SingletonList<XmlNodeSyntax>(
                                                                        XmlText()
                                                                        .WithTextTokens(
                                                                            TokenList(
                                                                                XmlTextLiteral(
                                                                                    TriviaList(),
                                                                                    $"Member of the primary key of the underlying table \"{tableName}\"",
                                                                                    $"Member of the primary key of the underlying table \"{tableName}\"",
                                                                                    TriviaList())))))
                                                                .WithStartTag(
                                                                    XmlElementStartTag(
                                                                        XmlName(
                                                                            Identifier("remarks"))))
                                                                .WithEndTag(
                                                                    XmlElementEndTag(
                                                                        XmlName(
                                                                            Identifier("remarks")))),
                                                                XmlText()
                                                                .WithTextTokens(
                                                                    TokenList(
                                                                        XmlTextNewLine(
                                                                            TriviaList(),
                                                                            Environment.NewLine,
                                                                            Environment.NewLine,
                                                                            TriviaList())))}))));
        }
        private SyntaxTriviaList GetDocumentPropertyColumn(IColumnTable column)
        {
            return TriviaList(
                                                    Trivia(
                                                        DocumentationCommentTrivia(
                                                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                                                            List<XmlNodeSyntax>(
                                                                new XmlNodeSyntax[]{
                                                                    XmlText()
                                                                    .WithTextTokens(
                                                                        TokenList(
                                                                            XmlTextLiteral(
                                                                                TriviaList(
                                                                                    DocumentationCommentExterior("///")),
                                                                                " ",
                                                                                " ",
                                                                                TriviaList()))),
                                                                    XmlExampleElement(
                                                                        SingletonList<XmlNodeSyntax>(
                                                                            XmlText()
                                                                            .WithTextTokens(
                                                                                TokenList(
                                                                                    new []{
                                                                                        XmlTextNewLine(
                                                                                            TriviaList(),
                                                                                            Environment.NewLine,
                                                                                            Environment.NewLine,
                                                                                            TriviaList()),
                                                                                        XmlTextLiteral(
                                                                                            TriviaList(
                                                                                                DocumentationCommentExterior("\t\t///")),
                                                                                             $" {column.Name} : { column.Description?.Replace("\n","\n\t/// \t\t/// ")??""} ",
                                                                                             $" {column.Name} : { column.Description?.Replace("\n","\n\t/// \t\t/// ")??""} ",
                                                                                            TriviaList()),
                                                                                        XmlTextNewLine(
                                                                                            TriviaList(),
                                                                                            Environment.NewLine,
                                                                                            Environment.NewLine,
                                                                                            TriviaList()),
                                                                                        XmlTextLiteral(
                                                                                            TriviaList(
                                                                                                DocumentationCommentExterior("\t\t///")),
                                                                                            " ",
                                                                                            " ",
                                                                                            TriviaList())}))))
                                                                    .WithStartTag(
                                                                        XmlElementStartTag(
                                                                            XmlName(
                                                                                Identifier("summary"))))
                                                                    .WithEndTag(
                                                                        XmlElementEndTag(
                                                                            XmlName(
                                                                                Identifier("summary")))),
                                                                    XmlText()
                                                                    .WithTextTokens(
                                                                        TokenList(
                                                                            XmlTextNewLine(
                                                                                TriviaList(),
                                                                                Environment.NewLine,
                                                                                Environment.NewLine,
                                                                                TriviaList())))}))));
        }
        private async Task<IEnumerable<ICodeFile>> GetCompilationUnit(DatabaseInfoModel model)
        {
            List<ICodeFile> codeFiles = new List<ICodeFile>();
            foreach (var table in await model.Tables)
            {
                var propertyColumns = await GetPropertyColumn(table);
                var tree = CompilationUnit()
                    .WithUsings(GetUsing()).WithMembers(SingletonList<MemberDeclarationSyntax>(
                        GetNamespace(model.RootNameSpace)
                        .WithMembers(SingletonList<MemberDeclarationSyntax>(
                GetInterface(table.Name)
                .WithModifiers(TokenList(
                    Token(
                    GetDocumentInterface(table.Name),
                    SyntaxKind.PublicKeyword,
                    TriviaList())))
                .WithMembers(List<MemberDeclarationSyntax>(propertyColumns))
                .WithCloseBraceToken(Token(TriviaList(Trivia(EndRegionDirectiveTrivia(true)
                .WithEndOfDirectiveToken(
                    Token(
                        TriviaList(PreprocessingMessage("Data Properties")),
                        SyntaxKind.EndOfDirectiveToken,
                        TriviaList())))),
                        SyntaxKind.CloseBraceToken,
                        TriviaList())))))).NormalizeWhitespace();
                codeFiles.Add(new CodeFile($"I{table.Name.Pascalize()}.cs", $"I{table.Name.Pascalize()}", tree));
            }

            return codeFiles;
        }
        public async Task<IEnumerable<ICodeFile>> Generate(IContextModel contextModel)
        {
            var model = contextModel as DatabaseInfoModel;
            IEnumerable<ICodeFile> codeFiles = await GetCompilationUnit(model);

            return codeFiles;
        }
    }
}
