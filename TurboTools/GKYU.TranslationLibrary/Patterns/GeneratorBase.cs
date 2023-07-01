using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Symbols;
using GKYU.TranslationLibrary.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GKYU.TranslationLibrary.Patterns
{
    public interface IGenerateSymbol<OUTPUT_TYPE>
         where OUTPUT_TYPE : ISymbol
    {

    }
    public abstract class GeneratorBase<OUTPUT_TYPE>
        : ScannerBase<OUTPUT_TYPE>
        , IReadSymbols<OUTPUT_TYPE>
        where OUTPUT_TYPE : ISymbol
    {

    }
}
