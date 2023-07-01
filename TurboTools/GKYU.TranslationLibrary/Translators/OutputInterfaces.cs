using GKYU.CoreLibrary.Attributes;
using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Translators
{
    public interface IWriteObjects<T>
    {
        Parameters Parameters { get; set; }
        Metrics Metrics { get; set; }
        bool CanWrite { get; }
        int Write<T>(T value);

    }
    public interface IWriteSymbols<T>
        where T : ISymbol
    {
        Parameters Parameters { get; set; }
        Metrics Metrics { get; set; }
        bool CanWrite { get; }
        int Write<T>(T value);

    }
}
