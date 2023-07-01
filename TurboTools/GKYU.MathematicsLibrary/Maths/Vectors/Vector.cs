using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.CoreLibrary.Expressions;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector<T>
    {
        private readonly T[] _components;

        public Vector(params T[] components)
        {
            _components = components;
        }

        public T this[int index]
        {
            get
            {
                return _components[index];
            }
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public double LengthSquared
        {
            get
            {
                double sum = 0;
                foreach(T n in _components)
                {
                    sum += ((dynamic)n * n);
                }
                return sum;
            }
        }
    }
}
