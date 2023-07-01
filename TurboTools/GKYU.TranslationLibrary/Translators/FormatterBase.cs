using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Translators
{
    public abstract class FormatterBase<INPUT_TYPE>
        : Emitter<INPUT_TYPE>
        , IWriteSymbols<INPUT_TYPE>
        where INPUT_TYPE : ISymbol
    {
    }
}
