using System.IO;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Translators;

    public partial class Syntax
    {
        public class Writer
            : WriterBase<SyntaxNode>
            , IWriteSymbolVisitors<SyntaxNode>
        {
            public class DefaultReport
                : Syntax.Visitor
                , IVisitSyntax
            {
                protected Writer _writer;
                public DefaultReport(Writer writer)
                    : base()
                {
                    _writer = writer;
                }
                public override void Visit(SyntaxNode node)
                {
                    _writer.WriteLine("NODE[{0}]", node.Name);

                }
                public override void Visit(FileReference usingStatement)
                {
                    _writer.WriteLine("USING[{0}]", usingStatement.Value);
                }
                public override void Visit(NamespaceDeclaration namespaceDeclaration)
                {
                    _writer.WriteLine("NAMESPACE[{0}]", namespaceDeclaration.Name);
                    _writer.PushTab();

                    _writer.PopTab();
                }
                public override void Visit(ClassDeclaration classDeclaration)
                {
                    _writer.WriteLine("CLASS[{0}]", classDeclaration.Name);
                    _writer.PushTab();

                    _writer.PopTab();
                }
                public override void Visit(MemberDeclaration classDeclaration)
                {
                    _writer.WriteLine("MEMBER[{0}]", classDeclaration.Name);
                    _writer.PushTab();

                    _writer.PopTab();
                }
                public override void Visit(MethodDeclaration classDeclaration)
                {
                    _writer.WriteLine("CLASS[{0}]", classDeclaration.Name);
                    _writer.PushTab();

                    _writer.PopTab();
                }

                public override void Visit(FileDeclaration fileDeclaration)
                {

                }
            }
            public class CSharpSourceCode
                : Syntax.Visitor
                , IVisitSyntax
            {
                protected Writer _writer;
                public CSharpSourceCode(Writer writer)
                    : base()
                {
                    _writer = writer;
                }
                public override void Visit(SyntaxNode node)
                {
                    _writer.Tab(); _writer.WriteLine("NODE[{0}]", node.Name);
                    base.Visit(node);
                }
                public override void Visit(FileReference usingStatement)
                {
                    _writer.Tab(); _writer.WriteLine("{0}", usingStatement.Name);
                }
                public override void Visit(NamespaceDeclaration namespaceDeclaration)
                {
                    foreach (FileReference usingStatement in namespaceDeclaration.UsingStatements)
                    {
                        usingStatement.Accept(this);
                    }
                    _writer.Tab(); _writer.WriteLine("namespace {0}", namespaceDeclaration.Name);
                    _writer.Tab(); _writer.WriteLine('{');
                    _writer.PushTab();
                    base.Visit(namespaceDeclaration);
                    _writer.PopTab();
                    _writer.Tab(); _writer.WriteLine('}');
                }
                public override void Visit(ClassDeclaration classDeclaration)
                {
                    _writer.Tab();
                    if (classDeclaration.Modifiers.Length > 0)
                    {
                        _writer.Write(classDeclaration.Modifiers);
                        _writer.Write(" ");
                    }
                    _writer.WriteLine("class {0}", classDeclaration.Name);
                    _writer.Tab(); _writer.WriteLine('{');
                    _writer.PushTab();
                    base.Visit(classDeclaration);
                    _writer.PopTab();
                    _writer.Tab(); _writer.WriteLine('}');
                }
                public override void Visit(MemberDeclaration memberDeclaration)
                {
                    switch (memberDeclaration.Style)
                    {
                        default:
                            _writer.Tab();
                            if (memberDeclaration.Modifiers.Length > 0)
                                _writer.Write("{0} ", memberDeclaration.Modifiers);
                            _writer.WriteLine("{0} {1};", memberDeclaration.Type, memberDeclaration.Name);
                            break;
                    }
                }
                public override void Visit(MethodDeclaration methodDeclaration)
                {
                    switch (methodDeclaration.Style)
                    {
                        default:
                            _writer.Tab();
                            if (methodDeclaration.Modifiers.Length > 0)
                                _writer.Write("{0} ", methodDeclaration.Modifiers);
                            _writer.WriteLine("{0} {1} ({2})", methodDeclaration.Type, methodDeclaration.Name, methodDeclaration.Arguments);
                            _writer.Tab(); _writer.WriteLine('{');
                            _writer.PushTab();
                            base.Visit(methodDeclaration);
                            _writer.PopTab();
                            _writer.Tab(); _writer.WriteLine('}');
                            break;
                    }
                }
            }
            static Writer()
            {

            }
            public Writer(StreamWriter streamWriter)
                : base(streamWriter)
            {

            }
        }

    }
}
