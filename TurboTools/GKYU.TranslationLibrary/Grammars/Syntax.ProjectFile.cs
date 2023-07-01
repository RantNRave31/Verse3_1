namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public abstract class ProjectFile
            : FileDeclaration
        {
            public ProjectFile()
                : base()
            {

            }
            public ProjectFile(Symbol.Table symbolTable)
                : base(symbolTable)
            {

            }
        }

    }
}
