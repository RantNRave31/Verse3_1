namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public interface ICopySyntax
        {
            SyntaxNode Visit(SyntaxNode node);
            Empty Visit(Empty empty);
            Identifier Visit(Identifier identifier);
            ExpressionNode Visit(ExpressionNode expressionNode);
            Choice Visit(Choice choice);
            Sequence Visit(Sequence sequence);
            Repitition Visit(Repitition repitition);
            Primative Visit(Primative primative);
            Reference Visit(Reference symbol);
            TypeDeclaration Visit(TypeDeclaration symbol);
            FileDeclaration Visit(FileDeclaration fileDeclaration);
            FileReference Visit(FileReference usingStatement);
            NamespaceDeclaration Visit(NamespaceDeclaration namespaceDeclaration);
            ClassDeclaration Visit(ClassDeclaration classDeclaration);
            MemberDeclaration Visit(MemberDeclaration memberDeclaration);
            MethodDeclaration Visit(MethodDeclaration methodDeclaration);
            LexerDeclaration Visit(LexerDeclaration lexerDeclaration);
            ParserDeclaration Visit(ParserDeclaration parserDeclaration);
        }

    }
}
