namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public interface IBuildSyntax
        {
            TypeDeclaration Declaration(string name);
            TypeDeclaration MacroDeclaration(string name);
            FileDeclaration FileDeclaration(string name);
            FileReference UsingStatement(string name);
            FileDeclaration FileDeclaration(string name, string[] usingStatements = null, string[] classNames = null, string style = null);
            NamespaceDeclaration NamespaceDeclaration(string name, string[] usingStatements = null, string[] classNames = null, string style = null);
            ClassDeclaration ClassDeclaration(string modifiers, string name, string[] classes = null, string style = null);
            MemberDeclaration MemberDeclaration(string modifiers, string type, string name, string style = null);
            MethodDeclaration MethodDeclaration(string modifiers, string type, string name, string style = null);
            LexerDeclaration LexerDeclaration(string modifiers, string type, string name, string style = null);
            ParserDeclaration ParserDeclaration(string modifiers, string type, string name, string style = null);
        }

    }
}
