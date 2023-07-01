using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.CoreLibrary.Attributes
{
    public class Parameter
    {
        public readonly string _name;
        public string Name { get { return _name; } }
        protected object _value;
        public virtual object Value { get { return _value; } set { _value = value; } }
        public Type ValueType
        {
            get
            {
                return Value?.GetType();
            }
        }
        public Parameter(string name, object value)
        {
            _name = name;
            _value = value;
        }
    }
    public class Parameter<T>
        : Parameter
    {
        public Parameter(string name, object value)
            : base(name, value)
        {

        }
        public static implicit operator T(Parameter<T> p) => (T)p.Value;
    }
    public class Parameters
        : IEnumerable<Parameter>
    {
        protected readonly Dictionary<string, Parameter> _properties;
        public Parameter this[string name]
        {
            get { return _properties[name]; }
            set { _properties[name] = value; }
        }
        public Parameters()
        {
            _properties = new Dictionary<string, Parameter>();
        }
        public void Add(Parameter parameter)
        {
            _properties.Add(parameter.Name, parameter);
        }
        public IEnumerator<Parameter> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
