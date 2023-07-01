namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public partial class Syntax
    {
        public class SymbolDeclaration
            : TypeDeclaration
        {
            public string definition;
            public ExpressionNode expression;
            public SymbolDeclaration()
                : base()
            {

            }
            public SymbolDeclaration(SymbolTable symbolTable)
                : base(symbolTable)
            {
                definition = string.Empty;
                this.expression = null;
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(SymbolDeclaration symbol)
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
