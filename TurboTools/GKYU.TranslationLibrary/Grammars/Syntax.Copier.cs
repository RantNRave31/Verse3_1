using System;
using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Grammars
{
    public partial class Syntax
    {
        public class Copier
            : ICopySyntax
        {
            public SyntaxNode Visit(SyntaxNode node)
            {
                return node;
            }
            public virtual FileReference Visit(FileReference usingStatement)
            {
                return new FileReference(usingStatement);
            }

            public virtual FileDeclaration Visit(FileDeclaration fileDeclaration)
            {
                FileDeclaration result = new FileDeclaration(fileDeclaration);
                List<FileReference> list = new List<FileReference>();
                foreach (FileReference usingStatement in fileDeclaration.FileReferences)
                {
                    list.Add(usingStatement.Accept(this));
                };
                result.FileReferences = list.ToArray();
                List<NamespaceDeclaration> list2 = new List<NamespaceDeclaration>();
                foreach (NamespaceDeclaration namespaceDeclaration in fileDeclaration.Namespaces)
                {
                    list2.Add(namespaceDeclaration.Accept(this));
                };
                result.Namespaces = list2.ToArray();
                return result;
            }

            public virtual NamespaceDeclaration Visit(NamespaceDeclaration namespaceDeclaration)
            {
                NamespaceDeclaration result = new NamespaceDeclaration(namespaceDeclaration);
                List<ClassDeclaration> list = new List<ClassDeclaration>();
                foreach (ClassDeclaration classDeclaration in namespaceDeclaration.Classes)
                {
                    list.Add(classDeclaration.Accept(this));
                };
                result.Classes = list.ToArray();
                return result;
            }

            public virtual ClassDeclaration Visit(ClassDeclaration classDeclaration)
            {
                foreach (MemberDeclaration memberDeclaration in classDeclaration.Members)
                {
                    Visit(memberDeclaration.Accept(this));
                }
                foreach (MethodDeclaration methodDeclaration in classDeclaration.Methods)
                {
                    Visit(methodDeclaration.Accept(this));
                }
                return classDeclaration;
            }

            public virtual MemberDeclaration Visit(MemberDeclaration memberDeclaration)
            {
                return memberDeclaration;
            }

            public virtual MethodDeclaration Visit(MethodDeclaration methodDeclaration)
            {
                return methodDeclaration;
            }

            public Empty Visit(Empty empty)
            {
                throw new NotImplementedException();
            }

            public Identifier Visit(Identifier identifier)
            {
                throw new NotImplementedException();
            }

            public ExpressionNode Visit(ExpressionNode expressionNode)
            {
                throw new NotImplementedException();
            }

            public Choice Visit(Choice choice)
            {
                throw new NotImplementedException();
            }

            public Sequence Visit(Sequence sequence)
            {
                throw new NotImplementedException();
            }

            public Repitition Visit(Repitition repitition)
            {
                throw new NotImplementedException();
            }

            public Primative Visit(Primative primative)
            {
                throw new NotImplementedException();
            }

            public LexerDeclaration Visit(LexerDeclaration lexerDeclaration)
            {
                throw new NotImplementedException();
            }

            public ParserDeclaration Visit(ParserDeclaration parserDeclaration)
            {
                throw new NotImplementedException();
            }

            public TypeDeclaration Visit(TypeDeclaration declaration)
            {
                throw new NotImplementedException();
            }

            public Reference Visit(Reference symbol)
            {
                throw new NotImplementedException();
            }

        }

    }
}
