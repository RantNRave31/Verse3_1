using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections;
    using GKYU.TranslationLibrary.Symbols;

    public partial class Syntax
    {
        public class SymbolTable
            : Symbol.Table
        {
            private static Dictionary<int, string> namedCharacters = new Dictionary<int, string>()
            {
                { -1, "EOF"},// EOF
                { 0x00, "NULL"},// NULL
                { 0x01, "SOH"},// Start of Heading
                { 0x02, "STX"},// Start of Text
                { 0x03, "ETX"},// End of Text
                { 0x04, "EOT"},// End of Transmission
                { 0x05, "ENQ"},// Enquiry
                { 0x06, "ACK"},// Acknowledge
                { 0x07, "BEL"},// Bell
                { 0x08, "BS"},// Backspace
                { 0x09, "TAB"},// Horizontal Tab
                { 0x0A, "LINE_FEED"},// 10 ('\n')
                { 0x0B, "VT"},// 11 Vertical Tab
                { 0x0C, "FF"},// 12 Form Feed
                { 0x0D, "CARRIAGE_RETURN"},// 13 ('\r')
                { 0x0E, "SO"},// 14 Shift Out
                { 0x0F, "SI"},// 15 Shift In
                { 0x10, "DLE"},// 16 Data Link Escape
                { 0x11, "DC1"},// 17 Device Control 1
                { 0x12, "DC2"},// 18 Device Control 2
                { 0x13, "DC3"},// 19 Device Control 3
                { 0x14, "DC4"},// 20 Device Control 4
                { 0x15, "NAK"},// 21 Negative Acknowledge
                { 0x16, "SYN"},// 22 Synchronous Idle
                { 0x17, "ETB"},// 23 End of Transmission Block
                { 0x18, "CAN"},// 24 Cancel
                { 0x19, "EM"},// 25 End of Medium
                { 0x1A, "SUB"},// 26 Substitute
                { 0x1B, "ESC"},// 27 Escape
                { 0x1C, "FS"},// 28 File Seperator
                { 0x1D, "GS"},// 29 Group Seperator
                { 0x1E, "RS"},// 30 Record Seperator
                { 0x1F, "US"},// 31 Unit Seperator
                { 32, "SPACE"},// 32
                { 33, "EXCLAIMATION_MARK"},// 33 ('!')
                { 34, "double_QUOTE"},// 34 ('"')
                { 35, "POUND_SIGN"},// 35'#'
                { 36, "DOLLAR_SIGN"},// 36'$'
                { 37, "PERCENT"},// 37'%'
                { 38, "AMPERSAND"},// 38'&'
                { 39, "SINGLE_QUOTE"},// 39'\''
                { 40, "LEFT_PARENTHESIS"},// 40'('
                { 41, "RIGHT_PARENTHESIS"},// 41')'
                { '*', "ASTERISK"},//'*',
                { '+', "PLUS_SIGN"},//'+',
                { ',', "COMMA"},//',',
                { '-', "MINUS_SIGN"},//'-',
                { '.', "PERIOD"},//'.',
                { '/', "FORWARD_SLASH"},//'/',
                { 48, "_0" },
                { 49, "_1" },
                { 50, "_2" },
                { 51, "_3" },
                { 52, "_4" },
                { 53, "_5" },
                { 54, "_6" },
                { 55, "_7" },
                { 56, "_8" },
                { 57, "_9" },
                { ':', "COLON"},//':',
                { ';', "SEMI_COLON"},//';',
                { '>', "GREATER_THAN"},//'>',
                { '=', "EQUAL_SIGN"},//'=',
                { '<', "LESS_THAN"},//'<',
                { '?', "QUESTION_MARK"},//'?',
                { '@', "AT_SIGN"},//'@',
                { '[', "LEFT_BRACKET"},// 91
                { '\\', "BACK_SLASH"},// 92
                { ']', "RIGHT_BRACKET"},// 93
                { '^', "CARET"},
                { '_', "UNDERSCORE"},// 95
                { '`', "GRAVE_ACCENT"},
                { '{', "LEFT_BRACE"},//'{',
                { '|', "PIPE"},//'|',
                { '}', "RIGHT_BRACE"},//'}',
                { '~', "TILDE" },
            };
            public Dictionary<string, Identifier> Identifiers = new Dictionary<string, Identifier>();
            public Dictionary<string, CharacterDeclaration> CharacterDeclarations = new Dictionary<string, CharacterDeclaration>();
            public Dictionary<string, CharacterSetDeclaration> CharacterSetDeclarations = new Dictionary<string, CharacterSetDeclaration>();
            public Dictionary<string, TokenDeclaration> tokenDeclarations = new Dictionary<string, TokenDeclaration>();
            public Dictionary<string, LexerDeclaration> lexerDeclarations = new Dictionary<string, LexerDeclaration>();
            public Dictionary<string, SymbolDeclaration> symbolDeclarations = new Dictionary<string, SymbolDeclaration>();
            public Dictionary<string, ParserDeclaration> parserDeclarations = new Dictionary<string, ParserDeclaration>();

            public SymbolTable()
            {
                Register<EndOfFile>();
                Register<Empty>();
                Register<EndOfLine>();

                Register<Identifier>();
                Register<Decimal>();
                Register<Integer>();
                Register<String>();
                Register<Symbolic>();
                Register<Variable>();
                Register<Reference>();
                Register<FileReference>();
                
                Register<Primative>();
                Register<Repitition>();
                Register<Choice>();
                Register<Sequence>();

                Register<TypeDeclaration>();
                Register<CharacterDeclaration>();
                Register<CharacterSetDeclaration>();
                Register<MacroExpression>();
                Register<TokenDeclaration>();
                Register<LexerDeclaration>();
                Register<SymbolDeclaration>();
                Register<ParserDeclaration>();
                Register<FileDeclaration>();
                Register<NamespaceDeclaration>();
                Register<ClassDeclaration>();
                Register<MemberDeclaration>();
                Register<MethodDeclaration>();

                Syntax.CharacterSetDeclaration ANY = this.DeclareCharacterSet("ANY", new CharacterSet(2, 126));
                Syntax.CharacterSetDeclaration WHITESPACE = this.DeclareCharacterSet("WHITESPACE", new CharacterSet(' ').And(new CharacterSet('\t')));
                Syntax.CharacterSetDeclaration EOL = this.DeclareCharacterSet("EOL", new CharacterSet('\r').And(new CharacterSet('\n')));
                Syntax.CharacterSetDeclaration UPPER_CASE = this.DeclareCharacterSet("UPPER_CASE", new CharacterSet('A', 'Z'));
                Syntax.CharacterSetDeclaration LOWER_CASE = this.DeclareCharacterSet("LOWER_CASE", new CharacterSet('a', 'z'));
                Syntax.CharacterSetDeclaration LETTER = this.DeclareCharacterSet("LETTER", new CharacterSet(UPPER_CASE.CharacterSet.Or(LOWER_CASE.CharacterSet)));
                Syntax.CharacterSetDeclaration DIGIT = this.DeclareCharacterSet("DIGIT", new CharacterSet('0', '9'));
            }
            public void Accept(IVisitSyntax visitor)
            {
                visitor.Visit(this);
            }
            public string GetCharacterName(char CharacterCode)
            {
                string name = string.Empty;
                if (namedCharacters.ContainsKey(CharacterCode))
                    name = namedCharacters[CharacterCode];
                else
                    name = (CharacterCode).ToString();
                return name;
            }
            public Identifier CreateIdentifier(string name)
            {
                Identifier newIdentifier = this.Create<Identifier>();
                newIdentifier.Name = name;
                Identifiers.Add(name, newIdentifier);
                return newIdentifier;
            }
            public Reference CreateReference(string modifiers, string type, string name, string style = null)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                Reference macroReference = Create<Reference>();
                macroReference.Identifier = identifier;
                return macroReference;
            }
            public Choice MakeChoice(SyntaxNode thisOne, SyntaxNode thatOne)
            {
                Choice newChoice = this.Create<Choice>();
                newChoice.thisOne = thisOne;
                newChoice.thatOne = thatOne;
                this.Add(newChoice.SymbolID, newChoice);
                return newChoice;
            }
            public Sequence MakeSequence(SyntaxNode first, SyntaxNode second)
            {
                Sequence newSequence = this.Create<Sequence>();
                newSequence.first = first;
                newSequence.second = second;
                return newSequence;
            }
            public Repitition MakeRepitition(SyntaxNode option)
            {
                Repitition newOption = this.Create<Repitition>();
                newOption.expression = option;
                this.Add(newOption.SymbolID, newOption);
                return newOption;
            }
            public Primative MakePrimative(SyntaxNode syntaxNode)
            {
                Primative newPrimative = this.Create<Primative>();
                newPrimative.referenceID = syntaxNode.SymbolID;
                newPrimative.Data = syntaxNode;
                this.Add(newPrimative.SymbolID, newPrimative);
                return newPrimative;
            }
            public CharacterDeclaration CreateCharacterDeclaration(string name, int CharacterCode)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                CharacterDeclaration newSymbol = this.Create<CharacterDeclaration>();
                newSymbol.identifier = identifier;
                newSymbol.CharacterCode = CharacterCode;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                CharacterDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public CharacterSetDeclaration CreateCharacterSet(string name, CharacterSet CharacterSet = null)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                foreach (int CharacterCode in CharacterSet)
                {
                    string CharacterName = string.Empty;
                    if (namedCharacters.ContainsKey(CharacterCode))
                        CharacterName = namedCharacters[CharacterCode];
                    else
                        CharacterName = ((char)CharacterCode).ToString();
                    if (!Identifiers.ContainsKey(CharacterName))
                        this.CreateCharacterDeclaration(CharacterName, CharacterCode);
                }
                CharacterSetDeclaration newSymbol = this.Create<CharacterSetDeclaration>();
                newSymbol.identifier = identifier;
                newSymbol.CharacterSet = CharacterSet;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                CharacterSetDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public MacroExpression CreateMacroDeclaration(string name)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                MacroExpression newSymbol = this.Create<MacroExpression>();
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                return newSymbol;
            }
            public TokenDeclaration CreateTokenDeclaration(string name)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                TokenDeclaration newSymbol = this.Create<TokenDeclaration>();
                newSymbol.identifier = identifier;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                tokenDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public SymbolDeclaration CreateSymbolDeclaration(string name)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                SymbolDeclaration newSymbol = this.Create<SymbolDeclaration>();
                newSymbol.identifier = identifier;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                symbolDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public FileReference UsingStatement(string value)
            {
                FileReference usingStatement = Create<FileReference>();
                usingStatement.Name = value;
                usingStatement.Value = value;
                return usingStatement;
            }
            public FileDeclaration FileDeclaration(string name, string style = null)
            {
                FileDeclaration fileDeclaration = Create<FileDeclaration>();
                fileDeclaration.Name = name;
                fileDeclaration.FileReferences = new Syntax.FileReference[] { };
                fileDeclaration.Namespaces = new Syntax.NamespaceDeclaration[] { };
                return fileDeclaration;
            }
            public NamespaceDeclaration NamespaceDeclaration(string name, string style = null)
            {
                NamespaceDeclaration namespaceDeclaration = Create<NamespaceDeclaration>();
                namespaceDeclaration.Name = name;
                namespaceDeclaration.Style = style;
                namespaceDeclaration.UsingStatements = new Syntax.FileReference[] { };
                namespaceDeclaration.Classes = new ClassDeclaration[] { };
                return namespaceDeclaration;
            }
            public ClassDeclaration ClassDeclaration(string modifiers, string name, string type = null, string style = null)
            {
                ClassDeclaration classDeclaration = Create<ClassDeclaration>();
                classDeclaration.Modifiers = modifiers;
                classDeclaration.Name = name;
                classDeclaration.Type = type;
                classDeclaration.Style = style;
                classDeclaration.Classes = new ClassDeclaration[] { };
                classDeclaration.Members = new MemberDeclaration[] { };
                classDeclaration.Methods = new MethodDeclaration[] { };
                return classDeclaration;
            }
            public MemberDeclaration MemberDeclaration(string modifiers, string type, string name, string style = null)
            {
                MemberDeclaration memberDeclaration = Create<MemberDeclaration>();
                memberDeclaration.Modifiers = modifiers;
                memberDeclaration.Name = name;
                memberDeclaration.Type = type;
                memberDeclaration.Style = style;
                return memberDeclaration;
            }
            public MethodDeclaration MethodDeclaration(string modifiers, string type, string name, string style = null)
            {
                MethodDeclaration methodDeclaration = Create<MethodDeclaration>();
                methodDeclaration.Modifiers = modifiers;
                methodDeclaration.Name = name;
                methodDeclaration.Type = type;
                methodDeclaration.Style = style;
                return methodDeclaration;
            }
            public LexerDeclaration CreateLexerDeclaration(string modifiers, string type, string name, string style = null)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                LexerDeclaration newSymbol = this.Create<LexerDeclaration>();
                newSymbol.identifier = identifier;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                lexerDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public ParserDeclaration CreateParserDeclaration(string modifiers, string type, string name, string style = null)
            {
                Identifier identifier;
                if (Identifiers.ContainsKey(name))
                    identifier = Identifiers[name];
                else
                    identifier = CreateIdentifier(name);
                ParserDeclaration newSymbol = this.Create<ParserDeclaration>();
                newSymbol.identifier = identifier;
                if (identifier.declarationSymbolID != 0)
                    throw new Exception("Symbol already defined");
                identifier.declarationSymbolID = newSymbol.SymbolID;
                this.Add(newSymbol.SymbolID, newSymbol);
                parserDeclarations.Add(name, newSymbol);
                return newSymbol;
            }
            public void Print(TextWriter trace, bool bDisplayCharacterDeclarations = false)
            {

                Console.WriteLine("Character Declarations:");
                Console.WriteLine("    {0} total Character declarations", CharacterDeclarations.Count);
                if (bDisplayCharacterDeclarations)
                {
                    foreach (CharacterDeclaration CharacterDeclaration in CharacterDeclarations.Values)
                    {
                        Console.WriteLine("    {0}[{1}] = '{2}'", CharacterDeclaration.ToString(), CharacterDeclaration.SymbolID, (char)CharacterDeclaration.CharacterCode);
                    }
                }
                Console.WriteLine("Character Set Declarations:");
                foreach (CharacterSetDeclaration CharacterSetDeclaration in CharacterSetDeclarations.Values)
                {
                    StringBuilder sb = new StringBuilder();
                    CharacterSet.Range currentRange = CharacterSetDeclaration.CharacterSet.head;
                    while (currentRange != null)
                    {
                        string from = GetCharacterName((char)currentRange.from);
                        string to = GetCharacterName((char)currentRange.to);
                        if (currentRange.from == currentRange.to)
                            sb.Append(string.Format("'{0}',", from));
                        else
                            sb.Append(string.Format("'{0}'..'{1}',", from, to));
                        currentRange = currentRange.next;
                    }
                    Console.WriteLine("    {0}[{1}] = {2}", CharacterSetDeclaration.ToString(), CharacterSetDeclaration.SymbolID, sb.ToString());
                }
                Console.WriteLine("Token Declarations:");
                foreach (TokenDeclaration tokenDeclaration in tokenDeclarations.Values)
                {
                    Console.WriteLine("    {0}[{1}] = {2}", tokenDeclaration.ToString(), tokenDeclaration.SymbolID, tokenDeclaration.definition);
                }
                Console.WriteLine("Lexer Declarations:");
                foreach (LexerDeclaration lexerDeclaration in lexerDeclarations.Values)
                {
                    Console.WriteLine("    {0}[{1}] = {2}", lexerDeclaration.ToString(), lexerDeclaration.SymbolID, lexerDeclaration.definition);
                }

            }
        }

    }
}
