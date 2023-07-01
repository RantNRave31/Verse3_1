namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public partial class Syntax
    {
        public class MacroExpression
            : ExpressionNode
        {
            public string definition;
            public MacroExpression()
                : base()
            {

            }
            public MacroExpression(SymbolTable symbolTable)
                : base(symbolTable)
            {
                definition = string.Empty;
            }
            public MacroExpression(MacroExpression reference)
                : base(reference)
            {
                definition = reference.definition;
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(MacroExpression symbol)
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
