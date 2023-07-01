namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections;

    public partial class Syntax
    {
        public class CharacterSetDeclaration
            : TypeDeclaration
        {
            public CharacterSet CharacterSet { get; set; }
            public CharacterSetDeclaration()
                : base()
            {

            }
            public CharacterSetDeclaration(SymbolTable symbolTable)
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
