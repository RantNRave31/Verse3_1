namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public abstract class SourceFile
            : FileDeclaration
        {
            public SourceFile()
                : base()
            {

            }
            public SourceFile(Symbol.Table symbolTable)
                : base(symbolTable)
            {

            }
        }

    }
}
