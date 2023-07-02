using System.Reflection;

namespace Core
{
    public class MetaData<T>
    {
        static PropertyInfo[] _properties = null;
        public static PropertyInfo[] Properties
        {
            get
            {
                if (null == _properties)
                    _properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                return _properties;
            }
        }

    }
}
