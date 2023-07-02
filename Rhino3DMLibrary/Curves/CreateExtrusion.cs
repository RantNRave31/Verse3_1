using System;
using System.Windows;
using Rhino;
using Rhino.Geometry;
using Verse3.Nodes;
using Verse3.Components;
using Core.Nodes;

namespace Rhino3DMLibrary
{
    public class CreateExtrusion : BaseCompViewModel
    {
        public CreateExtrusion() : base()
        {
        }
        public CreateExtrusion(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            Rhino.Geometry.Curve curve = (Rhino.Geometry.Curve)this.ChildElementManager.GetData<GeometryBase>(nodeBlockX, default);

            double height = this.ChildElementManager.GetData<double>(nodeBlockY, 50);
            bool cap = this.ChildElementManager.GetData<bool>(nodeBlockZ, true);
            if (curve != null)
            {

                GeometryBase geo = Extrusion.Create(curve, height, cap);
                this.ChildElementManager.SetData<GeometryBase>(geo, nodeBlockResult);
            }

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Create Extrusion", "Operations", "Curve");

        private RhinoGeometryDataNode nodeBlockX;
        private NumberDataNode nodeBlockY;
        private BooleanDataNode nodeBlockZ;
        private RhinoGeometryDataNode nodeBlockResult;
        public override void Initialize()
        {
            nodeBlockX = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockX, "Curve");

            nodeBlockY = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Height");

            nodeBlockZ = new BooleanDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockZ, "Is Capped");


            nodeBlockResult = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockResult, "Extruded surface", true);
        }
    }
}
