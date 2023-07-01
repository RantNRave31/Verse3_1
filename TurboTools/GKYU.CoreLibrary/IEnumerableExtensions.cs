using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary
{
    public static class EnumerableExtensions
    {
        public static T[] Append<T>(this T[] array, T value )
        {
            int length = (null == array) ? 1 : array.Length + 1;
            T[] result = new T[length];
            if(null != array)
                array.CopyTo(result, 0);
            result[length - 1] = value;
            return result;
        }
        public static void Display<T>(this IEnumerable<T> items)
        {
            foreach (T item in items)
                Console.WriteLine(item.ToString());
        }
        public static IEnumerable<char> ScanStream(this Stream inputStream)
        {
            using (BinaryReader br = new BinaryReader(inputStream))
            {
                if (inputStream != null)
                    while (br.BaseStream.Position != br.BaseStream.Length)
                        yield return br.ReadChar();
            }
        }
        public static IEnumerable<string> ScanStreamLines(this StreamReader inputStream)
        {
            while (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
                yield return inputStream.ReadLine();
        }
        public static IEnumerable<T> Scan<T>(this IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                yield return item;
            }
            yield break;
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
