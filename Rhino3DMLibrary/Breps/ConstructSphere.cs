using Core;
using System;
using System.Windows;
using Rhino;
using Rhino.Geometry;
using Verse3.Nodes;
using Verse3.Components;

namespace Rhino3DMLibrary
{
    public class ConstructSphere : BaseCompViewModel
    {
        public ConstructSphere() : base()
        {
        }
        public ConstructSphere(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            Rhino.Geometry.Point point1 = (Rhino.Geometry.Point)this.ChildElementManager.GetData<GeometryBase>(nodeBlockX, default);
            double radius = this.ChildElementManager.GetData<double>(nodeBlockY, 0);
            if (point1 != null)
            {
                Sphere sphere = new Sphere(point1.Location, radius);
                GeometryBase geo = sphere.ToBrep();
                this.ChildElementManager.SetData<GeometryBase>(geo, nodeBlockResult);
            }

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Sphere", "Basic", "Breps");

        private RhinoGeometryDataNode nodeBlockX;
        private NumberDataNode nodeBlockY;
        private RhinoGeometryDataNode nodeBlockResult;
        public override void Initialize()
        {
            nodeBlockX = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockX, "Point");

            nodeBlockY = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Radius");

            nodeBlockResult = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockResult, "Sphere", true);
        }
    }
}
