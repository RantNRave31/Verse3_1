namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public abstract class Atom
            : SyntaxNode
        {
            public int declarationSymbolID = 0;
            public Atom()
                : base()
            {

            }
            public Atom(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Atom(Atom reference)
                : base(reference)
            {
            }
            public TypeDeclaration Declaration
            {
                get
                {
                    return (TypeDeclaration)SymbolTable[declarationSymbolID];
                }
            }
        }

    }
}
