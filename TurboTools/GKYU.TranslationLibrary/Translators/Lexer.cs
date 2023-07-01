using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    using GKYU.CoreLibrary;
    using GKYU.TranslationLibrary.Symbols;

    public abstract class Lexer<OUTPUT_TYPE>
        : Scanner<OUTPUT_TYPE>
        , IStreamSymbols<OUTPUT_TYPE>
        where OUTPUT_TYPE : ISymbol
    {

    }
}
