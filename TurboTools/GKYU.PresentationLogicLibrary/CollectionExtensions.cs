using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GKYU.CoreLibrary
{
    public static class ColllectionExtensions
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.Invoke(addMethod, item);
        }
        public static void RemoveOnUI<T>(this ICollection<T> collection, T item)
        {
            Func<T, bool> removeMethod = collection.Remove;
            Application.Current.Dispatcher.Invoke(removeMethod, item);
        }
    }
}
