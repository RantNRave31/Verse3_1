using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary
{
    public static class IntegerExtensions
    {
        public static bool Between<T>(this T n, T a, T b)
            where T: IComparable<T>
        {
            return (n.CompareTo(a) >= 0) && (n.CompareTo(b) <= 0);
        }
        public static bool In<T>(this T n, IEnumerable<T> e)
        {
            return e.Contains(n);
        }
    }
}
