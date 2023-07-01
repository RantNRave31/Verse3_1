namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public enum SYMBOL : int
        {
            // ////////////////////////////////////////////////////////////////
            // Reserved Symbols
            EndOfFile,
            Empty,
            EndOfLine,
            // Atomic Nodes
            Identifier,
            Decimal,
            Integer,
            String,
            Symbolic,
            Variable,
            Reference,
            FileReference,
            // Expression Nodes
            Primative,
            Repitition,
            Choice,
            Sequence,
            // Declaration Nodes
            CharacterDeclaration,
            CharacterSetDeclaration,
            MacroExpression,
            TokenDeclaration,
            LexerDeclaration,
            SymbolDeclaration,
            ParserDeclaration,
            FileDeclaration,
            NamespaceDeclaration,
            ClassDeclaration,
            MemberDeclaration,
            MethodDeclaration,
            // Statement Nodes
            Assign,
            Ellipsis,
            EndStatement,
            CharacterLitoral,
        }

    }
}
