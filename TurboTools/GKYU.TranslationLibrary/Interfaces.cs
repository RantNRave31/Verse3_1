using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Translators;
using GKYU.TranslationLibrary.Patterns;
using GKYU.CoreLibrary.Attributes;

namespace GKYU.TranslationLibrary
{
    public interface ICreateSymbols<T>
         : IGenerateSymbol<T>
        where T : ISymbol
    {
     }
    public interface ICreateSymbol<T>
        where T : ISymbol
    {
    }

    public interface IBuildSymbols<T>
        : IGenerateSymbol<T>
        where T : ISymbol
    {
    }
    public interface ICopySymbols<T>
        : ITranslateSymbols<T,T>
        where T : ISymbol
    {
    }
    public interface IStreamSymbols<T>
        where T : ISymbol
    {
        Parameters Parameters { get; set; }
        Metrics Metrics { get; set; }
        bool EndOfFile { get; }
        T Peek(int k = 0);
        T Read();
    }
    public interface IStreamObjects<T>
        : IEnumerable<T>
    {
        Parameters Parameters { get; set; }
        Metrics Metrics { get; set; }
        bool EndOfFile { get; }
        T Peek(int k = 0);
        T Read();
    }
    public interface IBuildSymbol<T>
        where T : ISymbol
    {
    }
    public interface ILoadSymbols<T>
        where T : ISymbol
    {
        T1 Load<T1>();
    }
    public interface ISaveSymbols<T>
        where T : ISymbol
    {
        void Save(T element);
    }
}
