using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class MinHeap<T>
        : BinaryHeap<T>
        where T : IComparable
    {
        public MinHeap(int capacity = 16)
            : base(capacity)
        {

        }
        protected void heapify(int index)
        {
            int l = left(index);
            int r = right(index);
            int min = index;
            if (l < count && buffer[l].CompareTo(buffer[index]) < 0)
                min = l;
            if (r < count && buffer[r].CompareTo(buffer[min]) > 0)
                min = r;
            if (min != index)
            {
                swap(ref buffer[index], ref buffer[min]);
                heapify(min);
            }
        }
        protected void decreaseIndex(int index, T value)
        {
            buffer[index] = value;
            while (index != 0 && buffer[parent(index)].CompareTo(buffer[index]) > 0)
            {
                swap(ref buffer[index], ref buffer[parent(index)]);
                index = parent(index);
            }
        }
        protected override void addToBack(T item)
        {
            if (count == capacity)
                throw new Exception("overflow");
            buffer[count++] = item;
            int index = count - 1;
            while (index > 0 && buffer[parent(index)].CompareTo(buffer[index]) > 0)
            {
                swap(ref buffer[index], ref buffer[parent(index)]);
                index = parent(index);
            }
        }
        protected override void add(T item)
        {
            addToBack(item);
        }
        protected override bool removeAtFront()
        {
            T root = buffer[0];
            if (count <= 0)
                return false;
            if (count == 1)
            {
                count--;
                return true;
            }
            buffer[0] = buffer[--count];
            heapify(0);
            return true;
        }
        protected override bool remove(T item)
        {
            int index = findIndexOf(item);
            decreaseIndex(index, default);
            removeAtFront();
            return true;
        }
        public T Peek(int n = 0)
        {
            return buffer[n];
        }
        public void Push(T item)
        {
            addToBack(item);
        }
        public void Pop()
        {
            removeAtFront();
        }
    }
}
