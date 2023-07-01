namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Integer
            : Number
        {
            public Integer()
                : base()
            {

            }
            public Integer(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public static implicit operator int(Integer p) => int.Parse(p.Value);
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
