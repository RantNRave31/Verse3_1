namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class EndOfLine
            : SyntaxNode
        {
            public EndOfLine()
                : base()
            {

            }
            public EndOfLine(SymbolTable symbolTable)
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
