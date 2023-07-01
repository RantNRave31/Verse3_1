namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Empty
            : ExpressionNode
        {
            public Empty()
                : base()
            {

            }
            public Empty(SymbolTable symbolTable)
                : base(symbolTable)
            {

            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
