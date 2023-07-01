using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections;
    public class GrammarConstructor
    {
        public static Syntax.LexerDeclaration CreateSimpleGrammar1()
        {
            Syntax.SymbolTable symbolTable = new Syntax.SymbolTable();

            symbolTable.DeclareCharacter("Empty", 0);
            symbolTable.DeclareCharacter("NoMatch", 1);
            Syntax.LexerDeclaration grammarDeclaration = symbolTable.CreateLexerDeclaration("public","","Test");
            symbolTable.DeclareCharacter("EOF", -1);

            Syntax.CharacterDeclaration UNDERSCORE = symbolTable.DeclareCharacter("UNDERSCORE", '_');
            symbolTable.DeclareCharacter("A", 'A');

            Syntax.CharacterSetDeclaration ANY = symbolTable.DeclareCharacterSet("ANY", new CharacterSet(2, 126));
            Syntax.CharacterSetDeclaration UPPER_CASE = symbolTable.DeclareCharacterSet("UPPER_CASE", new CharacterSet('A', 'Z'));
            Syntax.CharacterSetDeclaration LOWER_CASE = symbolTable.DeclareCharacterSet("LOWER_CASE", new CharacterSet('a', 'z'));
            Syntax.CharacterSetDeclaration LETTER = symbolTable.DeclareCharacterSet("LETTER", new CharacterSet(UPPER_CASE.CharacterSet.Or(LOWER_CASE.CharacterSet)));
            Syntax.CharacterSetDeclaration DIGIT = symbolTable.DeclareCharacterSet("DIGIT", new CharacterSet('0', '9'));



            Syntax.TokenDeclaration IDENTIFIER = symbolTable.DeclareToken("IDENTIFIER");
            IDENTIFIER.definition = "LETTER [LETTER | DIGIT | UNDERSCORE]";
            IDENTIFIER.expression = LETTER.ThenRepeat(LETTER.Or(DIGIT).Or(UNDERSCORE));

            Syntax.TokenDeclaration NUMBER = symbolTable.DeclareToken("NUMBER");
            NUMBER.definition = "DIGIT [DIGIT]";
            NUMBER.expression = DIGIT.ThenRepeat(DIGIT);


            return grammarDeclaration;
        }
        public static Syntax.LexerDeclaration CreateSimpleGrammar2()
        {
            Syntax.SymbolTable symbolTable = new Syntax.SymbolTable();

            symbolTable.DeclareCharacter("Empty", 0);
            Syntax.TokenDeclaration NoMatch = symbolTable.CreateTokenDeclaration("NoMatch");
            //symbolTable.DeclareCharacter("NoMatch", 1);
            Syntax.LexerDeclaration lexerDeclaration = symbolTable.CreateLexerDeclaration("public","", "Test");

            symbolTable.DeclareCharacter("eof", -1);
            Syntax.TokenDeclaration EOF = symbolTable.DeclareToken("EOF");
            EOF.definition = "eof";
            EOF.expression = (Syntax.CharacterDeclaration)symbolTable["eof"];

            Syntax.CharacterSetDeclaration ANY = symbolTable.DeclareCharacterSet("ANY", new CharacterSet(2, 126));

            Syntax.CharacterDeclaration UNDERSCORE = symbolTable.DeclareCharacter("UNDERSCORE", '_');
            symbolTable.DeclareCharacter("A", 'A');

            Syntax.CharacterSetDeclaration UPPER_CASE = symbolTable.DeclareCharacterSet("UPPER_CASE", new CharacterSet('A', 'Z'));
            Syntax.CharacterSetDeclaration LOWER_CASE = symbolTable.DeclareCharacterSet("LOWER_CASE", new CharacterSet('a', 'z'));
            Syntax.CharacterSetDeclaration LETTER = symbolTable.DeclareCharacterSet("LETTER", new CharacterSet(UPPER_CASE.CharacterSet.Or(LOWER_CASE.CharacterSet)));
            Syntax.CharacterSetDeclaration DIGIT = symbolTable.DeclareCharacterSet("DIGIT", new CharacterSet('0', '9'));
            Syntax.CharacterSetDeclaration PRINTABLE = symbolTable.DeclareCharacterSet("PRINTABLE", new CharacterSet(ANY.CharacterSet.Subtract(new CharacterSet('"', '"'))));
            Syntax.CharacterSetDeclaration CHARACTER = symbolTable.DeclareCharacterSet("CHARACTER", new CharacterSet(ANY.CharacterSet.Subtract(new CharacterSet('\'', '\''))));

            Syntax.TokenDeclaration IDENTIFIER = symbolTable.DeclareToken("IDENTIFIER");
            IDENTIFIER.definition = "LETTER [LETTER | DIGIT | UNDERSCORE]";
            IDENTIFIER.expression = LETTER.ThenRepeat(LETTER.Or(DIGIT).Or(UNDERSCORE));

            Syntax.TokenDeclaration NUMBER = symbolTable.DeclareToken("NUMBER");
            NUMBER.definition = "DIGIT [DIGIT]";
            NUMBER.expression = DIGIT.ThenRepeat(DIGIT);

            Syntax.TokenDeclaration STRING = symbolTable.DeclareToken("STRING");
            STRING.definition = "'\"' PRINTABLE* '\"'";
            STRING.expression = ('"').ThenRepeat(PRINTABLE).Then('"');

            Syntax.TokenDeclaration CHARACTER_LITORAL = symbolTable.CreateTokenDeclaration("CHARACTER_LITORAL");
            CHARACTER_LITORAL.definition = "'\\'' CHARACTER '\\''";
            CHARACTER_LITORAL.expression = ('\'').Then(CHARACTER).Then('\'');

            Syntax.TokenDeclaration ASSIGN = symbolTable.DeclareToken("ASSIGN");
            ASSIGN.definition = "'='";
            ASSIGN.expression = (Syntax.CharacterDeclaration)symbolTable["EQUAL_SIGN"];

            Syntax.TokenDeclaration ELLIPSIS = symbolTable.DeclareToken("ELLIPSIS");
            ELLIPSIS.definition = "'.' '.'";
            ELLIPSIS.expression = ('.').Then((Syntax.CharacterDeclaration)symbolTable["PERIOD"]);

            Syntax.TokenDeclaration END_STATEMENT = symbolTable.DeclareToken("END_STATEMENT");
            END_STATEMENT.definition = "';'";
            END_STATEMENT.expression = (Syntax.CharacterDeclaration)symbolTable["SEMI_COLON"];

            return lexerDeclaration;
        }
        public static Syntax.SymbolTable CreateSimpleGrammar()
        {
            Syntax.SymbolTable symbolTable = new Syntax.SymbolTable();

            symbolTable.DeclareCharacter("Empty", 0);
            Syntax.TokenDeclaration NoMatch = symbolTable.CreateTokenDeclaration("NoMatch");
            NoMatch.definition = "0";
            NoMatch.expression = (Syntax.CharacterDeclaration)symbolTable["Empty"];
            // apparently, with this design, you should not give nomatch a match expression or it will introduce artifacts.
            //NoMatch.expression = (Grammar.CharacterDeclaration)symbolTable["Empty"];

            //symbolTable.DeclareCharacter("NoMatch", 1);
            Syntax.LexerDeclaration lexerDeclaration = symbolTable.CreateLexerDeclaration("public","","TestLexer");
            Syntax.ParserDeclaration parserDeclaration = symbolTable.CreateParserDeclaration("public","","TestParser");

            symbolTable.DeclareCharacter("eof", -1);
            Syntax.TokenDeclaration EOF = symbolTable.DeclareToken("EOF");
            EOF.definition = "eof";
            EOF.expression = (Syntax.CharacterDeclaration)symbolTable["eof"];

            Syntax.CharacterSetDeclaration ANY = symbolTable.DeclareCharacterSet("ANY", new CharacterSet(2, 126));

            Syntax.CharacterDeclaration UNDERSCORE = symbolTable.DeclareCharacter("UNDERSCORE", '_');
            symbolTable.DeclareCharacter("A", 'A');

            Syntax.CharacterSetDeclaration UPPER_CASE = symbolTable.DeclareCharacterSet("UPPER_CASE", new CharacterSet('A', 'Z'));
            Syntax.CharacterSetDeclaration LOWER_CASE = symbolTable.DeclareCharacterSet("LOWER_CASE", new CharacterSet('a', 'z'));
            Syntax.CharacterSetDeclaration LETTER = symbolTable.DeclareCharacterSet("LETTER", new CharacterSet(UPPER_CASE.CharacterSet.Or(LOWER_CASE.CharacterSet)));
            Syntax.CharacterSetDeclaration DIGIT = symbolTable.DeclareCharacterSet("DIGIT", new CharacterSet('0', '9'));
            Syntax.CharacterSetDeclaration PRINTABLE = symbolTable.DeclareCharacterSet("PRINTABLE", new CharacterSet(ANY.CharacterSet.Subtract(new CharacterSet('"', '"'))));
            Syntax.CharacterSetDeclaration CHARACTER = symbolTable.DeclareCharacterSet("CHARACTER", new CharacterSet(ANY.CharacterSet.Subtract(new CharacterSet('\'', '\''))));

            Syntax.TokenDeclaration IDENTIFIER = symbolTable.DeclareToken("IDENTIFIER");
            IDENTIFIER.definition = "LETTER [LETTER | DIGIT | UNDERSCORE]";
            IDENTIFIER.expression = LETTER.ThenRepeat(LETTER.Or(DIGIT).Or(UNDERSCORE));

            Syntax.TokenDeclaration NUMBER = symbolTable.DeclareToken("NUMBER");
            NUMBER.definition = "DIGIT [DIGIT]";
            NUMBER.expression = DIGIT.ThenRepeat(DIGIT);

            Syntax.TokenDeclaration STRING = symbolTable.DeclareToken("STRING");
            STRING.definition = "'\"' PRINTABLE* '\"'";
            STRING.expression = ('"').ThenRepeat(PRINTABLE).Then('"');

            Syntax.TokenDeclaration CHARACTER_LITORAL = symbolTable.CreateTokenDeclaration("CHARACTER_LITORAL");
            CHARACTER_LITORAL.definition = "'\\'' CHARACTER '\\''";
            CHARACTER_LITORAL.expression = ('\'').Then(CHARACTER).Then('\'');

            Syntax.TokenDeclaration ASSIGN = symbolTable.DeclareToken("ASSIGN");
            ASSIGN.definition = "'='";
            ASSIGN.expression = (Syntax.CharacterDeclaration)symbolTable["EQUAL_SIGN"];

            Syntax.TokenDeclaration ELLIPSIS = symbolTable.DeclareToken("ELLIPSIS");
            ELLIPSIS.definition = "'.' '.'";
            ELLIPSIS.expression = ('.').Then((Syntax.CharacterDeclaration)symbolTable["PERIOD"]);

            Syntax.TokenDeclaration END_STATEMENT = symbolTable.DeclareToken("END_STATEMENT");
            END_STATEMENT.definition = "';'";
            END_STATEMENT.expression = (Syntax.CharacterDeclaration)symbolTable["SEMI_COLON"];

            Syntax.SymbolDeclaration Expression = symbolTable.CreateSymbolDeclaration("Expression");
            Expression.definition = "Term (('+' | '-') Term )*";

            Syntax.SymbolDeclaration Primative = symbolTable.CreateSymbolDeclaration("Primative");
            Primative.definition = "IDENTIFIER | CHARACTER_LITORAL | '(' Expression ')'";
            Primative.expression = IDENTIFIER.Or(CHARACTER_LITORAL).Or('(').Then(Expression).Then(')');

            Syntax.SymbolDeclaration Factor = symbolTable.CreateSymbolDeclaration("Factor");
            Factor.expression = Primative.Then('*');

            Syntax.SymbolDeclaration Term = symbolTable.CreateSymbolDeclaration("Term");
            Term.definition = "Factor (Factor)";
            Term.expression = Factor.ThenRepeat(((symbolTable.CharacterDeclarations["ASTERISK"]).Or('/')).Then(Factor));

            Expression.expression = Term.ThenRepeat(((symbolTable.CharacterDeclarations["PLUS_SIGN"]).Or('-')).Then(Term));

            Syntax.SymbolDeclaration TokenDeclaration = symbolTable.CreateSymbolDeclaration("TokenDeclaration");
            TokenDeclaration.definition = "IDENTIFIER ASSIGN Expression END_STATEMENT";
            TokenDeclaration.expression = IDENTIFIER.Then(ASSIGN).Then(Expression).Then(END_STATEMENT);

            return symbolTable;
        }
    }
}
