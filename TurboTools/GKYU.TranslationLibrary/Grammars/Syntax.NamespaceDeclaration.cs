namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public partial class NamespaceDeclaration
            : TypeDeclaration
        {

            private FileReference[] usingStatementsField;

            private ClassDeclaration[] classesField;

            private string styleField;

            /// <remarks/>
            public FileReference[] UsingStatements
            {
                get
                {
                    return this.usingStatementsField;
                }
                set
                {
                    this.usingStatementsField = value;
                }
            }

            /// <remarks/>
            public ClassDeclaration[] Classes
            {
                get
                {
                    return this.classesField;
                }
                set
                {
                    this.classesField = value;
                }
            }

            /// <remarks/>
            public string Style
            {
                get
                {
                    return this.styleField;
                }
                set
                {
                    this.styleField = value;
                }
            }
            public NamespaceDeclaration()
                : base()
            {

            }
            public NamespaceDeclaration(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {

            }
            public NamespaceDeclaration(NamespaceDeclaration reference)
                : base(reference)
            {
                this.usingStatementsField = reference.usingStatementsField;
                this.classesField = reference.classesField;
                this.styleField = reference.styleField;
            }
            public new NamespaceDeclaration Accept(ICopySyntax visitor)
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
