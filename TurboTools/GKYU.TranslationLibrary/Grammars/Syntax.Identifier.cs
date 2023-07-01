namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Identifier
            : Atom
        {
            public Identifier()
                : base()
            {

            }
            public Identifier(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Identifier(Identifier reference)
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
