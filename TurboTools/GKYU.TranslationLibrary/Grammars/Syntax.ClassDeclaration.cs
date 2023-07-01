namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public partial class ClassDeclaration
            : Syntax.TypeDeclaration
        {

            private string modifiersField;

            private ClassDeclaration[] classesField;

            private MemberDeclaration[] membersField;

            private MethodDeclaration[] methodsField;

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
            public MemberDeclaration[] Members
            {
                get
                {
                    return this.membersField;
                }
                set
                {
                    this.membersField = value;
                }
            }

            /// <remarks/>
            public MethodDeclaration[] Methods
            {
                get
                {
                    return this.methodsField;
                }
                set
                {
                    this.methodsField = value;
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
            public ClassDeclaration()
                : base()
            {

            }
            public ClassDeclaration(Symbol.Table symbolTable)
                : base((Syntax.SymbolTable)symbolTable)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
                Classes = new ClassDeclaration[] { };
                Members = new MemberDeclaration[] { };
                Methods = new MethodDeclaration[] { };

            }
            public ClassDeclaration(ClassDeclaration reference)
                : base(reference)
            {
                Modifiers = string.Empty;
                Name = string.Empty;
                Type = string.Empty;
                Style = string.Empty;
                Classes = new ClassDeclaration[] { };
                Members = new MemberDeclaration[] { };
                Methods = new MethodDeclaration[] { };
            }
            public new ClassDeclaration Accept(ICopySyntax visitor)
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
