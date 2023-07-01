using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GKYU.CoreLibrary;
using GKYU.CollectionsLibrary.Collections;
using GKYU.CollectionsLibrary.Collections.Graphs;
using GKYU.TranslationLibrary.Domains.Texts;
using GKYU.TranslationLibrary.Grammars;
using GKYU.TranslationLibrary.Symbols;
using GKYU.TranslationLibrary.Translators;
using static GKYU.TranslationLibrary.Grammars.Syntax;
using GKYU.CoreLibrary.Attributes;

namespace GKYU.TranslationLibrary
{
    public interface IMacroInterface
    {
        MacroProcessor.Macro this[string key] { get; set; }
        void Add(MacroProcessor.Macro value);
        bool ContainsKey(string key);
        string Parse(string unexpandedText, bool enableExpansion = false);
        void Remove(string name);
        void Translate(string targetFileName, string sourceFileName, bool enableExpansion = false);
    }
    public partial class MacroProcessor
        : IMacroInterface
    {
        public class Macro
            : Symbol.Named
        {
            protected object _object;
            public virtual object Object { get { return _object; } set { _object = value; } }
            public Type ValueType
            {
                get
                {
                    return Value?.GetType();
                }
            }
            public Macro(string name, object value)
            {
                _name = name;
                _object = value;
            }
            public override string ToString()
            {
                return _object.ToString();
            }
        }
        public class MacroExpressionEvaluator
            : Visitor
        {
            StringBuilder result;
            public MacroExpressionEvaluator(StringBuilder stringBuilder)
            {
                result = stringBuilder;
            }
            protected string EvaluateMacroReference(Syntax.Reference macroReference)
            {
                StringBuilder result = new StringBuilder();
                if (null == macroReference._reference)
                    result.Append("!!!" + macroReference.Identifier.Name + " NOF!!!");
                else if (macroReference.Index != null)
                {
                    Macro macro = (Macro)macroReference._reference;
                    Array array = (Array)macro.Object;
                    int index = (int)macroReference.Index;
                    var myValue = array.GetValue(index);
                    if (myValue == null)
                        result.Append("{NULL}");
                    else
                        result.Append(myValue.ToString());
                }
                else
                    result.Append(macroReference._reference.ToString());
                return result.ToString();
            }
            public override void Visit(Empty symbol)
            {
            }
            public override void Visit(Syntax.String symbol)
            {
                result.Append(symbol.Value);
            }
            public override void Visit(Reference symbol)
            {
                result.Append(EvaluateMacroReference(symbol));
            }
            public override void Visit(Primative symbol)
            {
                symbol.Data.Accept(this);
            }
            public override void Visit(Choice symbol)
            {
                symbol.thatOne.Accept(this);
                symbol.thatOne.Accept(this);
            }
            public override void Visit(Repitition symbol)
            {
                symbol.expression.Accept(this);
            }
            public override void Visit(Sequence symbol)
            {
                symbol.first.Accept(this);
                symbol.second.Accept(this);
            }
            public override void Visit(MacroExpression symbol)
            {
                symbol.expression.Accept(this);
            }
        }
        public class Macro<T>
            : Macro
        {
            public Macro(string name, object value)
                : base(name, value)
            {

            }
            public static implicit operator T(Macro<T> p) => (T)p.Object;
        }
        public class Macros
            : IEnumerable<Macro>
        {
            protected readonly Dictionary<string,Macro> _macros;
            public Macro this[string name]
            {
                get { return _macros[name]; }
                set { _macros[name] = value; }
            }
            public Macros()
            {
                _macros = new Dictionary<string, Macro>();
            }
            public Macros(Macros macros)
                : this()
            {
                foreach (Macro macro in macros)
                {
                    Add(macro);
                }
            }
            public bool ContainsKey(string key)
            {
                return _macros.ContainsKey(key);
            }
            public void Add(Macro parameter)
            {
                _macros.Add(parameter.Name, parameter);
            }
            public void Remove(string name)
            {
                _macros.Remove(name);
            }
            public IEnumerator<Macro> GetEnumerator()
            {
                return _macros.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
        public class Lexer
            : LexerBase<Token>
            , IReadSymbols<Token>
        {
            public enum TOKEN : int
            {
                EOF = -1,
                UNDEFINED = 0,
                EOL,
                IDENTIFIER,
                NUMBER,
                SYMBOL,
                WHITESPACE,
            }
            public enum STATE : int
            {
                INITIAL,
                UNIDENTIFIED,
                MATCH,
                END_OF_LINE,
                IDENTIFIER,
                NUMBER,
                SYMBOL,
                WHITESPACE,
                END_OF_FILE,
                FINAL,
            }
            private readonly StreamCharacterScanner _scanner;

            public Lexer(StreamCharacterScanner scanner)
                : base()
            {
                _scanner = scanner;
            }
            public override void Skip(int count)
            {
                throw new NotImplementedException();
            }

            public override void Skip2(int kind)
            {
                throw new NotImplementedException();
            }

            public override void Skip2(int[] kind)
            {
                throw new NotImplementedException();
            }
            private Stack<int> _parenthesis = new Stack<int>();
            protected override Token Next()
            {
                Token result = new Token() { Kind = (int)TOKEN.UNDEFINED };
                switch((int)STATE.INITIAL)
                {
                    case (int)STATE.INITIAL:
                        if (_scanner.Peek() == -1)
                            goto case (int)STATE.END_OF_FILE;
                        else if (_scanner.Peek().In(new int[] { '\r', '\n' }))
                            goto case (int)STATE.END_OF_LINE;
                        else if (_scanner.Peek().Between('a', 'z') || _scanner.Peek().Between('A', 'Z'))
                            goto case (int)STATE.IDENTIFIER;
                        else if (_scanner.Peek().Between('0', '9'))
                            goto case (int)STATE.NUMBER;
                        else if (_scanner.Peek().In(new int[] { '.', ',', '?', ';', ':', '<', '>', '(', ')', '[', ']', '{', '}', '"', '\'', '!', '@', '#', '$', '%', '^', '&', '*', '_', '-', '+' }))
                            goto case (int)STATE.SYMBOL;
                        else if (_scanner.Peek().In(new int[] { ' ', '\t' }))
                            goto case (int)STATE.WHITESPACE;
                        else
                            goto case (int)STATE.UNIDENTIFIED;
                    case (int)STATE.UNIDENTIFIED:
                        result.Kind = (int)TOKEN.UNDEFINED;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        goto case (int)STATE.FINAL;
                    case (int)STATE.MATCH:
                        goto case (int)STATE.FINAL;
                    case (int)STATE.END_OF_LINE:
                        result.Kind = (int)TOKEN.EOL;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        if (_scanner.Peek().In(new int[] { '\r', '\n' }))
                            goto case (int)STATE.END_OF_LINE;
                        goto case (int)STATE.MATCH;
                    case (int)STATE.IDENTIFIER:
                        result.Kind = (int)TOKEN.IDENTIFIER;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        if (_scanner.Peek().Between('a', 'z') || _scanner.Peek().Between('A', 'Z'))
                            goto case (int)STATE.IDENTIFIER;
                        if (_scanner.Peek().Between('0', '9'))
                            goto case (int)STATE.IDENTIFIER;
                        goto case (int)STATE.MATCH;
                    case (int)STATE.NUMBER:
                        result.Kind = (int)TOKEN.NUMBER;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        if (_scanner.Peek().Between('0', '9'))
                            goto case (int)STATE.NUMBER;
                        goto case (int)STATE.MATCH;
                    case (int)STATE.SYMBOL:
                        result.Kind = (int)TOKEN.SYMBOL;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        goto case (int)STATE.MATCH;
                    case (int)STATE.WHITESPACE:
                        result.Kind = (int)TOKEN.WHITESPACE;
                        result.Value += (char)_scanner.Peek();
                        NextInput();
                        goto case (int)STATE.MATCH;
                    case (int)STATE.END_OF_FILE:
                        result.Kind = (int)TOKEN.EOF;
                        this.EndOfInput = true;
                        this.EndOfFile = true;
                        goto case (int)STATE.FINAL;
                    case (int)STATE.FINAL:
                        break;
                }
                return result;
            }

            protected override bool NextInput()
            {
                _scanner.Read();
                return !_scanner.EndOfFile;
            }
        }
        public class Parser
            : ParserBase<Token, SyntaxNode>
        {
            internal enum STATE : int
            {
                INITIAL,
                SYMBOL,
                MACRO,
                TEXT,
                END_OF_LINE,
                END_OF_FILE,
                FINAL
            }
            protected readonly Syntax.SymbolTable _symbolTable;
            protected readonly MacroProcessor _macros;
            protected readonly CharacterSet characterSet;
            protected readonly Dictionary<Type, HashSet<Lexer.TOKEN>> _beginsWith = new Dictionary<Type, HashSet<Lexer.TOKEN>>();
            public Parser(MacroProcessor macros, Lexer lexer)
                : base(lexer)
            {
                _symbolTable = new Syntax.SymbolTable();
                _macros = macros;
                Parameters.Add(new Parameter<bool>("EnableExpansion", false));
                _beginsWith.Add(typeof(Reference), new HashSet<Lexer.TOKEN>(){ Lexer.TOKEN.IDENTIFIER});
            }
            protected bool BeginsWith(Type t, Lexer.TOKEN kind)
            {
                if (_beginsWith.ContainsKey(t))
                {
                    return _beginsWith[t].Contains(kind);
                }
                else
                    return false;
            }
            protected SyntaxNode EOF()
            {
                SyntaxNode result = _symbolTable.Create<EndOfFile>();
                EndOfInput = true;
                base.EndOfFile = true;
                result.Kind = (int)SYMBOL.EndOfFile;
                result.Value = "{EOF}";
                return result;
            }
            protected SyntaxNode EOL()
            {
                SyntaxNode result = _symbolTable.Create<EndOfLine>();
                EndOfInput = true;
                base.EndOfFile = true;
                result.Kind = (int)SYMBOL.EndOfLine;
                result.Value = CurrentInput.Value;
                NextInput();
                return result;
            }
            protected Identifier Identifier()
            {
                string identifierName = string.Empty;
                Token token;
                 while (CurrentIs((int)Lexer.TOKEN.IDENTIFIER))
                {
                    token = CurrentInput;
                    Expect((int)Lexer.TOKEN.IDENTIFIER, "EXPECTS IDENTIFIER");
                    identifierName += token.Value;
                    if(Accept((int)Lexer.TOKEN.SYMBOL, "."))
                        identifierName += ".";
                }
                Syntax.Identifier identifier;
                if (_symbolTable.Identifiers.ContainsKey(identifierName))
                    identifier = _symbolTable.Identifiers[identifierName];
                else
                    identifier = _symbolTable.CreateIdentifier(identifierName);
                identifier.Kind = (int)SYMBOL.Identifier;
                identifier.Value = identifierName;
                return identifier;
            }
            protected Number Number()
            {
                Token token = CurrentInput;
                Expect((int)Lexer.TOKEN.NUMBER, "EXPECTS NUMBER");
                if (token.Value.Contains('.'))
                {
                    Syntax.Decimal result = _symbolTable.Create<Syntax.Decimal>();
                    result.Kind = (int)SYMBOL.Decimal;
                    result.Name = string.Format("DECIMAL({0})", result.Value);
                    result.Value = token.Value;
                    return result;
                }
                else
                {
                    Syntax.Integer result = _symbolTable.Create<Syntax.Integer>();
                    result.Kind = (int)SYMBOL.Integer;
                    result.Name = string.Format("INTEGER({0})", result.Value);
                    result.Value = token.Value;
                    return result;
                }
            }
            protected Integer Indexer()
            {
                Expect((int)Lexer.TOKEN.SYMBOL, "[", "Expect Indexer BEGINS with '['");
                Integer number = (Integer)Number();
                Expect((int)Lexer.TOKEN.SYMBOL, "]", "Expect Indexer ENDS with ']'");
                return number;
            }
            protected SyntaxNode Symbolic()
            {
                SyntaxNode result = _symbolTable.Create<Syntax.Symbolic>();
                result.Kind = (int)SYMBOL.Symbolic;
                result.Value = CurrentInput.Value;
                NextInput();
                return result;
            }
            protected SyntaxNode String()
            {
                SyntaxNode result = _symbolTable.Create<Syntax.String>();
                result.Kind = (int)SYMBOL.String;
                result.Value = CurrentInput.Value;
                NextInput();
                return result;
            }
            protected Reference MacroReference()
            {
                Syntax.Identifier identifier = Identifier();
                Syntax.Integer index = null;
                if (CurrentIs((int)Lexer.TOKEN.SYMBOL, "["))
                {
                    index = Indexer();
                }
                Syntax.Reference macroReference = _symbolTable.CreateReference(string.Empty, "Macro", identifier.Name);
                macroReference.Kind = (int)SYMBOL.Reference;
                macroReference.Identifier = identifier;
                macroReference.Index = index;
                if (_macros.ContainsKey(identifier.Name))
                    macroReference._reference = _macros[identifier.Name];
                return macroReference;
            }
            private ExpressionNode Atom()
            {
                if (CurrentIs((int)Lexer.TOKEN.EOF))
                    return new Syntax.Empty() { Kind = (int)SYMBOL.Empty };
                if (CurrentIs((int)Lexer.TOKEN.SYMBOL, "("))
                {
                    Expect((int)Lexer.TOKEN.SYMBOL, "(", "Expect Expression BEGINS with '('");
                    ExpressionNode r = Expression();
                    Expect((int)Lexer.TOKEN.SYMBOL, ")", "Expect Expression ENDS with ')'");
                    return r;
                }
                else if (CurrentIs((int)Lexer.TOKEN.SYMBOL, ","))
                {
                    Syntax.String s = _symbolTable.Create<Syntax.String>();
                    s.Value = ",";
                    NextInput();
                    Syntax.Primative primative = _symbolTable.MakePrimative(s);
                    return primative;
                }
                else if (CurrentIs((int)Lexer.TOKEN.IDENTIFIER))
                {
                    SyntaxNode macro = MacroReference();
                    Syntax.Primative primative = _symbolTable.MakePrimative(macro);
                    return primative;
                }
                else
                    return new Syntax.Empty() { Kind = (int)SYMBOL.Empty };
            }
            private ExpressionNode Factor()
            {
                ExpressionNode atom = this.Atom();
                while (!CurrentIs((int)Syntax.SYMBOL.EndOfFile) && CurrentIs((int)Lexer.TOKEN.SYMBOL, "*"))
                {
                    Expect((int)Lexer.TOKEN.SYMBOL, "*", "Expects Repetition BEGINS with '*'");
                    atom = _symbolTable.MakeRepitition(atom);
                    NextInput();
                }
                return atom;
            }
            private ExpressionNode Term()
            {
                ExpressionNode factor = new Syntax.Empty();
                while (!CurrentIs((int)Syntax.SYMBOL.EndOfFile) && !CurrentIs((int)Lexer.TOKEN.SYMBOL,")") && !CurrentIs((int)Lexer.TOKEN.SYMBOL, "|"))
                {
                    ExpressionNode nextFactor = this.Factor();
                    if (nextFactor.Kind == (int)SYMBOL.Empty)
                        break;
                    factor = _symbolTable.MakeSequence(factor, nextFactor);
                }
                return factor;
            }
            private ExpressionNode Expression()
            {
                ExpressionNode term = this.Term();
                if (!CurrentIs((int)Syntax.SYMBOL.EndOfFile) && CurrentIs((int)Lexer.TOKEN.SYMBOL,"|"))
                {
                    Expect((int)Lexer.TOKEN.SYMBOL, "|","Expects Choice to Begin with '|'");
                    ExpressionNode regex = (ExpressionNode)Expression();
                    Syntax.Choice choice = _symbolTable.MakeChoice(term, regex);
                    return choice;
                }
                else
                    return term;
            }
            protected MacroExpression MacroExpression()
            {
                Expect((int)Lexer.TOKEN.SYMBOL, "$", "Expect Macro BEGINS with '$('");
                ExpressionNode expression = Expression();
                string macroDeclarationName = string.Format("MacroDeclaration{0}", _symbolTable[(int)SYMBOL.MacroExpression].Count++);
                MacroExpression macroExpression = _symbolTable.CreateMacroDeclaration(macroDeclarationName);
                macroExpression.expression = expression;
                return macroExpression;
            }

            protected override SyntaxNode Next()
            {
                switch(STATE.INITIAL)
                {
                    case STATE.INITIAL:
                        if (CurrentInput.Kind == (int)Lexer.TOKEN.EOF)
                            goto case STATE.END_OF_FILE;
                        if (CurrentInput.Kind == (int)Lexer.TOKEN.EOL)
                            goto case STATE.END_OF_LINE;
                        if (CurrentInput.Kind == (int)Lexer.TOKEN.SYMBOL)
                            goto case STATE.SYMBOL;
                        goto case STATE.TEXT;
                    case STATE.SYMBOL:
                        if (CurrentIs((int)Lexer.TOKEN.SYMBOL, "$") && NextIs((int)Lexer.TOKEN.SYMBOL, "("))
                            return MacroExpression();
                        else
                            return Symbolic();
                    case STATE.TEXT:
                        return String();
                    case STATE.END_OF_LINE:
                        return EOL();
                    case STATE.END_OF_FILE:
                        return EOF();
                }
            }
        }
        private Stack<Macros> _contextStack = new Stack<Macros>();
        public MacroProcessor()
        {
            _contextStack.Push(new Macros());
        }
        public IEnumerable<Macro> GetMacros()
        {
            foreach(Macro parameter in _contextStack.Peek())
            {
                yield return parameter;
            }
            yield break;
        }
        public Macro this[string key]
        {
            get
            {
                if (!_contextStack.Peek().ContainsKey(key))
                    return new Macro(key, string.Format("$({0} MACRO NOT FOUND)", key));
                return _contextStack.Peek()[key];
            }
            set
            {
                if (!_contextStack.Peek().ContainsKey(key))
                    _contextStack.Peek().Add(value);
                if (value == _contextStack.Peek()[key])
                    return;
                _contextStack.Peek()[key] = value;
            }
        }
        public bool ContainsKey(string key)
        {
            return _contextStack.Peek().ContainsKey(key);
        }
        protected string ParsePass(string inputText, bool enableExpansion)
        {
            StringBuilder result = new StringBuilder();
            Parser parser = new Parser(this, new Lexer(new StreamCharacterScanner(new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(inputText))))));
            parser.Parameters["EnableExpansion"].Value = enableExpansion;
            foreach(SyntaxNode element in parser)
            {
                switch(element.Kind)
                {
                    case (int)SYMBOL.EndOfFile:
                        break;
                    case (int)SYMBOL.MacroExpression:
                        MacroExpression macroExpression = (MacroExpression)element;
                        MacroExpressionEvaluator evaluater = new MacroExpressionEvaluator(result);
                        macroExpression.Accept(evaluater);
                        break;
                    default:
                        result.Append(element.Value);
                        break;
                }
            }
            return result.ToString();
        }
        public string Parse(string unexpandedText, bool enableExpansion = false)
        {
            string expandedText = ParsePass(unexpandedText, enableExpansion);
            if(enableExpansion)
            {
                expandedText = ParsePass(expandedText, enableExpansion);
            }
            return expandedText;
        }
        public void Translate(string targetFileName, string sourceFileName, bool enableExpansion = false)
        {
            using(StreamReader sr = new StreamReader(sourceFileName))
            {
                using (StreamWriter sw = new StreamWriter(targetFileName))
                {
                    string inputLine = null;
                    while (null != (inputLine = sr.ReadLine()))
                    {
                        string expandedText = Parse(inputLine);
                        sw.WriteLine(expandedText);
                    }
                }
            }
        }
        public void Add(MacroProcessor.Macro macro)
        {
            this._contextStack.Peek().Add(macro);
        }
        public void Add<T>(string name, T source)
        {
            HashSet<Type> _integralTypes = new HashSet<Type>()
            {
                typeof(byte),
                typeof(char),
                typeof(bool),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(UInt16),
                typeof(UInt32),
                typeof(UInt64),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(byte),
                typeof(DateTime),
                typeof(string),
            };
            if (typeof(T).IsArray)
            {
                Macro<T> macro = new MacroProcessor.Macro<T>(name, source);
                Add(macro);
            }
            else if (_integralTypes.Contains(typeof(T)))
            {
                Macro<T> macro = new MacroProcessor.Macro<T>(name, source);
                Add(macro);
            }
            else
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                string propertyName = propertyInfo.Name;
                if (_integralTypes.Contains(propertyInfo.PropertyType))
                {
                    object o = (propertyInfo.GetValue(source));
                    Macro<T> macro = new MacroProcessor.Macro<T>(name + "." + propertyName, (o == null) ? default(T) : o);
                    Add(macro);
                }
                else if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                        object o = (propertyInfo.GetValue(source));
                        Macro<T> macro = new MacroProcessor.Macro<T>(name + "." + propertyName, o);
                        Add(macro);
                        //IEnumerable items = (IEnumerable)propertyInfo.GetValue(source);
                        //int nItems = 0;
                        //foreach (object item in items)
                        //{
                        //    Macro<T> macro = new MacroProcessor.Macro<T>(name + "." + propertyName + "[" + nItems.ToString() + "]", item);
                        //    Add(macro);
                        //    nItems++;
                        //}
                    }
            }
        }
        public void Remove(string name)
        {
            this._contextStack.Peek().Remove(name);
        }
        public void PushContext()
        {
            _contextStack.Push(new Macros(_contextStack.Peek()));
        }
        public void PopContext()
        {
            _contextStack.Pop();
        }
        public bool Expect(string key, string errorMessage)
        {
            if (this.ContainsKey(key))
                return true;
            throw new System.Exception(string.Format("EXPECT The Macro $({0}) to exist.", key));
        }
        public bool Expect(string key, object value, string errorMessage)
        {
            if (this.ContainsKey(key) && this[key].Object == value)
                return true;
            throw new System.Exception(string.Format("EXPECT The Macro $({0}) to exist and it's value equal to:  {0}", key, value));
        }
        public bool Expect(string key, object from, object to, string errorMessage)
        {
            if (this.ContainsKey(key) && this[key].Object == from)
                return true;
            throw new System.Exception(string.Format("EXPECT The Macro $({0}) to exist and it's value from:  '{1}' to '{2}'", key, from, to));
        }
    }
}
