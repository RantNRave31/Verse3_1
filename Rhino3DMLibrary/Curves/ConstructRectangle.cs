﻿using System;
using System.Windows;
using Rhino;
using Rhino.Geometry;
using Verse3.Nodes;
using Verse3.Components;
using Core.Nodes;

namespace Rhino3DMLibrary
{
    public class ConstructRectangle : BaseCompViewModel
    {
        public ConstructRectangle() : base()
        {
        }
        public ConstructRectangle(int x, int y) : base(x, y)
        {
        }

        public override void Compute()
        {
            ((Rhino.Geometry.PlaneSurface)this.ChildElementManager.GetData<GeometryBase>(nodeBlockX, default)).TryGetPlane(out Rhino.Geometry.Plane plane);
            double width = this.ChildElementManager.GetData<double>(nodeBlockY, 20);
            double height = this.ChildElementManager.GetData<double>(nodeBlockZ, 10);
            if (plane.IsValid)
            {
                Rectangle3d rectangle = new Rectangle3d(plane, width, height);
                GeometryBase geo = rectangle.ToNurbsCurve();
                this.ChildElementManager.SetData<GeometryBase>(geo, nodeBlockResult);
            }

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Construct Rectangle", "Line", "Curve");

        private RhinoGeometryDataNode nodeBlockX;
        private NumberDataNode nodeBlockY;
        private NumberDataNode nodeBlockZ;
        private RhinoGeometryDataNode nodeBlockResult;
        public override void Initialize()
        {
            nodeBlockX = new RhinoGeometryDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockX, "Plane");

            nodeBlockY = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Width");

            nodeBlockY = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlockY, "Height");

            nodeBlockResult = new RhinoGeometryDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlockResult, "Rectangle", true);
        }
    }
}
