using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    public class Dictionary2D<K, T>
    {
        Dictionary<K, Dictionary<K, T>> _primaryDictionary = new Dictionary<K, Dictionary<K, T>>();
        public Dictionary<K, T> this[K primaryKey]
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
        public T this[K primaryKey, K secondaryKey]
        {
            get
            {
                return _primaryDictionary[primaryKey][secondaryKey];
            }
            set
            {
                Dictionary<K, T> secondaryDictionary = null;
                if (!_primaryDictionary.ContainsKey(primaryKey))
                    _primaryDictionary.Add(primaryKey, secondaryDictionary = new Dictionary<K, T>());
                else
                    secondaryDictionary = _primaryDictionary[primaryKey];
                if (!secondaryDictionary.ContainsKey(secondaryKey))
                {
                    secondaryDictionary.Add(secondaryKey, value);
                }
                else
                {
                    secondaryDictionary[secondaryKey] = value;
                }
            }
        }
        public bool ContainsKey(K primaryKey, K secondaryKey)
        {
            if (!_primaryDictionary.ContainsKey(primaryKey))
                return false;
            Dictionary<K, T> secondaryDictionary = _primaryDictionary[primaryKey];
            if (secondaryDictionary.ContainsKey(secondaryKey))
                return true;
            return false;
        }
    }
}
