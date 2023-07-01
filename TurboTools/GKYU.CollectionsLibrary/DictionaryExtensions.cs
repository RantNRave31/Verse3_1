using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GKYU.CollectionsLibrary
{
    public static class DictionaryExtensions
    {
        public static void AddRange<KEY_TYPE,ITEM_TYPE>(this Dictionary<KEY_TYPE, ITEM_TYPE> target, Dictionary<KEY_TYPE,ITEM_TYPE> source)
        {
            foreach (KeyValuePair<KEY_TYPE, ITEM_TYPE> pair in source)
            {
                target.Add(pair.Key, pair.Value);
            }
        }
    }
}
