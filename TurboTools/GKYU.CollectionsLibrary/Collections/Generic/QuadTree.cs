using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.CoreLibrary;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class QuadTree<T>
            : Collection<T>
    {

        public QuadTree(int capacity = 16)
            : base(capacity)
        {

        }
        protected uint keyToBufferIndex(uint x, uint y)
        {
            return x.Interleave(y);
        }
        public T this[uint x, uint y]
        {
            get
            {
                return buffer[keyToBufferIndex(x, y)];
            }
            set
            {
                buffer[keyToBufferIndex(x, y)] = value;
            }
        }
        public QuadTree<T> this[uint x, uint y, uint radius]
        {
            get
            {
                return new QuadTree<T>(8);
            }
        }
        public T[] ToArray()
        {
            return buffer;
        }
    }
}
