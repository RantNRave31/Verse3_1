using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class Collection<T>
        : ICollection<T>
    {
        public class Enumerator
            : IEnumerator<T>
        {
            protected Collection<T> sourceCollection;
            protected int key = 0;
            protected T current;
            public T Current
            {
                get
                {
                    return current;
                }
            }
            object IEnumerator.Current { get { return Current; } }
            public Enumerator(Collection<T> sourceCollection)
            {
                this.sourceCollection = sourceCollection;
            }
            public void Dispose()
            {
                sourceCollection = null;
                key = -1;
                current = default;
            }

            public bool MoveNext()
            {
                if (key < sourceCollection.Count)
                {
                    current = sourceCollection[key++];
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                key = 0;
                current = default;
            }
        }

        protected T[] buffer;
        protected int capacity;
        protected int count = 0;

        public virtual int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value > capacity)
                {
                    T[] temp = buffer;
                    buffer = new T[capacity = value];
                    count = 0;
                    copyFrom(temp);
                }
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public Collection(IEnumerable<T> source, int capacity = 8)
        {
            buffer = new T[capacity];
            this.capacity = buffer.Length;
            count = 0;
            copyFrom(source, 0);
        }
        public Collection(int capacity = 8)
            : this(new T[capacity], capacity)
        {
        }
        protected virtual void Dispose(bool disposing)
        {
            clear();
            buffer = null;
            capacity = 0;
            count = 0;
        }
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        protected virtual int hashFunction(int key)
        {
            return key % capacity;
        }
        public T this[int key]
        {
            get
            {
                return buffer[hashFunction(key)];
            }
            set
            {
                buffer[hashFunction(key)] = value;
            }
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(128);
            for (int key = 0; key < count; key++)
            {
                result.Append(this[key].ToString());
                result.Append(',');
            }
            if (result.Length > 0)
                result.Length--;
            return result.ToString();
        }
        protected static void swap(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }
        protected virtual void resize()
        {
            T[] temp = buffer;
            buffer = new T[capacity = capacity * 2];
            temp.CopyTo(buffer, 0);
        }
        protected virtual void copyFrom(IEnumerable<T> source, int index = 0)
        {
            clear();
            foreach (T item in source)
            {
                if (!EqualityComparer<T>.Default.Equals(item, default))
                    add(item);
            }
        }
        protected virtual void copyTo(T[] target, int index = 0)
        {
            for (int key = 0; key < count; key++)
            {
                target[index + key] = this[key];
            }
        }
        protected virtual void addAtKey(int key, T item)
        {
            if (count == capacity)
                resize();
            for (int index = key; index < count; index--)
            {
                this[index] = this[index + 1];
            }
            count++;
            this[key] = item;
        }
        protected virtual void addToFront(T item)
        {
            addAtKey(0, item);
        }
        protected virtual void addToBack(T item)
        {
            if (count == capacity)
                resize();
            buffer[count++] = item;
        }
        protected virtual void add(T item)
        {
            addToBack(item);
        }
        protected virtual void clear()
        {
            for (int n = 0; n < capacity; n++)
                buffer[n] = default;
            count = 0;
        }
        protected virtual int findIndexOf(T item)
        {
            for (int index = 0; index < count; index++)
            {
                if (EqualityComparer<T>.Default.Equals(this[index], item))
                    return index;
            }
            return -1;
        }
        protected bool contains(T item)
        {
            return findIndexOf(item) >= 0;
        }
        protected virtual bool removeAtFront()
        {
            if (count == 0)
                return false;
            removeAtKey(0);
            return true;
        }
        protected virtual bool removeAtKey(int key)
        {
            for (; key < count; key++)
            {
                buffer[key] = buffer[key + 1];
            }
            buffer[count--] = default;
            return true;
        }
        protected virtual bool removeAtBack()
        {
            buffer[count--] = default;
            return true;
        }
        protected virtual bool remove(T item)
        {
            int key = findIndexOf(item);
            if (key < 0)
                return false;
            return removeAtKey(key);
        }
        public void Add(T item)
        {
            add(item);
        }
        public void Clear()
        {
            clear();
        }
        public bool Contains(T item)
        {
            return contains(item);
        }
        public void CopyTo(T[] target, int index)
        {
            copyTo(target, index);
        }
        public bool Remove(T item)
        {
            return remove(item);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
