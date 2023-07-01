namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public abstract class ExpressionNode
            : SyntaxNode
        {
            public ExpressionNode expression;
            public ExpressionNode()
                : base()
            {

            }
            public ExpressionNode(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public ExpressionNode(ExpressionNode reference)
                : base(reference)
            {
                expression = reference.expression;
            }
            public override string ToString()
            {
                return string.Format("{0}[{1}]", this.GetType().Name, SymbolID);
            }
            public abstract override void Accept(IVisitSyntax visitor);
        }

    }
}
