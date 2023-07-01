namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Choice
            : ExpressionNode
        {
            public SyntaxNode thisOne;
            public SyntaxNode thatOne;
            public Choice()
                : base()
            {

            }
            public Choice(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public override string ToString()
            {
                return "Choice";
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
