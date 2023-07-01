namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Reference
            : SyntaxNode
        {
            public Identifier Identifier;
            public Integer Index;
            public object _reference;
            public Reference()
                : base()
            {

            }
            public Reference(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public Reference(SymbolTable symbolTable, string value)
                : this(symbolTable)
            {
                this.Value = value;
            }
            public Reference(Reference reference)
                : base(reference)
            {
                Identifier = reference.Identifier;
                Index = reference.Index;
                _reference = reference._reference;
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
