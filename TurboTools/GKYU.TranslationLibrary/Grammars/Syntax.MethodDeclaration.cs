namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public partial class MethodDeclaration
            : Syntax.SyntaxNode
        {

            private string modifiersField;

            private string[] argumentsField;

            private string typeField;

            private string styleField;

            /// <remarks/>
            public string Modifiers
            {
                get
                {
                    return this.modifiersField;
                }
                set
                {
                    this.modifiersField = value;
                }
            }

            /// <remarks/>
            public string[] Arguments
            {
                get
                {
                    return this.argumentsField;
                }
                set
                {
                    this.argumentsField = value;
                }
            }

            /// <remarks/>
            public string Type
            {
                get
                {
                    return this.typeField;
                }
                set
                {
                    this.typeField = value;
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
            public MethodDeclaration()
                : base()
            {

            }
            public MethodDeclaration(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
                Arguments = new string[] { };

            }
            public MethodDeclaration(MethodDeclaration reference)
                : base(reference)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
                Arguments = new string[] { };
            }
            public new MethodDeclaration Accept(ICopySyntax visitor)
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
