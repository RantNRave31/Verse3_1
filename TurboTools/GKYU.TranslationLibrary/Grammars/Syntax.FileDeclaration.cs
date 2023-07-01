namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public partial class FileDeclaration
            : TypeDeclaration
        {
            public string FileType { get; set; }
            private FileReference[] fileReferences;
            private NamespaceDeclaration[] namespaceDeclarationsField;
            public FileReference[] FileReferences
            {
                get
                {
                    return this.fileReferences;
                }
                set
                {
                    this.fileReferences = value;
                }
            }
            public NamespaceDeclaration[] Namespaces
            {
                get
                {
                    return this.namespaceDeclarationsField;
                }
                set
                {
                    this.namespaceDeclarationsField = value;
                }
            }
            public FileDeclaration(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {

            }
            public FileDeclaration()
                : base()
            {

            }
            public FileDeclaration(FileDeclaration reference)
                : base(reference)
            {
                this.fileReferences = reference.fileReferences;
            }
            public new FileDeclaration Accept(ICopySyntax visitor)
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
