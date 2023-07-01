namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public class FileReference
            : SyntaxNode
        {
            public string FileType { get; set; }
            public string FileGuid { get; set; }
            public string MajorVersion { get; set; }
            public string MinorVersion { get; set; }
            public string FilePath { get; set; }
            public FileReference()
                : base()
            {

            }
            public FileReference(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {

            }
            public FileReference(Symbol.Table symbolTable, string value)
                : this(symbolTable)
            {
                this.Value = value;
            }
            public FileReference(FileReference reference)
                : base(reference)
            {
            }
            public new FileReference Accept(ICopySyntax visitor)
            {
                return visitor.Visit(this);
            }

            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
