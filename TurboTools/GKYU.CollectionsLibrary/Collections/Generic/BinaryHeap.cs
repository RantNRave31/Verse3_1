using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public abstract class BinaryHeap<T>
        : BinaryTree<T>
    {
        public BinaryHeap(int capacity = 64)
            : base(capacity)
        {
        }
    }
}
