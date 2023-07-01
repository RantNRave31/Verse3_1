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
    public interface ITranslateSymbol<T>
            : ITranslateSymbols<T, T>
            where T : ISymbol
    {

    }
    public interface ITranslateSymbols<INPUT_TYPE, OUTPUT_TYPE>
        where OUTPUT_TYPE : ISymbol
    {
        OUTPUT_TYPE Translate(INPUT_TYPE element);
    }
    public abstract class TranslatorBase<INPUT_TYPE,OUTPUT_TYPE>
        : ParserBase<INPUT_TYPE, OUTPUT_TYPE>
        , IReadSymbols<OUTPUT_TYPE>
        where INPUT_TYPE : ISymbol
        where OUTPUT_TYPE : ISymbol
    {
        public TranslatorBase(IReadSymbols<INPUT_TYPE> scanner)
            : base(scanner)
        {

        }
    }
}
