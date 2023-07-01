using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Maths
{
    public class Range<T>
    {
        private Range<float> topSpeed;

        public T Minimum { get; set; }
        public T Maximum { get; set; }
        public float Current { get; set; }

        public Range(T a, T b, T c)
        {

        }
        public Range(T a, T b)
            : this(a,b,default(T))
        {

        }

        public Range(Range<float> topSpeed)
        {
            this.topSpeed = topSpeed;
        }
    }
}
