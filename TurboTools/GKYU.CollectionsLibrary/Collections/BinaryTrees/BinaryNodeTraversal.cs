using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.BinaryTrees
{
    public interface ITraverse<T>
    {
        void Traverse(T current);
    }
    public class PreorderTraversal<T>
        : ITraverse<BinaryNode<T>>
        where T : IEquatable<T>
    {
        protected Action<BinaryNode<T>> action;
        public PreorderTraversal(Action<BinaryNode<T>> action)
        {
            this.action = action;
        }
        public virtual void Traverse(BinaryNode<T> current)
        {
            if (current != null)
            {
                action(current);
                Traverse(current.Left);
                Traverse(current.Right);
            }

        }
    }
    public class InorderTraversal<T>
        : ITraverse<BinaryNode<T>>
        where T : IEquatable<T>
    {
        protected Action<BinaryNode<T>> action;
        public InorderTraversal(Action<BinaryNode<T>> action)
        {
            this.action = action;
        }
        public virtual void Traverse(BinaryNode<T> current)
        {
            if (current != null)
            {
                Traverse(current.Left);
                action(current);
                Traverse(current.Right);
            }

        }
    }
    public class PostorderTraversal<T>
        : ITraverse<BinaryNode<T>>
        where T : IEquatable<T>
    {
        protected Action<BinaryNode<T>> action;
        public PostorderTraversal(Action<BinaryNode<T>> action)
        {
            this.action = action;
        }
        public virtual void Traverse(BinaryNode<T> current)
        {
            if (current != null)
            {
                Traverse(current.Left);
                Traverse(current.Right);
                action(current);
            }

        }
    }
}
