using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GKYU.TranslationLibrary.Domains.Texts
{
    public partial class Text
    {
        public enum TOKEN : int
        {
            EOF,
            UNRECOGNIZED,
            EOL,
            WHITESPACE,
            SYMBOL,
            NUMBER,
            WORD,
        };
        public static Dictionary<string, TOKEN> TokenList = new Dictionary<string, TOKEN>()
        {
            // Scanner Tokens
            { "EOF", TOKEN.EOF },
            { "UNRECOGNIZED", TOKEN.UNRECOGNIZED },
            { "EOL", TOKEN.EOL },
            { "WHITESPACE", TOKEN.WHITESPACE },
            { "SYMBOL", TOKEN.SYMBOL },
            { "NUMBER", TOKEN.NUMBER },
            { "WORD", TOKEN.WORD },
            // Symbols
  };
        public enum TOKENSET : int
        {
        };
        public static Dictionary<TOKENSET, HashSet<WORD>> TokenSets = new Dictionary<TOKENSET, HashSet<WORD>>()
        {
        };
        public enum SYMBOL : int
        {
            EOF,
            Unrecognized,
            EOL,
            WhiteSpace,
            PersonSymbol,
            Number,
            Word,
        };
        public static Dictionary<string, SYMBOL> SymbolList = new Dictionary<string, SYMBOL>()
        {
            // Symbols
            { "EOF", SYMBOL.EOF },
            { "Unrecognized", SYMBOL.Unrecognized },
            { "EOL", SYMBOL.EOL },
            { "WhiteSpace", SYMBOL.WhiteSpace },
            { "PersonSymbol", SYMBOL.PersonSymbol },
            { "Number", SYMBOL.Number },
            { "Word", SYMBOL.Word },
        };
    }
}