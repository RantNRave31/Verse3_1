using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary.MetaData
{
    public static class ClassInfoExtensions
    {
        public static IEnumerable<Type> BaseClasses(this Type type)
        {
            Type baseClass = null;
            while (typeof(object) != (baseClass = type.BaseType))
            {
                yield return baseClass;
            }
        }
    }
}
