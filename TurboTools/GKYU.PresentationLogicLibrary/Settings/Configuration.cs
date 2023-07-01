using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GKYU.PresentationCoreLibrary.ViewModels;
using GKYU.PresentationLogicLibrary.ViewModels;

namespace GKYU.PresentationLogicLibrary.Settings
{
    public class Configuration
        : ViewModelBase
    {
        public class Setting
            : Configuration
        {
            private int _sortPriority;
            public int SortPriority
            {
                get
                {
                    return _sortPriority;
                }
                set
                {
                    if (value == _sortPriority)
                        return;
                    _sortPriority = value;
                    OnPropertyChanged("SortPriority");
                }
            }

            private string _category;
            public string Category
            {
                get
                {
                    return _category;
                }
                set
                {
                    if (value == _category)
                        return;
                    _category = value;
                    OnPropertyChanged("Category");
                }
            }

            private string _name;
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    if (value == _name)
                        return;
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
            private string _value;
            public string Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (value == _value)
                        return;
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        public class Section
            : Configuration
        {
            public string Name;
            public Dictionary<string, Setting> Settings;
        }
        public class Catalog
            : Configuration
        {
            public Dictionary<string, Section> Sections = new Dictionary<string, Section>();
        }
        public class Scanner
        {
            protected enum STATE : int
            {
                INITIAL,
                MATCH,
                UNRECOGNIZED,
                EOL,
                COMMENT,
                WHITESPACE,
                NUMBER,
                IDENTIFIER,
                EQUALS,
                VALUE,
                LEFT_BRACKET,
                RIGHT_BRACKET,
                EOF,
            }
            public enum TOKEN : int
            {
                EOF = -1,
                UNRECOGNIZED = 0,
                EOL,
                COMMENT, // '#' Printable
                WHITESPACE,
                NUMBER,
                IDENTIFIER,
                VALUE,
                LEFT_BRACKET,
                RIGHT_BRACKET,
            }
            public class Token
            {
                public TOKEN Kind = TOKEN.UNRECOGNIZED;
                public int Position = 0;
                public string Value = string.Empty;
            }

            private static int _lookAhead = 2;
            private StreamReader _inputStream;
            private Queue<Token> _outputQueue;
            private bool bDone;
            public Scanner(StreamReader inputStream)
            {
                _inputStream = inputStream;
                _outputQueue = new Queue<Token>();
            }
            private Token NextToken()
            {
                STATE state = 0;
                Token token = null;
                switch(state)
                {
                    case STATE.INITIAL:
                        token = new Token();
                        if (_inputStream.Peek() == -1)
                            goto case STATE.EOF;
                        if ((_inputStream.Peek() == '\r') || (_inputStream.Peek() == '\n'))
                            goto case STATE.EOL;
                        if (_inputStream.Peek() == '#')
                            goto case STATE.COMMENT;
                        if ((_inputStream.Peek() == ' ') || (_inputStream.Peek() == '\t'))
                            goto case STATE.WHITESPACE;
                        if ((_inputStream.Peek() >= '0') && (_inputStream.Peek() <= '9'))
                            goto case STATE.NUMBER;
                        if (((_inputStream.Peek() >= 'a') && (_inputStream.Peek() <= 'z')) || ((_inputStream.Peek() >= 'A') && (_inputStream.Peek() <= 'Z')))
                            goto case STATE.IDENTIFIER;
                        if (_inputStream.Peek() == '=')
                            goto case STATE.EQUALS;
                        if (_inputStream.Peek() == '[')
                            goto case STATE.LEFT_BRACKET;
                        if (_inputStream.Peek() == ']')
                            goto case STATE.RIGHT_BRACKET;
                        goto case STATE.UNRECOGNIZED;
                    case STATE.MATCH:
                        return token;
                    case STATE.UNRECOGNIZED:
                        token.Value += (char)_inputStream.Read();
                        return token;
                    case STATE.EOL:
                        token.Kind = TOKEN.EOL;
                        token.Value += (char)_inputStream.Read();
                        if ((_inputStream.Peek() == '\r') && (_inputStream.Peek() == '\n'))
                        {
                            token.Value += (char)_inputStream.Read();
                        }
                        goto case STATE.INITIAL;// Skip End of Lines
                    case STATE.COMMENT:
                        token.Kind = TOKEN.COMMENT;
                        token.Value += (char)_inputStream.Read();
                        if ((_inputStream.Peek() >= 32) && (_inputStream.Peek() <= 127))
                            goto case STATE.COMMENT;
                        goto case STATE.MATCH;
                    case STATE.WHITESPACE:
                        token.Kind = TOKEN.WHITESPACE;
                        token.Value += (char)_inputStream.Read();
                        if ((_inputStream.Peek() == ' ') || (_inputStream.Peek() == '\t') || (_inputStream.Peek() == '\r') || (_inputStream.Peek() == '\n'))
                            goto case STATE.WHITESPACE;
                        goto case STATE.INITIAL;// Skip Whitespace
                    case STATE.NUMBER:
                        token.Kind = TOKEN.NUMBER;
                        token.Value += (char)_inputStream.Read();
                        if ((_inputStream.Peek() >= '0') && (_inputStream.Peek() <= '9'))
                            goto case STATE.NUMBER;
                        goto case STATE.MATCH;
                    case STATE.IDENTIFIER:
                        token.Kind = TOKEN.IDENTIFIER;
                        token.Value += (char)_inputStream.Read();
                        if (((_inputStream.Peek() >= 'a') && (_inputStream.Peek() <= 'z'))
                            || ((_inputStream.Peek() >= 'A') && (_inputStream.Peek() <= 'Z'))
                            || ((_inputStream.Peek() >= '0') && (_inputStream.Peek() <= '9'))
                            || (_inputStream.Peek() == '_')
                            )
                            goto case STATE.IDENTIFIER;
                        goto case STATE.MATCH;
                    case STATE.EQUALS:
                        token.Kind = TOKEN.VALUE;
                        _inputStream.Read();// skip over equals
                        if ((_inputStream.Peek() >= 32) && (_inputStream.Peek() <= 127))
                            goto case STATE.VALUE;
                        goto case STATE.MATCH;
                    case STATE.VALUE:
                        token.Value += (char)_inputStream.Read();
                        if ((_inputStream.Peek() >= 32) && (_inputStream.Peek() <= 127))
                            goto case STATE.VALUE;
                        goto case STATE.MATCH;
                    case STATE.LEFT_BRACKET:
                        token.Kind = TOKEN.LEFT_BRACKET;
                        token.Value += (char)_inputStream.Read();
                        goto case STATE.MATCH;
                    case STATE.RIGHT_BRACKET:
                        token.Kind = TOKEN.RIGHT_BRACKET;
                        token.Value += (char)_inputStream.Read();
                        goto case STATE.MATCH;
                    case STATE.EOF:
                        bDone = true;
                        token.Kind = TOKEN.EOF;
                        return token;
                    //default:
                    //    break;
                }
                return token;
            }
            private void QueueTokens()
            {
                if ((!bDone) && (_outputQueue.Count < (_lookAhead + 1)))
                {
                    int nCount = _lookAhead - _outputQueue.Count;
                    while (nCount > 0)
                    {
                        nCount--;
                        _outputQueue.Enqueue(NextToken());
                    }
                }
            }
            public Token Scan()
            {
                Token result = _outputQueue.Peek();
                QueueTokens();
                if (_outputQueue.Peek().Kind != TOKEN.EOF)
                    return _outputQueue.Dequeue();
                return result;
            }
            public Token Peek(int k = 0)
            {
                if (k > _lookAhead)
                    throw new ArgumentOutOfRangeException("_lookAhead");
                QueueTokens();
                Token[] tokens = _outputQueue.ToArray();
                if (k >= _outputQueue.Count)
                {
                    foreach(Token token in tokens)// find EOF
                    {
                        if (token.Kind == TOKEN.EOF)
                            return token;
                    }
                }
                return tokens[k];
            }
            public static void Load(string inputFileName)
            {
                using (StreamReader inputStream = new StreamReader(inputFileName))
                {
                    Scanner scanner = new Scanner(inputStream);
                    while(scanner.Peek().Kind != TOKEN.EOF)
                    {
                        Token token = scanner.Scan();
                        switch(token.Kind)
                        {
                            case TOKEN.EOF:
                                Console.WriteLine("+ EOF:  {0}", token.Value);
                                break;
                            case TOKEN.UNRECOGNIZED:
                                Console.WriteLine("+ UNRECOGNIZED:  {0}", token.Value);
                                break;
                            case TOKEN.EOL:
                                Console.WriteLine("+ EOL:  ");
                                break;
                            case TOKEN.COMMENT:
                                Console.WriteLine("+ COMMENT:  {0}", token.Value);
                                break;
                            case TOKEN.WHITESPACE:
                                Console.WriteLine("+ WHITESPACE:  {0}", token.Value);
                                break;
                            case TOKEN.NUMBER:
                                Console.WriteLine("+ NUMBER:  {0}", token.Value);
                                break;
                            case TOKEN.IDENTIFIER:
                                Console.WriteLine("+ IDENTIFIER:  {0}", token.Value);
                                break;
                            case TOKEN.VALUE:
                                Console.WriteLine("+ VALUE:  {0}", token.Value);
                                break;
                            case TOKEN.LEFT_BRACKET:
                                Console.WriteLine("+ LEFT_BRACKET:  {0}", token.Value);
                                break;
                            case TOKEN.RIGHT_BRACKET:
                                Console.WriteLine("+ RIGHT_BRACKET:  {0}", token.Value);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("token.Kind");
                        }
                    }
                }
            }
        }
        public class Parser
        {
            private readonly Scanner _scanner;
            private Scanner.Token _lastToken;
            private Stack<Configuration> _stack = new Stack<Configuration>();
            public Parser(Scanner scanner)
            {
                _scanner = scanner;
            }
            private void Accept(Scanner.TOKEN token)
            {
                _lastToken = _scanner.Scan();
            }
            private void Expect(Scanner.TOKEN token, string errorMessage)
            {
                _lastToken = _scanner.Scan();
            }
            public Setting Setting()
            {
                Setting result = new Setting();
                Expect(Scanner.TOKEN.IDENTIFIER, "A Configuration Setting must START with an Identifier");
                result.Name = _lastToken.Value;
                Expect(Scanner.TOKEN.VALUE, "A Configuration Setting Identifier must be followed with a VALUE");
                result.Value = _lastToken.Value;
                return result;
            }
            public Dictionary<string, Setting> Block(Section section)
            {
                Dictionary<string, Setting> result = new Dictionary<string, Setting>();
                while(_scanner.Peek().Kind == Scanner.TOKEN.IDENTIFIER)
                {
                    Setting setting = Setting();
                    result.Add(setting.Name, setting);
                }
                return result;
            }
            public Section BlankSection()
            {
                Section result = new Section();
                result.Name = string.Empty;
                switch (_scanner.Peek().Kind)
                {
                    case Scanner.TOKEN.IDENTIFIER:
                        result.Settings = Block(result);
                        break;
                    default:
                        break;
                }
                return result;
            }
            public Section Section()
            {
                Section result = new Section();
                Expect(Scanner.TOKEN.LEFT_BRACKET, "A Section must START with a LEFT_BRACKET");
                Expect(Scanner.TOKEN.IDENTIFIER, "A Configuration Section must have an Identifier");
                result.Name = _lastToken.Value;
                Expect(Scanner.TOKEN.RIGHT_BRACKET, "A Configuration Section Name must be followed by a RIGHT_BRACKET");
                switch (_scanner.Peek().Kind)
                {
                    case Scanner.TOKEN.IDENTIFIER:
                        result.Settings = Block(result);
                        break;
                    default:
                        break;
                }
                return result;
            }
            public Catalog Parse()
            {
                Catalog result = new Catalog();
                Section section = null;
                while (_scanner.Peek().Kind != Scanner.TOKEN.EOF)
                {
                    switch (_scanner.Peek().Kind)
                    {
                        case Scanner.TOKEN.IDENTIFIER:
                            section = BlankSection();
                            result.Sections.Add(section.Name, section);
                            break;
                        case Scanner.TOKEN.LEFT_BRACKET:
                            section = Section();
                            result.Sections.Add(section.Name, section);
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
        }

        public Configuration()
            : base("Configuration")
        {

        }
        public static Catalog Load(string inputFileName)
        {
            Catalog result = null;
            using (StreamReader inputStream = new StreamReader(inputFileName))
            {
                Scanner scanner = new Scanner(inputStream);
                Parser parser = new Parser(scanner);
                result = parser.Parse();
            }
            return result;
        }
    }
}
