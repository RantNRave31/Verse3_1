namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Variable
            : SyntaxNode
        {
            public Variable()
                : base()
            {

            }
            public Variable(SymbolTable symbolTable)
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
