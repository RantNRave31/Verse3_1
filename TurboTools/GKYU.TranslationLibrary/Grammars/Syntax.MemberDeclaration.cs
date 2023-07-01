namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public partial class MemberDeclaration
            : Syntax.SyntaxNode
        {

            private string modifiersField;

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
            public MemberDeclaration()
                : base()
            {

            }
            public MemberDeclaration(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
            }
            public MemberDeclaration(MemberDeclaration reference)
                : base(reference)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
            }
            public new MemberDeclaration Accept(ICopySyntax visitor)
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
