using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Generic
{
    public class BinaryTree<T>
        : Collection<T>
    {
        public enum TRAVERSAL_TYPE
        {
            DEPTH_FIRST_PRE_ORDER = 0,
            DEPTH_FIRST_IN_ORDER = 1,
            DEPTH_FIRST_POST_ORDER = 2,
            BREADTH_FIRST_LEVEL_ORDER = 3,
        }
        public new class Enumerator
            : IEnumerator<T>
        {
            protected BinaryTree<T> binaryTree;
            protected HashSet<int> visited = new HashSet<int>();
            protected Stack<int> memoryStack = new Stack<int>();
            public T Parent
            {
                get
                {
                    return binaryTree[binaryTree.parent(memoryStack.Peek())];
                }
            }
            public T Current
            {
                get
                {
                    return binaryTree[memoryStack.Peek()];
                }
            }
            object IEnumerator.Current { get { return Current; } }
            public T Left
            {
                get
                {
                    return binaryTree[binaryTree.left(memoryStack.Peek())];
                }
            }
            public T Right
            {
                get
                {
                    return binaryTree[binaryTree.right(memoryStack.Peek())];
                }
            }
            protected Func<bool> moveNext;
            public Enumerator(BinaryTree<T> binaryTree, TRAVERSAL_TYPE traversalType)
            {
                this.binaryTree = binaryTree;
                memoryStack.Push(0);
                switch (traversalType)
                {
                    case TRAVERSAL_TYPE.DEPTH_FIRST_PRE_ORDER:
                        moveNext = () =>
                        {
                            while (true)
                            {
                                int p = memoryStack.Peek();
                                int l = binaryTree.left(memoryStack.Peek());
                                int r = binaryTree.right(memoryStack.Peek());
                                if (!visited.Contains(memoryStack.Peek()))
                                {
                                    visited.Add(memoryStack.Peek());
                                    return true;
                                }
                                if (l < binaryTree.Count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[l], default)
                                    && !visited.Contains(l))
                                {
                                    memoryStack.Push(l);
                                    continue;
                                }
                                if (r < binaryTree.Count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[r], default)
                                    && !visited.Contains(r))
                                {
                                    memoryStack.Push(r);
                                    continue;
                                }
                                if (visited.Contains(memoryStack.Peek()))
                                    memoryStack.Pop();
                                if (memoryStack.Count > 0)
                                    continue;
                                else
                                    return false;
                            }
                        };
                        break;
                    case TRAVERSAL_TYPE.DEPTH_FIRST_IN_ORDER:
                        moveNext = () =>
                        {
                            while (true)
                            {
                                int p = memoryStack.Peek();
                                int l = binaryTree.left(memoryStack.Peek());
                                int r = binaryTree.right(memoryStack.Peek());
                                if (l < binaryTree.count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[l], default)
                                    && !visited.Contains(l))
                                {
                                    memoryStack.Push(l);
                                    continue;
                                }
                                if (!visited.Contains(memoryStack.Peek()))
                                {
                                    visited.Add(memoryStack.Peek());
                                    return true;
                                }
                                if (r < binaryTree.count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[r], default)
                                    && !visited.Contains(r))
                                {
                                    memoryStack.Push(r);
                                    continue;
                                }
                                if (visited.Contains(memoryStack.Peek()))
                                    memoryStack.Pop();
                                if (memoryStack.Count > 0)
                                    continue;
                                else
                                    return false;
                            }
                        };
                        break;
                    case TRAVERSAL_TYPE.DEPTH_FIRST_POST_ORDER:
                        moveNext = () =>
                        {
                            while (true)
                            {
                                int p = memoryStack.Peek();
                                int l = binaryTree.left(memoryStack.Peek());
                                int r = binaryTree.right(memoryStack.Peek());
                                if (l < binaryTree.count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[l], default)
                                    && !visited.Contains(l))
                                {
                                    memoryStack.Push(l);
                                    continue;
                                }
                                if (r < binaryTree.count
                                    && !EqualityComparer<T>.Default.Equals(binaryTree[r], default)
                                    && !visited.Contains(r))
                                {
                                    memoryStack.Push(r);
                                    continue;
                                }
                                if (!visited.Contains(memoryStack.Peek()))
                                {
                                    visited.Add(memoryStack.Peek());
                                    return true;
                                }
                                if (visited.Contains(memoryStack.Peek()))
                                    memoryStack.Pop();
                                if (memoryStack.Count > 0)
                                    continue;
                                else
                                    return false;
                            }
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("traversalType");
                }
            }
            public void Dispose()
            {
            }
            public bool MoveNext()
            {
                return moveNext();
            }
            public void Reset()
            {
                memoryStack.Clear();
                memoryStack.Push(0);
            }
        }
        protected TRAVERSAL_TYPE defaultTraversalType;
        public BinaryTree(int capacity = 64)
            : base(capacity)
        {
            defaultTraversalType = TRAVERSAL_TYPE.DEPTH_FIRST_IN_ORDER;
        }
        public BinaryTree(IEnumerable<T> source)
            : base(source)
        {
            defaultTraversalType = TRAVERSAL_TYPE.DEPTH_FIRST_IN_ORDER;
        }
        protected int parent(int index) { return (index - 1) / 2; }
        protected int left(int index) { return 2 * index + 1; }
        protected int right(int index) { return 2 * index + 2; }

        protected IEnumerator<T> getTraversalEnumerator(TRAVERSAL_TYPE traversalType)
        {
            return new Enumerator(this, traversalType);
        }
        public IEnumerable<T> Traverse(TRAVERSAL_TYPE traversalType)
        {
            Enumerator enumerator = new Enumerator(this, traversalType);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        public new IEnumerator<T> GetEnumerator()
        {
            return getTraversalEnumerator(defaultTraversalType);
        }
    }
}
