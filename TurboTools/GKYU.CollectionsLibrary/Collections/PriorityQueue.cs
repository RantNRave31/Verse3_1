using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    /// <summary>
    /// Code adapted From Dr. James McCaffrey
    /// https://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
    /// </summary>
    /// <typeparam name="DATA_TYPE"></typeparam>
    /// <typeparam name="PRIORITY_TYPE"></typeparam>
    public class PriorityQueue<DATA_TYPE, PRIORITY_TYPE>
        where PRIORITY_TYPE : IComparable<PRIORITY_TYPE>
    {
        private List<Tuple<DATA_TYPE, PRIORITY_TYPE>> data;
        public int Count()
        {
            return data.Count;
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int n = 0; n < data.Count - 1; n++)
            {
                result.Append(data[n].Item1.ToString());
                result.Append(string.Format("[{0}],", data[n].Item2));
            }
            result.Append(data[data.Count].Item1.ToString());
            result.Append(string.Format("[{0}] ", data[data.Count].Item2));
            result.Append(string.Format("Count = {0}", data.Count));
            return result.ToString();
        }
        public PriorityQueue()
        {
            data = new List<Tuple<DATA_TYPE, PRIORITY_TYPE>>();
        }
        public void Enqueue(Tuple<DATA_TYPE, PRIORITY_TYPE> item)
        {
            data.Add(item);
            int childIndex = data.Count - 1;
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;
                if (data[childIndex].Item2.CompareTo(data[parentIndex].Item2) >= 0)
                    break;
                Tuple<DATA_TYPE, PRIORITY_TYPE> temp = data[childIndex];
                data[childIndex] = data[parentIndex];
                data[parentIndex] = temp;
                childIndex = parentIndex;
            }
        }
        public Tuple<DATA_TYPE, PRIORITY_TYPE> Peek()
        {
            if (data.Count == 0)
                return null;
            return data[0];
        }
        public Tuple<DATA_TYPE, PRIORITY_TYPE> Dequeue()
        {
            if (data.Count == 0)
                throw new InvalidOperationException("Cannot Dequeue empty collection");
            int lastIndex = data.Count - 1;
            Tuple<DATA_TYPE, PRIORITY_TYPE> frontItem = data[0];
            data[0] = data[lastIndex];
            data.RemoveAt(lastIndex);
            if (data.Count != 0)
            {
                lastIndex--;
                int parentIndex = 0;
                while (true)
                {
                    int leftChildIndex = parentIndex * 2 + 1;
                    if (leftChildIndex > lastIndex)
                        break;
                    int rightChildIndex = leftChildIndex + 1;
                    if (rightChildIndex <= lastIndex && data[rightChildIndex].Item2.CompareTo(data[leftChildIndex].Item2) < 0)
                        leftChildIndex = rightChildIndex;
                    if (data[parentIndex].Item2.CompareTo(data[leftChildIndex].Item2) <= 0)
                        break;
                    Tuple<DATA_TYPE, PRIORITY_TYPE> temp = data[parentIndex];
                    data[parentIndex] = data[leftChildIndex];
                    data[leftChildIndex] = temp;
                    parentIndex = leftChildIndex;
                }
            }
            return frontItem;
        }
        public void ChangePriority(DATA_TYPE data, PRIORITY_TYPE priority)
        {
            for (int n = 0; n < this.data.Count; n++)
            {
                if (this.data[n].Item1.Equals(data))
                {
                    this.data.RemoveAt(n);
                    Enqueue(new Tuple<DATA_TYPE, PRIORITY_TYPE>(data, priority));
                    break;
                }
            }
        }
        public bool IsValid()
        {
            if (data.Count == 0)
                return true;
            int lastIndex = data.Count - 1;
            for (int parentIndex = 0; parentIndex < data.Count; parentIndex++)
            {
                int leftChildIndex = 2 * parentIndex + 1;
                int rightChildIndex = 2 * parentIndex + 2;
                if (leftChildIndex <= lastIndex && data[parentIndex].Item2.CompareTo(data[leftChildIndex].Item2) > 0)
                    return false;
                if (rightChildIndex <= lastIndex && data[parentIndex].Item2.CompareTo(data[rightChildIndex].Item2) > 0)
                    return false;
            }
            return true;
        }
    }
}
