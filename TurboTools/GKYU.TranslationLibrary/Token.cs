using GKYU.TranslationLibrary.Symbols;
using GKYU.TranslationLibrary.Translators;

namespace GKYU.TranslationLibrary
{
    public interface IVisitToken
        : IVisitSymbol
    {
        void Visit(Token token);
    }
    public class Token
        : ISymbol
    {
        public int ID { get; set; }
        public int Kind { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }
        public int Count { get; set; }
        public object Data { get; set; }
        public void Accept(IVisitSymbol symbol)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }
    public class Token<T>
        : Token
        , ISymbol<T>
    {
        public T Data { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Data);
        }
    }
}
