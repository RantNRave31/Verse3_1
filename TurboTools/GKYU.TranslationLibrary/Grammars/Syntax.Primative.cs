namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Primative
            : ExpressionNode
        {
            public int referenceID;
            public SyntaxNode Data;
            public Primative()
                : base()
            {

            }
            public Primative(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Primative(Primative reference)
                : base(reference)
            {
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
