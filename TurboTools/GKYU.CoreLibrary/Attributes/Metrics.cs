using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.CoreLibrary.Attributes
{
    public class Metric
    {
        public string Name { get; set; }
        public virtual object Value { get; set; }
        public Type ValueType
        {
            get
            {
                return Value?.GetType();
            }
        }
        public Metric(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
    public class Metric<T>
        : Metric
    {
        public Metric(string name, object value)
            : base(name, value)
        {

        }
        public static implicit operator T(Metric<T> p) => (T)p.Value;
    }
    public class Metrics
        : IEnumerable<Metric>
    {
        protected Dictionary<string, Metric> _parameters { get; set; }
        public Metric this[string name]
        {
            get { return _parameters[name]; }
            set { _parameters[name] = value; }
        }
        public Metrics()
        {
            _parameters = new Dictionary<string, Metric>();
        }
        public void Add(Metric parameter)
        {
            _parameters.Add(parameter.Name, parameter);
        }
        public IEnumerator<Metric> GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
