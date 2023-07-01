namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Sequence
            : ExpressionNode
        {
            public SyntaxNode first { get; set; }
            public SyntaxNode second { get; set; }
            public Sequence()
                : base()
            {

            }
            public Sequence(SymbolTable symbolTable)
                : base(symbolTable)
            {
                this.first = first;
                this.second = second;
            }
            public override string ToString()
            {
                return "Sequence";
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
