using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class Stack<T>
        : Collection<T>
    {
        public Stack(int capacity = 8)
            : base(capacity)
        {

        }
        public void Push(T item)
        {
            addToBack(item);
        }
        public T Peek(int k = 0)
        {
            return this[count - 1 - k];
        }
        public void Pop()
        {
            removeAtBack();
        }
    }
}
