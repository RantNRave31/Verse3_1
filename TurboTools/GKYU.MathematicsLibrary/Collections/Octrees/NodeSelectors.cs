using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Collections.Octrees
{
    public interface INodeSelector<O>
    {
        IOctreeNode<O> Choose(IOctreeNode<O> a, IOctreeNode<O> b);
    }
    public static class Selectors<O>
    {
        public static INodeSelector<O> First { get { return new NodeSelectorFunc<O>((a, b) => a); } }
        public static INodeSelector<O> Second { get { return new NodeSelectorFunc<O>((a, b) => b); } }
    }
    public class NodeSelectorFunc<O> : INodeSelector<O>
    {
        private readonly Func<IOctreeNode<O>, IOctreeNode<O>, IOctreeNode<O>> _func;

        public NodeSelectorFunc(Func<IOctreeNode<O>, IOctreeNode<O>, IOctreeNode<O>> func)
        {
            _func = func;
        }

        public IOctreeNode<O> Choose(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return _func(a, b);
        }
    }
}
