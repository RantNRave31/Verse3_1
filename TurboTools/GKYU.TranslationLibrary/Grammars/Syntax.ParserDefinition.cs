namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public partial class Syntax
    {
        public class ParserDeclaration
            : TypeDeclaration
        {
            public string definition;
            public ParserDeclaration()
                : base()
            {

            }
            public ParserDeclaration(SymbolTable symbolTable)
                : base(symbolTable)
            {

            }
            public override void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public static explicit operator Graph<int, int>(ParserDeclaration symbol)
            {
                Graph<int, int> result;
                Grammar2NFA visitor = new Grammar2NFA();
                symbol.Accept(visitor);
                result = visitor.graph;
                return result;
            }
        }

    }
}
