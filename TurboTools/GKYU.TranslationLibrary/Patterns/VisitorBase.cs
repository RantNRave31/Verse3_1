using GKYU.TranslationLibrary.Symbols;
using GKYU.TranslationLibrary.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GKYU.TranslationLibrary.Patterns
{
    public interface IVisitSymbol<T>
       where T : ISymbol
    {
        void Visit(T element);
    }
    public abstract class VisitorBase<INPUT_TYPE>
        : IVisitSymbol
        , IVisitSymbol<INPUT_TYPE>
        where INPUT_TYPE : ISymbol
    {
        public VisitorBase()
            : base()
        {

        }


        public abstract void Visit(Symbol symbol);

        public abstract void Visit(Symbol.Named namedSymbol);
        public abstract void Visit(INPUT_TYPE element);
    }
}
