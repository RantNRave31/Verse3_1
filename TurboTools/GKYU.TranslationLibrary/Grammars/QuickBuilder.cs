using System;
using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.TranslationLibrary.Symbols;
    using GKYU.TranslationLibrary.Translators;
    using GKYU.TranslationLibrary;
    using GKYU.StateMachinesLibrary;
    using System.Linq;

    public partial class Syntax
    {
        public class QuickBuilder
            : Builder
            , IBuildSyntax
        {
            public class Parser<T>
                : ParserBase<Token, Syntax.SyntaxNode>
                , IReadSymbols<Syntax.SyntaxNode>
            {
                QuickBuilder _builder;
                static Type _startSymbol = typeof(T);
                private SimpleOneTimeGate _generateDomainClassGate = new SimpleOneTimeGate(false);
                public bool GenerateDomainClass
                {
                    get
                    {
                        return _generateDomainClassGate.Closed;
                    }
                    set
                    {
                        _generateDomainClassGate.Closed = value;
                    }
                }
                public bool GenerateDefaultConstructor { get; set; }
                public bool GenerateCopyConstructor { get; set; }
                public Parser(QuickBuilder builder, IEnumerable<string> inputList)
                    : base(new EnumerableLexer<string, Token>(inputList))
                {
                    _builder = builder;
                    GenerateDomainClass = false;
                }
                protected override Syntax.SyntaxNode Next()
                {
                    Syntax.SyntaxNode result = null;
                    string domainClassName = string.Empty;
                    // TODO: add symbol lookup from here to symbol table for more info, then decorate class correctly
                    // NOTE: any thing build in symbol table, does not have to be passed.  it's faster, duh
                    if (!EndOfInput)
                        switch (_startSymbol)
                        {
                            case var cls when cls == typeof(FileDeclaration):
                                {
                                    result = FileDeclaration(CurrentInput.Value);
                                    break;
                                }
                            case var cls when cls == typeof(FileReference):
                                {
                                    result = UsingStatement(CurrentInput.Value);
                                    break;
                                }
                            case var cls when cls == typeof(NamespaceDeclaration):
                                {
                                    result = NamespaceDeclaration(CurrentInput.Value);
                                    break;
                                }
                            case var cls when cls == typeof(ClassDeclaration):
                                {
                                    result = ClassDeclaration("public", CurrentInput.Value);
                                    break;
                                }
                            case var cls when cls == typeof(MemberDeclaration):
                                {
                                    result = MemberDeclaration(CurrentInput.Value);
                                    break;
                                }
                            default:
                                throw new System.Exception("ERROR:  Start Symbol case is not switched in Parser.Next()");
                        }

                    else
                    {
                        EndOfFile = true;
                    }
                    return result;
                }
                protected FileDeclaration FileDeclaration(string fileName)
                {
                    FileDeclaration result = _builder.FileDeclaration(fileName);
                    NextInput();
                    if (!EndOfInput)
                    {
                        result.Namespaces = new NamespaceDeclaration[] { NamespaceDeclaration(CurrentInput.Value) };
                    }
                    return result;
                }
                protected FileReference UsingStatement(string usingStatement)
                {
                    FileReference result = _builder.UsingStatement(usingStatement);
                    NextInput();
                    if (!EndOfInput)
                    {
                        result = _builder.UsingStatement(usingStatement);
                    }
                    return result;
                }
                protected NamespaceDeclaration NamespaceDeclaration(string namespaceName)
                {
                    _builder._symbolTable.EnterNamespace(namespaceName);
                    NamespaceDeclaration result = _builder.NamespaceDeclaration(namespaceName);
                    List<ClassDeclaration> classDeclarations = new List<ClassDeclaration>();
                    NextInput();
                    while (!EndOfInput)
                    {
                        classDeclarations.Add(ClassDeclaration("public", CurrentInput.Value));
                    }
                    result.Classes = classDeclarations.ToArray();
                    _builder._symbolTable.ExitNamespace();
                    return result;
                }
                protected ClassDeclaration ClassDeclaration(string modifiers, string className)
                {
                    ClassDeclaration result = _builder.ClassDeclaration(modifiers, className);
                    List<ClassDeclaration> classDeclarations = new List<ClassDeclaration>();
                    NextInput();
                    if (_generateDomainClassGate.Enter() && !EndOfInput)
                    {
                        while (_generateDomainClassGate.Closed && !EndOfInput)
                        {
                            classDeclarations.Add(ClassDeclaration(modifiers, CurrentInput.Value));
                        }
                        _generateDomainClassGate.Exit();
                        result.Classes = classDeclarations.ToArray();
                    }
                    return result;
                }
                protected MemberDeclaration MemberDeclaration(string className)
                {
                    NextInput();
                    MemberDeclaration result = _builder.MemberDeclaration("public", CurrentInput.Value, className);
                    NextInput();
                    return result;
                }
            }
            protected SymbolTable _symbolTable;
            public Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> EntityMap { get; set; }
            public QuickBuilder(SymbolTable symbolTable, Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap = null)
                : base()
            {
                _symbolTable = symbolTable;
                if (null == entityMap)
                    EntityMap = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
                else
                    EntityMap = entityMap;
            }
            public override FileReference UsingStatement(string name)
            {
                return _symbolTable.UsingStatement(name);
            }
            public override FileDeclaration FileDeclaration(string name, string[] usingStatements = null, string[] namespaceNames = null, string style = null)
            {
                FileDeclaration result = _symbolTable.FileDeclaration(name, style);
                result.FileReferences = ParseList<FileReference>(usingStatements);
                result.Namespaces = ParseList<NamespaceDeclaration>(namespaceNames);
                return result;
            }
            public override NamespaceDeclaration NamespaceDeclaration(string name, string[] usingStatements = null, string[] classNames = null, string style = null)
            {
                NamespaceDeclaration result = _symbolTable.NamespaceDeclaration(name, style);
                result.UsingStatements = ParseList<FileReference>(usingStatements);
                result.Classes = ParseList<ClassDeclaration>(classNames);
                return result;
            }
            public override ClassDeclaration ClassDeclaration(string modifiers, string name, string[] classNames = null, string style = null)
            {
                ClassDeclaration result = _symbolTable.ClassDeclaration(modifiers, name, style);
                result.Classes = ParseList<ClassDeclaration>(classNames);
                string[] members = EntityMap.MemberPairVector(_symbolTable.Namespace, name);
                result.Members = ParseList<MemberDeclaration>(members);
                return result;
            }
            public override MemberDeclaration MemberDeclaration(string modifiers, string type, string name, string style = null)
            {
                MemberDeclaration result = _symbolTable.MemberDeclaration(modifiers, type, name, style);
                return result;
            }
            public override MethodDeclaration MethodDeclaration(string modifiers, string type, string name, string style = null)
            {
                MethodDeclaration result = _symbolTable.MethodDeclaration(modifiers, type, name, style);
                return result;
            }
            public override LexerDeclaration LexerDeclaration(string modifiers, string type, string name, string style = null)
            {
                LexerDeclaration result = _symbolTable.CreateLexerDeclaration(modifiers, type, name, style);
                return result;
            }
            public override ParserDeclaration ParserDeclaration(string modifiers, string type, string name, string style = null)
            {
                ParserDeclaration result = _symbolTable.CreateParserDeclaration(modifiers, type, name, style);
                return result;
            }
            public static Syntax.SyntaxNode Build<T>(
                 Syntax.SymbolTable symbolTable, 
                 string modifiers, 
                 string type, 
                 string name, 
                 string style = null
                )
            {
                QuickBuilder builder = new QuickBuilder(symbolTable);
                if (typeof(T) == typeof(Syntax.MemberDeclaration))
                    return builder.MemberDeclaration(modifiers, type, name, style);
                if (typeof(T) == typeof(Syntax.MethodDeclaration))
                    return builder.MethodDeclaration(modifiers, type, name, style);
                if (typeof(T) == typeof(Syntax.LexerDeclaration))
                    return builder.LexerDeclaration(modifiers, type, name, style);
                if (typeof(T) == typeof(Syntax.ParserDeclaration))
                    return builder.ParserDeclaration(modifiers, type, name, style);
                else
                    return null;
            }
            public static Syntax.SyntaxNode Build<T>(
                Syntax.SymbolTable symbolTable,
                string namespaceName,
                string[] usingStatements,
                IEnumerable<string> classNames,
                bool generateDomainClass,
                Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> entityMap = null
                )
            {
                QuickBuilder builder = new QuickBuilder(symbolTable, entityMap);
                Parser<T> parser = new Parser<T>(builder, classNames);
                parser.GenerateDomainClass = generateDomainClass;
                return (Syntax.SyntaxNode)parser.Read();
            }
            protected T[] ParseList<T>(string[] values)
                where T : Syntax.SyntaxNode
            {
                Parser<T> parser = new Parser<T>(this, values);
                List<object> list = new List<object>();
                foreach (Syntax.SyntaxNode element in parser)
                {
                    list.Add(element);
                }
                return list.OfType<T>().ToArray();
            }
        }

    }
}
