using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Collections.Octrees
{
    public interface INodeOperation<O>
    {
        IOctreeNode<O> Run(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> EmptyEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> EmptyLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> EmptyPartial(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> LeafEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> LeafLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> LeafPartial(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> PartialEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> PartialLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        IOctreeNode<O> PartialPartial(IOctreeNode<O> a, IOctreeNode<O> b);
    }
    public abstract class BaseNodeOperation<O> : INodeOperation<O>
    {
        public IOctreeNode<O> Run(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            // CASE 1
            if (a.IsEmpty() && b.IsEmpty())
            {
                return EmptyEmpty(a, b);
            }
            // CASE 2
            if (a.IsEmpty() && b.IsLeaf())
            {
                return EmptyLeaf(a, b);
            }
            //CASE 3
            if (a.IsEmpty() && b.IsPartial())
            {
                return EmptyPartial(a, b);
            }
            //CASE 4
            if (a.IsLeaf() && b.IsEmpty())
            {
                return LeafEmpty(a, b);
            }
            //CASE 5
            if (a.IsLeaf() && b.IsLeaf())
            {
                return LeafLeaf(a, b);
            }
            //CASE 6
            if (a.IsLeaf() && b.IsPartial())
            {
                return LeafPartial(a, b);
            }
            //CASE 7
            if (a.IsPartial() && b.IsEmpty())
            {
                return PartialEmpty(a, b);
            }
            //CASE 8
            if (a.IsPartial() && b.IsLeaf())
            {
                return PartialLeaf(a, b);
            }
            //CASE 9
            if (a.IsPartial() && b.IsPartial())
            {
                return PartialPartial(a, b);
            }
            throw new Exception();
        }

        public abstract IOctreeNode<O> EmptyEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> EmptyLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> EmptyPartial(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> LeafEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> LeafLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> LeafPartial(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> PartialEmpty(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> PartialLeaf(IOctreeNode<O> a, IOctreeNode<O> b);
        public abstract IOctreeNode<O> PartialPartial(IOctreeNode<O> a, IOctreeNode<O> b);
    }
    public class AdditionOperation<O> : BaseNodeOperation<O>
    {
        private readonly INodeSelector<O> _selector;

        public AdditionOperation(INodeSelector<O> selector)
        {
            _selector = selector;
        }

        public override IOctreeNode<O> EmptyEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> EmptyLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return b;
        }

        public override IOctreeNode<O> EmptyPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return b;
        }

        public override IOctreeNode<O> LeafEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> LeafLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return _selector.Choose(a, b);
        }

        public override IOctreeNode<O> LeafPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Split().Children.Zip(b.Children, Run).ToList());
        }

        public override IOctreeNode<O> PartialEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> PartialLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Children.Zip(b.Split().Children, Run).ToList());
        }

        public override IOctreeNode<O> PartialPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Children.Zip(b.Children, Run).ToList());
        }
    }
    public class IntersectOperation<O> 
        : BaseNodeOperation<O>
    {
        private readonly INodeSelector<O> _selector;

        public IntersectOperation(INodeSelector<O> selector)
        {
            _selector = selector;
        }

        public override IOctreeNode<O> EmptyEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> EmptyLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> EmptyPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> LeafEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return b;
        }

        public override IOctreeNode<O> LeafLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return _selector.Choose(a, b);
        }

        public override IOctreeNode<O> LeafPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Split().Children.Zip(b.Children, Run).ToList());
        }

        public override IOctreeNode<O> PartialEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return b;
        }

        public override IOctreeNode<O> PartialLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, b.Split().Children.Zip(a.Children, Run).ToList());
        }

        public override IOctreeNode<O> PartialPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Children.Zip(b.Children, Run).ToList());
        }
    }
    public class SubtractOperation<O>
        : BaseNodeOperation<O>
    {
        public override IOctreeNode<O> EmptyEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> EmptyLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> EmptyPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> LeafEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> LeafLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth);
        }

        public override IOctreeNode<O> LeafPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Split().Children.Zip(b.Children, Run).ToList());
        }

        public override IOctreeNode<O> PartialEmpty(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return a;
        }

        public override IOctreeNode<O> PartialLeaf(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth);
        }

        public override IOctreeNode<O> PartialPartial(IOctreeNode<O> a, IOctreeNode<O> b)
        {
            return new Octree<O>.Node(a.Center, a.NodeSize, a.Depth, a.Children.Zip(b.Children, Run).ToList());
        }
    }
}
