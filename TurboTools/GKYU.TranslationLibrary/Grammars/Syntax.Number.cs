namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Number
            : Atom
        {
            public Number()
                : base()
            {

            }
            public Number(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Number(Number reference)
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
