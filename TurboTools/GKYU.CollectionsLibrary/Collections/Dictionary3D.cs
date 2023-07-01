using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    public class Dictionary3D<K, T>
    {
        Dictionary<K, Dictionary<K, Dictionary<K, T>>> _primaryDictionary = new Dictionary<K, Dictionary<K, Dictionary<K, T>>>();
        public Dictionary<K, Dictionary<K, T>> this[K primaryKey]
        {
            get
            {
                return _primaryDictionary[primaryKey];
            }
            set
            {
                _primaryDictionary[primaryKey] = value;
            }
        }
        public Dictionary<K, T> this[K primaryKey, K secondaryKey]
        {
            get
            {
                return _primaryDictionary[primaryKey][secondaryKey];
            }
            set
            {
                _primaryDictionary[primaryKey][secondaryKey] = value;
            }
        }
        public T this[K primaryKey, K secondaryKey, K tertiaryKey]
        {
            get
            {
                return _primaryDictionary[primaryKey][secondaryKey][tertiaryKey];
            }
            set
            {
                Dictionary<K, Dictionary<K, T>> secondaryDictionary = null;
                if (!_primaryDictionary.ContainsKey(primaryKey))
                    _primaryDictionary.Add(primaryKey, secondaryDictionary = new Dictionary<K, Dictionary<K, T>>());
                else
                    secondaryDictionary = _primaryDictionary[primaryKey];
                Dictionary<K, T> tertiaryDictionary = null;
                if (!secondaryDictionary.ContainsKey(secondaryKey))
                    secondaryDictionary.Add(secondaryKey, tertiaryDictionary = new Dictionary<K, T>());
                else
                    tertiaryDictionary = secondaryDictionary[secondaryKey];
                if (!tertiaryDictionary.ContainsKey(tertiaryKey))
                {
                    tertiaryDictionary.Add(tertiaryKey, value);
                }
                else
                {
                    tertiaryDictionary[tertiaryKey] = value;
                }
            }
        }
        public bool ContainsKey(K primaryKey, K secondaryKey, K tertiaryKey)
        {
            if (!_primaryDictionary.ContainsKey(primaryKey))
                return false;
            Dictionary<K, Dictionary<K, T>> secondaryDictionary = _primaryDictionary[primaryKey];
            if (!secondaryDictionary.ContainsKey(secondaryKey))
                return false;
            Dictionary<K, T> tertiaryDictionary = secondaryDictionary[secondaryKey];
            if (tertiaryDictionary.ContainsKey(tertiaryKey))
                return true;
            return false;
        }
        public IEnumerable<T> Where()
        {
            foreach (K primaryKey in _primaryDictionary.Keys)
            {
                foreach (K secondayKey in _primaryDictionary[primaryKey].Keys)
                {
                    foreach (K tertiaryKey in _primaryDictionary[primaryKey][secondayKey].Keys)
                    {
                        yield return _primaryDictionary[primaryKey][secondayKey][tertiaryKey];
                    }
                }
            }
        }
    }
}
