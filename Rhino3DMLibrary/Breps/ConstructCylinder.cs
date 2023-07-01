using Core;
using System;
using System.Windows;
using Rhino;
using Rhino.Geometry;
using Verse3.Nodes;
using Verse3.Components;

namespace Rhino3DMLibrary
{
    public class ConstructCylinder : BaseCompViewModel
    {
        public ConstructCylinder() : base()
        {
        }
        public ConstructCylinder(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            ((Rhino.Geometry.ArcCurve)this.ChildElementManager.GetData<GeometryBase>(nodeBlockX, default)).TryGetCircle(out Circle circle); 
            double height = this.ChildElementManager.GetData<double>(nodeBlockY, 50);

            if (circle.IsValid)
            {
                Cylinder cylinder = new Cylinder(circle, height);
                GeometryBase geo = cylinder.ToNurbsSurface();
                this.ChildElementManager.SetData<GeometryBase>(geo, nodeBlockResult);
            }

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Cylinder", "Basic", "Breps");

        private RhinoGeometryDataNode nodeBlockX;
        private NumberDataNode nodeBlockY;

        private RhinoGeometryDataNode nodeBlockResult;
        public override void Initialize()
        {
            nodeBlockX = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockX, "Circle");

            nodeBlockY = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Height");

            nodeBlockResult = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockResult, "Cylinder", true);
        }
    }
}
