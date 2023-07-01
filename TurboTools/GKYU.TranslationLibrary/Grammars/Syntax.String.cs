namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class String
            : Atom
        {
            public String()
                : base()
            {

            }
            public String(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public static implicit operator string(String p) => p.Value;
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
