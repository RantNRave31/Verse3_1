using System;
using System.Collections;
using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Symbols
{
    public partial class Symbol
    {
        public class Table
            : IEnumerable<Symbol>
        {
            public class Enumerator
                : IEnumerator<Symbol>
            {
                protected readonly IEnumerator<Symbol> inputEnumerator;
                public Symbol Current
                {
                    get
                    {
                        return inputEnumerator.Current;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return this.Current;
                    }
                }

                public Enumerator(Table table)
                {
                    inputEnumerator = table.GetEnumerator();
                }
                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    return inputEnumerator.MoveNext();
                }

                public void Reset()
                {
                    inputEnumerator.Reset();
                }
            }
            protected readonly Dictionary<string, Dictionary<string, Symbol>> namespaceMap = new Dictionary<string, Dictionary<string, Symbol>>();
            protected readonly GKYU.CollectionsLibrary.Collections.Generic.Stack<Dictionary<string, Symbol>> scopeStack = new GKYU.CollectionsLibrary.Collections.Generic.Stack<Dictionary<string, Symbol>>();
            protected readonly GKYU.CollectionsLibrary.Collections.Generic.Stack<string> namespaceStack = new GKYU.CollectionsLibrary.Collections.Generic.Stack<string>();
            protected Dictionary<int, Symbol> _primaryIndex { get; set; }
            protected List<Symbol> _symbols;
            public Table()
            {
                namespaceStack.Push(string.Empty);
                scopeStack.Push(new Dictionary<string, Symbol>());
                _symbols = new List<Symbol>();
                _primaryIndex = new Dictionary<int, Symbol>();
            }
            public IEnumerator<Symbol> GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            private Symbol find(string name)
            {
                Stack<Dictionary<string, Symbol>> stack = new Stack<Dictionary<string, Symbol>>(scopeStack);
                while (stack.Count > 0)
                {
                    Dictionary<string, Symbol> namespaceContext = stack.Pop();
                    if (namespaceContext.ContainsKey(name))
                        return namespaceContext[name];
                }
                return null;
            }
            public string Namespace
            {
                get
                {
                    return namespaceStack.Peek();
                }
            }
            public Symbol this[int index]    // Indexer declaration  
            {
                get
                {
                    return _primaryIndex[index];
                }
            }
            public Symbol this[string name]
            {
                get
                {
                    return find(name);
                }
            }
            public void EnterNamespace(string namespaceName)
            {
                namespaceStack.Push(namespaceName);
                scopeStack.Push(new Dictionary<string, Symbol>(scopeStack.Peek()));
            }
            public void ExitNamespace()
            {
                namespaceStack.Pop();
                scopeStack.Pop();
            }
            public void Register<T>()
                where T : Symbol
            {
                KindOf<T> kind = new KindOf<T>(this);
                kind.SymbolTable = this;
                kind.SymbolID = this._symbols.Count;
                kind.Kind = this._symbols.Count;
                kind.Name = typeof(T).Name;
                this.Add(kind.SymbolID, kind);
            }
            public T Create<T>()
                where T : Symbol
            {
                T result = (T)Activator.CreateInstance(typeof(T));
                result.SymbolTable = this;
                result.SymbolID = this._symbols.Count;
                result.Kind = this[typeof(T).Name].SymbolID;
                return result;
            }
            public T Create<T>(string name)
                where T : Symbol
            {
                T result = (T)Activator.CreateInstance(typeof(T));
                result.SymbolTable = this;
                result.SymbolID = this._symbols.Count;
                result.Kind = this[typeof(T).Name].SymbolID;
                return result;
            }
            public void Add(int key, Symbol data)
            {
                switch (data)
                {
                    case Named s:
                        scopeStack.Peek().Add(s.Name, s);
                        break;
                    default:
                        break;
                }
                _symbols.Add(data);
                _primaryIndex.Add(key, data);
            }
            public virtual void Report()
            {
            }

        }
    }
}
