using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class Queue<T>
        : Collection<T>
    {
        protected int offset = 0;

        public Queue(int capacity = 8)
            : base(capacity)
        {

        }
        protected override int hashFunction(int key)
        {
            return (key + offset) % capacity;
        }
        //protected int postIncrement(int value)
        //{
        //    int ret = offset;
        //    offset += value;
        //    offset %= capacity;
        //    return ret;
        //}
        //protected int preDecrement(int value)
        //{
        //    offset -= value;
        //    if (offset < 0)
        //        offset += capacity;
        //    return offset;
        //}
        public void Enqueue(T item)
        {
            addToBack(item);
        }
        public T Peek(int k = 0)
        {
            return this[k];
        }
        protected override bool removeAtFront()
        {
            this[0] = default;
            count--;
            offset++;
            offset %= capacity;
            return true;
        }
        public void Dequeue()
        {
            removeAtFront();
        }
    }
}
