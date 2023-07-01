namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class CharacterDeclaration
            : TypeDeclaration
        {
            public int CharacterCode;
            public CharacterDeclaration()
                : base()
            {

            }
            public CharacterDeclaration(SymbolTable symbolTable)
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
