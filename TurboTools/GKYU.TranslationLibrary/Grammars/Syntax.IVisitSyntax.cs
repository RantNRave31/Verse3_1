namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Grammars.Analyzers;
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public interface IVisitSyntax
            : IVisitSymbol
        {
            void Visit(SyntaxNode node);
            void Visit(Empty symbol);
            void Visit(Identifier symbol);
            void Visit(Number symbol);
            void Visit(Integer symbol);
            void Visit(Decimal symbol);
            void Visit(String symbol);
            void Visit(Variable symbol);
            void Visit(Reference symbol);
            void Visit(Choice symbol);
            void Visit(Repitition symbol);
            void Visit(Sequence symbol);
            void Visit(Primative symbol);
            void Visit(MacroExpression symbol);
            void Visit(CharacterDeclaration symbol);
            void Visit(CharacterSetDeclaration symbol);
            void Visit(TypeDeclaration symbol);
            void Visit(TokenDeclaration symbol);
            void Visit(SymbolDeclaration symbol);
            void Visit(FileDeclaration uileDeclaration);
            void Visit(FileReference usingStatement);
            void Visit(NamespaceDeclaration namespaceDeclaration);
            void Visit(ClassDeclaration classDeclaration);
            void Visit(MemberDeclaration memberDeclaration);
            void Visit(MethodDeclaration methodDeclaration);
            void Visit(LexerDeclaration lexerDeclaration);
            void Visit(ParserDeclaration parserDeclaration);
            void Visit(SymbolTable symbolTable);
            void Accept(IVisitAnalysisPass visitor);
        }

    }
}
