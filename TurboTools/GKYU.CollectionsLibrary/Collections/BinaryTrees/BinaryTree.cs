using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKYU.CollectionsLibrary.Collections;

namespace GKYU.CollectionsLibrary.Collections.BinaryTrees
{
    public class BinaryTree<T>
        : IEnumerable<BinaryNode<T>>
        where T : IEquatable<T>
    {
        public class EnumeratorLR
            : IEnumerator<BinaryNode<T>>
        {
            public NodePath<T> stack;
            private BinaryNode<T> root = new BinaryNode<T>(-1);
            private BinaryNode<T> current;
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return current;
                }
            }
            public BinaryNode<T> Current
            {
                get
                {
                    return current;
                }
            }
            public EnumeratorLR(BinaryNode<T> node)
            {
                stack = new NodePath<T>();
                root.Left = node;
                current = null;
            }
            public void Dispose()
            {
                stack.Clear();
                stack = null;
                current = null;
            }
            public void Reset()
            {
                stack.Clear();
                current = null;
            }
            public bool MoveNext()
            {
                if (null == current)
                {
                    current = root;
                    while (null != current.Left)
                    {
                        stack.Push(current);
                        current = current.Left;
                    }
                }
                else if (null != current.Right)
                {
                    stack.Push(current);
                    current = current.Right;
                    while (null != current.Left)
                    {
                        stack.Push(current);
                        current = current.Left;
                    }
                }
                else
                {

                    while (current == ((BinaryNode<T>)stack.Peek()).Right)
                    {
                        current = (BinaryNode<T>)stack.Pop();
                    }
                    if (current.Right != stack.Peek())
                        current = (BinaryNode<T>)stack.Pop();
                }
                return current != root;
            }
        }
        public class EnumeratorRL
            : IEnumerator<BinaryNode<T>>
        {
            public NodePath<T> stack;
            private BinaryNode<T> root = new BinaryNode<T>(-1);
            private BinaryNode<T> current;
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return current;
                }
            }
            public BinaryNode<T> Current
            {
                get
                {
                    return current;
                }
            }
            public EnumeratorRL(BinaryNode<T> node)
            {
                stack = new NodePath<T>();
                root.Right = node;
                current = null;
            }
            public void Dispose()
            {
                stack.Clear();
                stack = null;
                current = null;
            }
            public void Reset()
            {
                stack.Clear();
                current = null;
            }
            public bool MoveNext()
            {
                if (null == current)
                {
                    current = root;
                    while (null != current.Right)
                    {
                        stack.Push(current);
                        current = current.Right;
                    }
                }
                else if (null != current.Left)
                {
                    stack.Push(current);
                    current = current.Left;
                    while (null != current.Right)
                    {
                        stack.Push(current);
                        current = current.Right;
                    }
                }
                else
                {

                    while (current == ((BinaryNode<T>)stack.Peek()).Left)
                    {
                        current = (BinaryNode<T>)stack.Pop();
                    }
                    if (current.Left != stack.Peek())
                        current = (BinaryNode<T>)stack.Pop();
                }
                return current != root;
            }
        }
        public BinaryNode<T> root;
        public BinaryNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
        private bool lefTrigonometryht;
        public BinaryTree(bool lefTrigonometryht = true)
        {
            root = null;
            this.lefTrigonometryht = lefTrigonometryht;
        }
        public virtual void Clear()
        {
            root = null;
        }
        public IEnumerator<BinaryNode<T>> GetEnumerator()
        {
            if (lefTrigonometryht) return new EnumeratorLR(root);
            else return new EnumeratorRL(root);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
