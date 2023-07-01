namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public partial class Syntax
    {
        public class LexerDeclaration
            : TypeDeclaration
        {
            public string definition;
            public LexerDeclaration()
                : base()
            {

            }
            public LexerDeclaration(SymbolTable symbolTable)
                : base(symbolTable)
            {

            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(LexerDeclaration symbol)
            {
                Graph<int, int> result;
                Grammar2NFA visitor = new Grammar2NFA();
                symbol.Accept(visitor);
                result = visitor.graph;
                return result;
            }
        }

    }
}
