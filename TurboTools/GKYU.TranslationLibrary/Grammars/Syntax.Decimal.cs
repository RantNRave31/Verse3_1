namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Decimal
            : Number
        {
            public Decimal()
                : base()
            {

            }
            public Decimal(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Decimal(Decimal reference)
                : base(reference)
            {
            }
            public static implicit operator decimal(Decimal p) => decimal.Parse(p.Value);
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
