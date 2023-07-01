using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary.MetaData
{
    public class MetaData<T>
    {
        static PropertyInfo[] _properties = null;
        public static PropertyInfo[] Properties
        {
            get
            {
                if(null == _properties)
                    _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                return _properties;
            }
        }

    }
}
