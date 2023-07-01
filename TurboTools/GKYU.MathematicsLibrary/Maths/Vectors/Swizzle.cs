using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Swizzle : DynamicObject
    {
        private readonly Dictionary<char, object> _members = new Dictionary<char, object>();

        public void Add(char c, object val)
        {
            _members.Add(c, val);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var array = binder.Name.Select(c =>
            {
                object value;
                if (_members.TryGetValue(c, out value))
                {
                    return value;
                }
                throw new Exception(String.Format("Member Does Not Exist: {0}", c));
            }).ToArray();
            var type = array[0].GetType();

            if (binder.Name.Length == 1)
            {
                result = array[0];
            }
            else if (type == typeof(float))
            {
                result = FromFloatArray(array.Cast<float>().ToArray());
            }
            else if (type == typeof(double))
            {
                result = FromdoubleArray(array.Cast<double>().ToArray());
            }
            else
            {
                result = array;
            }
            return true;
        }

        private object FromFloatArray(float[] array)
        {
            switch (array.Length)
            {
                case 2:
                    return new Vector2F(array[0], array[1]);
                case 3:
                    return new Vector3DF(array[0], array[1], array[2]);
                case 4:
                    return new Vector4F(array[0], array[1], array[2], array[3]);
                default:
                    return new Vector<float>(array);
            }
        }
        private object FromdoubleArray(double[] array)
        {
            switch (array.Length)
            {
                case 2:
                    return new Vector2D(array[0], array[1]);
                case 3:
                    return new Vector3D(array[0], array[1], array[2]);
                case 4:
                    return new Vector4D(array[0], array[1], array[2], array[3]);
                default:
                    return new Vector<double>(array);
            }
        }
    }
}
