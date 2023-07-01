using System;

namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Builder
            : IBuildSyntax
        {
            public virtual ClassDeclaration ClassDeclaration(string modifiers, string name, string[] classes = null, string style = null)
            {
                throw new NotImplementedException();
            }

            public TypeDeclaration Declaration(string name)
            {
                throw new NotImplementedException();
            }

            public virtual FileDeclaration FileDeclaration(string name, string[] usingStatements = null, string[] classNames = null, string style = null)
            {
                throw new NotImplementedException();
            }

            public virtual FileDeclaration FileDeclaration(string name)
            {
                throw new NotImplementedException();
            }

            public virtual LexerDeclaration LexerDeclaration(string modifiers, string type, string name, string style = null)
            {
                throw new NotImplementedException();
            }

            public TypeDeclaration MacroDeclaration(string name)
            {
                throw new NotImplementedException();
            }

            public virtual MemberDeclaration MemberDeclaration(string modifiers, string type, string name, string style = null)
            {
                throw new NotImplementedException();
            }

            public virtual MethodDeclaration MethodDeclaration(string modifiers, string type, string name, string style = null)
            {
                throw new NotImplementedException();
            }

            public virtual NamespaceDeclaration NamespaceDeclaration(string name, string[] usingStatements = null, string[] classNames = null, string style = null)
            {
                throw new NotImplementedException();
            }

            public virtual ParserDeclaration ParserDeclaration(string modifiers, string type, string name, string style = null)
            {
                throw new NotImplementedException();
            }

            public virtual FileReference UsingStatement(string name)
            {
                throw new NotImplementedException();
            }
        }

    }
}
