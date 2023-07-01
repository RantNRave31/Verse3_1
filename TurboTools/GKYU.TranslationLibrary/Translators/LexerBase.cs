using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    using GKYU.CoreLibrary;
    using GKYU.TranslationLibrary.Symbols;

    public abstract class LexerBase<OUTPUT_TYPE>
        : ScannerBase<OUTPUT_TYPE>
        , IReadSymbols<OUTPUT_TYPE>
        where OUTPUT_TYPE : ISymbol
    {

    }
}
