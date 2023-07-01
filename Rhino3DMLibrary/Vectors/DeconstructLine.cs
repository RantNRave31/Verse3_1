using Core;
using System;
using System.Windows;
using Verse3.Elements;
using Rhino;
using Rhino.Geometry;
using Verse3.Components;

namespace Rhino3DMLibrary
{
    public class DeconstructLine : BaseCompViewModel
    {
        public DeconstructLine() : base()
        {
        }
        public DeconstructLine(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            Rhino.Geometry.LineCurve line = (Rhino.Geometry.LineCurve)this.ChildElementManager.GetData<GeometryBase>(nodeBlockLine, default);
            if (line != null)
            {
                Rhino.Geometry.Point p1 = new Rhino.Geometry.Point(line.PointAtStart);
                Rhino.Geometry.Point p2 = new Rhino.Geometry.Point(line.PointAtEnd);
                this.ChildElementManager.SetData<GeometryBase>(p1, nodeBlockp1);
                this.ChildElementManager.SetData<GeometryBase>(p2, nodeBlockp2);
                this.previewTextBlock.DisplayedText = p1.ToString() + " to " + p2.ToString();
            }
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Deconstruct Line", "Line", "Curve");

        private RhinoGeometryDataNode nodeBlockLine;
        private RhinoGeometryDataNode nodeBlockp1;
        private RhinoGeometryDataNode nodeBlockp2;
        public override void Initialize()
        {
            nodeBlockLine = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockLine, "Line");

            nodeBlockp1 = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockp1, "Start");

            nodeBlockp2 = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockp2, "End");
        }
    }
}
