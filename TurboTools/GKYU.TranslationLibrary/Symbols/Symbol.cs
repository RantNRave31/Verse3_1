using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKYU.TranslationLibrary;
using GKYU.TranslationLibrary.Translators;
namespace GKYU.TranslationLibrary.Symbols
{
    public interface IVisitSymbol
    {
        void Visit(Symbol symbol);
        void Visit(Symbol.Named namedSymbol);
    }
    public interface ISymbol
    {
        int Kind { get; set; }
        string Name { get; set; }
        string Value { get; set; }
        int Count { get; set; }
        object Data { get; set; }
        void Accept(IVisitSymbol symbol);
    }
    public interface ISymbol<T>
        : ISymbol
    {
        new T Data { get; }
    }
    public partial class Symbol
        : ISymbol
    {
        public class Named
            : Symbol
        {
            public override string Name { get { if (_name == string.Empty) return this.ToString(); else return _name; } set { if (value == _name) return; _name = value; } }
            public Named()
                : base()
            {
                _name = string.Empty;
            }
            public Named(Symbol.Table symbolTable)
                : base(symbolTable)
            {
                _name = string.Empty;

            }
            public Named(Named reference)
                : base(reference)
            {
                _name = string.Empty;

            }
        }
        public class KindOf<T>
            : Named
        {
            public KindOf(Symbol.Table symbolTable)
                : base(symbolTable)
            {

            }
        }
        public int SymbolID { get; set; }
        public int Kind { get; set; }
        protected string _name;
        public virtual string Name { get { if (_name == string.Empty) return this.ToString(); else return _name; } set { if (value == _name) return; _name = value; } }
        public virtual string Value { get; set; }
        public virtual int Count { get; set; }
        public virtual object Data { get; set; }
        public virtual Symbol.Table SymbolTable { get; set; }
        public Symbol()
        {
            SymbolID = 0;
            Kind = 0;
            Count = 0;
        }
        public Symbol(Symbol.Table symbolTable)
            : this()
        {
            this.SymbolTable = symbolTable;
        }
        public Symbol(Symbol reference)
            : this(reference.SymbolTable)
        {
            SymbolID = reference.SymbolID;
            Kind = reference.Kind;
        }
        public override string ToString()
        {
            return string.Format("SYMBOL[{0}]", (Name != null && Name != string.Empty)?Name:SymbolID.ToString());
        }
        public bool Equals(Symbol other)
        {
            if (this.SymbolID == other.SymbolID)
                return true;
            else
                return false;
        }
        public virtual void Accept(IVisitSymbol visitor)
        {
            visitor.Visit(this);
        }
    }
}
