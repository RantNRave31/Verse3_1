namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class EndOfFile
            : SyntaxNode
        {
            public EndOfFile()
                : base()
            {

            }
            public EndOfFile(SymbolTable symbolTable)
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
