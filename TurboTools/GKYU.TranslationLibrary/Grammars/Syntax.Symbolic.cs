namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Symbolic
            : Atom
        {
            public Symbolic()
                : base()
            {

            }
            public Symbolic(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public static implicit operator string(Symbolic p) => p.Value;
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
