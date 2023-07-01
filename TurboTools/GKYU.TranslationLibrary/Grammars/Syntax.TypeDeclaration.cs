namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class TypeDeclaration
            : SyntaxNode
        {
            public Identifier identifier;
            public ExpressionNode expression;
            public TypeDeclaration()
                : base()
            {

            }
            public TypeDeclaration(SymbolTable symbolTable)
                : base(symbolTable)
            {
            }
            public TypeDeclaration(TypeDeclaration reference)
                : base(reference)
            {

            }
            public static implicit operator ExpressionNode(TypeDeclaration d)
            {
                return ((Syntax.SymbolTable)d.SymbolTable).MakePrimative(d);
            }
            public override string ToString()
            {
                return string.Format("{0}[{1}]", this.identifier.Name, SymbolID);
            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
        }

    }
}
