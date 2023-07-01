using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Information
{
    public class GrammarValidationInfo
    {
        public LexerValidationInfo lexerResult = new LexerValidationInfo();
        public ParserValidationInfo parserResult = new ParserValidationInfo();
    }
}
