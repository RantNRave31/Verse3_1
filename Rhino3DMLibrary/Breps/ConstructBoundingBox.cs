using System;
using System.Windows;
using Verse3.Elements;
using Rhino;
using Rhino.Geometry;
using Verse3.Components;
using Core.Nodes;

namespace Rhino3DMLibrary
{
    public class ConstructBoundingBox : BaseCompViewModel
    {
        public ConstructBoundingBox() : base()
        {
        }
        public ConstructBoundingBox(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            Rhino.Geometry.Point point1 = (Rhino.Geometry.Point)this.ChildElementManager.GetData<GeometryBase>(nodeBlockX, default);
            Rhino.Geometry.Point point2 = (Rhino.Geometry.Point)this.ChildElementManager.GetData<GeometryBase>(nodeBlockY, default);
   
            if (point1 != null && point2 != null)
            {
                BoundingBox box = new BoundingBox(point1.Location, point2.Location);
                GeometryBase geo = box.ToBrep();
                this.ChildElementManager.SetData<GeometryBase>(geo, nodeBlockResult);
            }

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Bounding Box", "Basic", "Breps");

        private RhinoGeometryDataNode nodeBlockX;
        private RhinoGeometryDataNode nodeBlockY;
        private RhinoGeometryDataNode nodeBlockResult;
        public override void Initialize()
        {
            nodeBlockX = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockX, "Point 2");

            nodeBlockY = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Point 2");

            nodeBlockResult = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockResult, "Bounding Box", true);
        }
    }
}
