namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public abstract class SyntaxNode
            : Symbol
        {
            public SyntaxNode()
                : base()
            {

            }
            public SyntaxNode(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public SyntaxNode(SyntaxNode reference)
                : base(reference)
            {

            }
            public virtual SyntaxNode Accept(ICopySyntax visitor)
            {
                return visitor.Visit(this);
            }

            public virtual void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(SyntaxNode symbol)
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
