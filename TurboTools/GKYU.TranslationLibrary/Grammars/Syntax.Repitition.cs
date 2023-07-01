namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Repitition
            : ExpressionNode
        {
            public SyntaxNode expression { get; set; }
            public Repitition()
                : base()
            {

            }
            public Repitition(SymbolTable symbolTable)
                : base(symbolTable)
            {
                this.expression = expression;
            }
            public override string ToString()
            {
                return "Repitition";
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
