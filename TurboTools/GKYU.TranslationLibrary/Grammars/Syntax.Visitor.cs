using System;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Grammars.Analyzers;
    using GKYU.TranslationLibrary.Symbols;
    using GKYU.TranslationLibrary.Patterns;

    public partial class Syntax
    {
        public class Visitor
            : VisitorBase<SyntaxNode>
            , IVisitSyntax
        {

            public override void Visit(Symbol symbol)
            {
                throw new NotImplementedException();
            }

            public override void Visit(Symbol.Named namedSymbol)
            {
                throw new NotImplementedException();
            }
            public override void Visit(Syntax.SyntaxNode node)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Empty symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(Identifier symbol)
            {
                
            }
            public virtual void Visit(Number symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Integer symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Decimal symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(String symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Variable symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Reference symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Choice symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(Repitition symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(Sequence symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(Primative symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(TypeDeclaration declaration)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(MacroExpression symbol)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(FileReference usingStatement)
            {
                throw new NotImplementedException();
            }
            public virtual void Visit(FileDeclaration fileDeclaration)
            {
                foreach (FileReference usingStatement in fileDeclaration.FileReferences)
                {
                    Visit(usingStatement);
                };
                foreach (NamespaceDeclaration namespaceDeclaration in fileDeclaration.Namespaces)
                {
                    Visit(namespaceDeclaration);
                };
            }
            public virtual void Visit(NamespaceDeclaration namespaceDeclaration)
            {
                foreach (FileReference usingStatement in namespaceDeclaration.UsingStatements)
                {
                    Visit(usingStatement);
                };
                foreach (ClassDeclaration classDeclaration in namespaceDeclaration.Classes)
                {
                    Visit(classDeclaration);
                };
            }
            public virtual void Visit(ClassDeclaration classDeclaration)
            {
                foreach (ClassDeclaration memberClassDeclaration in classDeclaration.Classes)
                {
                    Visit(memberClassDeclaration);
                }
                foreach (MemberDeclaration memberDeclaration in classDeclaration.Members)
                {
                    Visit(memberDeclaration);
                }
                foreach (MethodDeclaration methodDeclaration in classDeclaration.Methods)
                {
                    Visit(methodDeclaration);
                }
            }

            public virtual void Visit(MemberDeclaration memberDeclaration)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(MethodDeclaration methodDeclaration)
            {
                throw new NotImplementedException();
            }



            public virtual void Visit(CharacterDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(CharacterSetDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(TokenDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(LexerDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(SymbolDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(ParserDeclaration symbol)
            {
                throw new NotImplementedException();
            }

            public virtual void Visit(SymbolTable symbolTable)
            {
                throw new NotImplementedException();
            }
            public virtual void Accept(IVisitAnalysisPass visitor)
            {
                throw new NotImplementedException();
            }


        }

    }
}
