using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Collections;
using GKYU.CollectionsLibrary.Collections.Generic;

namespace GKYU.PresentationLogicLibrary.Collections
{
    public class ObservableStyleDictionary : ObservableSortedDictionary<string, Style>
    {
        #region constructors

        #region public

        public ObservableStyleDictionary() : base(new KeyComparer()) 
        {
        }

        #endregion public

        #endregion constructors

        #region key comparer class

        private class KeyComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        #endregion key comparer class

    }
}
