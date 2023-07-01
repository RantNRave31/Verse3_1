using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class Deque<T>
        : Queue<T>
    {
        public Deque(int capacity = 8)
            : base(capacity)
        {

        }
        protected override void addToFront(T item)
        {
            offset--;
            if (offset < 0)
                offset += capacity;
            count++;
            this[0] = item;
        }
        public void PushFront(T item)
        {
            addToFront(item);
        }
        public T PeekBack(int k = 0)
        {
            return this[count - 1 - k];
        }
        public void PopBack()
        {
            removeAtBack();
        }
    }
}
