using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Collections.Octrees
{
    public enum NodeState : byte { Empty = 0, Leaf = 1, Partial = 2 }
    public interface IOctreeNode<T>
        : IAABB
    {
        int Depth { get; }
        T Data { get; }
        double NodeSize { get; }

        IList<IOctreeNode<T>> Children { get; }

        bool IsEmpty();
        bool IsLeaf();
        bool IsPartial();
        //IOctreeNode<T, O> Clone(IList<IOctreeNode<T, O>> children);

        IOctreeNode<T> Split();

        NodeState State { get; }

        //IOctreeNode<T, O> Clone(NodeState state, T data = default(T));

    }
    public class Octree<T>
    {
        public class Node
            : AABB
            , IOctreeNode<T>
        {
            public int Depth { get; private set; }
            public T Data { get; private set; }
            public double NodeSize { get; private set; }

            public IList<IOctreeNode<T>> Children { get; private set; }

            public Node(Vector3D center, double nodeSize, int depth, T data = default(T))
                : base(center, nodeSize)
            {
                NodeSize = nodeSize;
                Depth = depth;
                Data = data;
                Children = Enumerable.Empty<IOctreeNode<T>>().ToList();
                State = CalculateNodeState();
            }


            public Node(Vector3D center, double nodeSize, int depth, IList<IOctreeNode<T>> children)
                : base(center, nodeSize)
            {
                NodeSize = nodeSize;
                Depth = depth;
                Data = default(T);
                Children = children ?? Enumerable.Empty<IOctreeNode<T>>().ToList();
                State = CalculateNodeState();
            }


            private NodeState CalculateNodeState()
            {
                if (!EqualityComparer<T>.Default.Equals(Data, default(T)))
                {
                    return NodeState.Leaf;
                }
                if (!Children.Any())
                {
                    return NodeState.Empty;
                }
                return NodeState.Partial;
            }

            public bool IsEmpty()
            {
                return State == NodeState.Empty;
            }

            public bool IsLeaf()
            {
                return State == NodeState.Leaf;
            }

            public bool IsPartial()
            {
                return State == NodeState.Partial;
            }

            public IOctreeNode<T> Split()
            {
                var newSize = NodeSize / 2.0;
                var half = NodeSize / 4.0;
                return new Node(Center, NodeSize, Depth, new List<IOctreeNode<T>>
            {
                //top-front-right
                new Node(Center + new Vector3D(+half, +half, +half),newSize,Depth + 1, Data),
                //top-back-right
                new Node(Center + new Vector3D(-half, +half, +half),newSize,Depth + 1, Data),
                //top-back-left
                new Node(Center + new Vector3D(-half, -half, +half),newSize,Depth + 1, Data),
                //top-front-left
                new Node(Center + new Vector3D(+half, -half, +half),newSize,Depth + 1, Data),
                //bottom-front-right
                new Node(Center + new Vector3D(+half, +half, -half),newSize,Depth + 1, Data),
                //bottom-back-right
                new Node(Center + new Vector3D(-half, +half, -half),newSize,Depth + 1, Data),
                //bottom-back-left
                new Node(Center + new Vector3D(-half, -half, -half),newSize,Depth + 1, Data),
                //bottom-front-left
                new Node(Center + new Vector3D(+half, -half, -half),newSize,Depth + 1, Data)
            });
            }

            //public IOctreeNode<T, O> Clone(IList<IOctreeNode<T, O>> children)
            //{
            //    return new OctreeNode<T,O>(Center, Size, Depth, Data, children);
            //}

            public NodeState State { get; private set; }

            //public IOctreeNode<T, O> Clone(NodeState state, T data = default(T))
            //{
            //    switch (state)
            //    {
            //        case NodeState.Empty:
            //            return new OctreeNode<T,O>(Center, Size, Depth, default(T));
            //        case NodeState.Leaf:
            //            return new OctreeNode<T,O>(Center, Size, Depth, data);
            //        case NodeState.Partial:
            //            return new OctreeNode<T,O>(Center, Size, Depth, Children);
            //        default:
            //            throw new ArgumentOutOfRangeException("state");
            //    }
            //}

            public static Node Filled(Vector3D center, double nodeSize, int depth, T data)
            {
                return new Node(center, nodeSize, depth, data);
            }

            public static Node Filled(IOctreeNode<T> node, T data)
            {
                return new Node(node.Center, node.NodeSize, node.Depth, data);
            }


            public static Node Empty(Vector3D center, double nodeSize, int depth)
            {
                return new Node(center, nodeSize, depth);
            }

            public static Node Empty(IOctreeNode<T> node)
            {
                return new Node(node.Center, node.NodeSize, node.Depth);
            }


        }
        public class Visitor
        {
            public virtual void Visit(Octree<T> octree)
            {
                foreach (Node octreeNode in octree.GetAllNodes())
                {
                    this.Visit(octree.Root);
                }
            }
            public virtual void Visit(IOctreeNode<T> octreeNode)
            {

            }
        }
        public class ReportVisitor
            : Visitor
        {
            protected StreamWriter _streamWriter;
            protected string _tab;
            public ReportVisitor(StreamWriter streamWriter)
            {
                _streamWriter = streamWriter;
            }
            protected void PushTab()
            {
                _tab += "    ";
            }
            protected void PopTab()
            {
                _tab = _tab.Substring(0, _tab.Length - 4);
            }
        }
        public IOctreeNode<T> Root { get; private set; }
        public int MaxDepth { get; private set; }
        public Octree(IOctreeNode<T> root, int maxDepth = 5)
        {
            Root = root;
            MaxDepth = maxDepth;
        }


        //public Octree<T> Interset(Func<IOctreeNode<T, O>, bool> test, int maxLevel)
        //{
        //    return new Octree<T>(IntersectNode(Root, test, maxLevel));
        //}


        public Octree<T> Subtract(Octree<T> other)
        {
            return new Octree<T>(PerformOperation(new SubtractOperation<T>(), Root, other.Root), MaxDepth);
        }

        public Octree<T> Union(Octree<T> other)
        {
            return Union(other, Selectors<T>.First);
        }

        public Octree<T> Union(Octree<T> other, INodeSelector<T> chooser)
        {
            return new Octree<T>(PerformOperation(new AdditionOperation<T>(chooser), Root, other.Root), MaxDepth);
        }

        public Octree<T> Intersect(Octree<T> other)
        {
            return Intersect(other, Selectors<T>.First);
        }

        public Octree<T> Intersect(Octree<T> other, INodeSelector<T> chooser)
        {
            return new Octree<T>(PerformOperation(new IntersectOperation<T>(chooser), Root, other.Root), MaxDepth);
        }

        public Octree<T> Intersect(Func<IOctreeNode<T>, bool> func, T data)
        {

            return new Octree<T>(Intersect(Root, func, MaxDepth, data));
        }

        private IOctreeNode<T> Intersect(IOctreeNode<T> node, Func<IOctreeNode<T>, bool> func, int maxDepth, T data)
        {
            if (func(node))
            {
                if (node.Depth >= maxDepth) return Node.Filled(node, data);
                switch (node.State)
                {
                    case NodeState.Empty:
                        return new Node(node.Center, node.NodeSize, node.Depth, node.Split().Children.Select(c => Intersect(c, func, maxDepth, data)).ToList());
                    case NodeState.Leaf:
                        return node;
                    case NodeState.Partial:
                        return new Node(node.Center, node.NodeSize, node.Depth, node.Children.Select(c => Intersect(c, func, maxDepth, data)).ToList());
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return node;
        }




        private IOctreeNode<T> PerformOperation(INodeOperation<T> operation, IOctreeNode<T> a, IOctreeNode<T> b)
        {
            return operation.Run(a, b);
        }

        //private IOctreeNode<T, O> UnionNode(IOctreeNode<T, O> a, IOctreeNode<T, O> b, INodeChooser<T> chooser)
        //{
        //    // CASE 1
        //    if (a.IsEmpty() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    // CASE 2
        //    if (a.IsEmpty() && b.IsLeaf())
        //    {
        //        return b;
        //    }
        //    //CASE 3
        //    if (a.IsEmpty() && b.IsPartial())
        //    {
        //        return b;
        //    }
        //    //CASE 4
        //    if (a.IsLeaf() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    //CASE 5
        //    if (a.IsLeaf() && b.IsLeaf())
        //    {
        //        return chooser.Choose(a, b);
        //    }
        //    //CASE 6
        //    if (a.IsLeaf() && b.IsPartial())
        //    {
        //        return new OctreeNode<T,O>(a.Center, a.Size, a.Depth, a.Split().Zip(b.Children, (childA, childb) => UnionNode(childA, childb, chooser)).ToList());
        //    }
        //    //CASE 7
        //    if (a.IsPartial() && b.IsEmpty())
        //    {
        //        return a;
        //    }
        //    //CASE 8
        //    if (a.IsPartial() && b.IsLeaf())
        //    {
        //        return new OctreeNode<T,O>(a.Center, a.Size, a.Depth, a.Children.Zip(b.Split(), (childA, childb) => UnionNode(childA, childb, chooser)).ToList());
        //    }
        //    //CASE 9
        //    if (a.IsPartial() && b.IsPartial())
        //    {
        //        return new OctreeNode<T,O>(a.Center,a.Size,a.Depth,a.Children.Zip(b.Children,(childA, childb) => UnionNode(childA,childb,chooser)).ToList());
        //    }

        //}
        //private IOctreeNode<T, O> IntersectNode(IOctreeNode<T, O> node, Func<IOctreeNode<T, O>, bool> test, int maxLevel)
        //{
        //    if (test(node))
        //    {
        //        switch (node.State)
        //        {
        //            case NodeState.Empty:
        //                return node.Clone(CreateChildren(node).Where(test));
        //            case NodeState.Leaf:
        //                return node;
        //            case NodeState.Partial:
        //                return node.Clone(node.Children.())
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        return node;
        //    }
        //}

        //public Octree<T> Union(Octree<T> other)
        //{

        //}

        //public Octree<T> Do(Func<OctreeNode<T,O>, NodeState> func)
        //{
        //    return new Octree<T>();
        //}

        //public static Octree<T> FromImages(params Bitmap[] images)
        //{
        //    //images.FirstOrDefault().GetPixel()
        //}



        public static Octree<T> Empty(double size, int maxDepth = 5)
        {
            return new Octree<T>(Node.Empty(Vector3D.Zero, size, 0), maxDepth);
        }

        public IEnumerable<IOctreeNode<T>> GetAllNodes()
        {
            return GetAllNodes(Root);
        }

        public static IEnumerable<IOctreeNode<T>> GetAllNodes(IOctreeNode<T> node)
        {
            yield return node;
            if (node.IsPartial())
            {
                foreach (var child in node.Children)
                {
                    foreach (var c in GetAllNodes(child))
                    {
                        yield return c;
                    }
                }
            }
        }
    }
}
