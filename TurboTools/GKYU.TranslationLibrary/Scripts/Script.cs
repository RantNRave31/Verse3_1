using GKYU.CoreLibrary;
using GKYU.CollectionsLibrary.Collections;
using GKYU.TranslationLibrary.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.CoreLibrary.MetaData;

namespace GKYU.TranslationLibrary.Scripts
{
    public abstract class Script
        : CompilerBase
    {
        public class Index
        {
        }
        public class Index<K,T>
            : Index
        {
            private Dictionary<K, T> _map = new Dictionary<K, T>();
            public Index()
            {

            }
            public T this[K key]
            {
                get
                {
                    return _map[key];
                }
                set
                {
                    _map[key] = value;
                }
            }
            public void Add(K key, T data)
            {
                _map.Add(key, data);
            }
            public void Remove(K key)
            {
                _map.Remove(key);
            }
        }
        public class Table
        {

        }
        public class Table<K,T>
            : Table
        {
            private Dictionary<Type, Index> _indices = new Dictionary<Type, Index>();
            internal Index<K,T> this[Type keyType]
            {
                get
                {
                    // need to add search for base class for the case when lookup index is of a base type and not the current type.
                    if(_indices.ContainsKey(keyType))
                        return (Index<K, T>)_indices[keyType];
                    foreach (Type baseClass in keyType.BaseClasses())
                    {
                        if (_indices.ContainsKey(baseClass))
                            return (Index<K, T>)_indices[baseClass];
                    }
                    return null;
                }
                set
                {
                    if (_indices.ContainsKey(keyType))
                        _indices[keyType] = value;
                    foreach (Type baseClass in keyType.BaseClasses())
                    {
                        if (_indices.ContainsKey(baseClass))
                            _indices[baseClass] = value;
                    }
                }
            }
            public void Add(K key, T data)
            {
                ((Index<K,T>)this[typeof(K)]).Add(key, data);
            }
            public void Remove(K key)
            {
                ((Index<K, T>)this[typeof(K)]).Remove(key);
            }
            public void CreateIndex<KEY_TYPE>()
            {
                Type keyType = typeof(KEY_TYPE);
                Index index = new Index<KEY_TYPE, T>();
                _indices.Add(keyType, index);
                foreach (Type baseClass in keyType.BaseClasses())
                {
                    if (!_indices.ContainsKey(baseClass))
                        _indices.Add(keyType, index);
                }
            }
        }
        public class DataContext
        {
            internal Table<Type, Table> _tables = new Table<Type, Table>();

            public DataContext()
            {
                _tables.CreateIndex<Type>();
                _tables.CreateIndex<string>();
            }
            public Table this[Type dataType]
            {
                get
                {
                    return ((Index<Type,Table>)_tables[dataType])[typeof(Type)];
                }
            }
            public void CreateTable<K,T>()
            {
                Type keyType = typeof(K);
                Type dataType = typeof(T);
                Table<K, T> newTable = new Table<K, T>();
                newTable.CreateIndex<K>();
                _tables.Add(keyType, newTable);
            }
            public void Add<K,T>(K key, T data)
            {
                Type keyType = typeof(K);
                Type dataType = typeof(T);
                Index<K, T> index = ((Table<K, T>)this[dataType])[keyType];
                index.Add(key, data);
            }
            public void Remove<K,T>(K key, T data)
            {
                Type keyType = typeof(K);
                Type dataType = typeof(T);
                Index<K, T> index = ((Table<K, T>)this[dataType])[keyType];
                index.Remove(key);
            }
        }
        protected DataContext _dataContext = new DataContext();
        protected Loader _loader = new Loader();
        protected Executor _executor = new Executor();
            public bool IsTest { get; set; }
        public Script(MacroProcessor macroProcessor)
            : base(macroProcessor)
        {

        }
        public virtual void Load(string name) 
        {
            if (null == name)
                _loader.LoadAll();
            _loader.Load(name); 
        }
        public virtual int Execute(string name = null)
        {
            if (null == name)
                return _executor.ExecuteAll();
            return _executor.Execute(name);
        }
    }
}
