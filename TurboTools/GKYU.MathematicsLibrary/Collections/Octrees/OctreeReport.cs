using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Collections.Octrees
{
    public class OctreeReport<O>
        : Octree<O>.ReportVisitor
    {
        protected int _nodeCount;

        public OctreeReport(StreamWriter streamWriter)
            : base(streamWriter)
        {
            _nodeCount = 0;
        }
        public override void Visit(Octree<O> octree)
        {
            _streamWriter.WriteLine("Octree Report");
            _streamWriter.WriteLine(" -Max Depth:  ", octree.MaxDepth);
            this.Visit(octree.Root);
            _streamWriter.WriteLine("Total Nodes:  {0}", _nodeCount);
        }
        public override void Visit(IOctreeNode<O> node)
        {
            _nodeCount++;
            _streamWriter.Write(_tab + "NODE[{0},{1}][{2},{3},{4}]:  ", node.Depth, node.NodeSize, node.Center.X, node.Center.Y, node.Center.Z);
            if (node.Data != null)
            {
                _streamWriter.Write(node.Data.ToString());
            }
            _streamWriter.WriteLine();
            if (node.IsPartial())
            {
                PushTab();
                foreach (var child in node.Children)
                {
                    this.Visit(child);
                }
                PopTab();
            }
            base.Visit(node);
        }
    }
}
