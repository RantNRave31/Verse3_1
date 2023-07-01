namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public partial class Syntax
    {
        public class TokenDeclaration
            : TypeDeclaration
        {
            public string definition;
            public ExpressionNode expression;
            public TokenDeclaration()
                : base()
            {

            }
            public TokenDeclaration(SymbolTable symbolTable)
                : base(symbolTable)
            {
                definition = string.Empty;
                this.expression = null;
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(TokenDeclaration symbol)
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
