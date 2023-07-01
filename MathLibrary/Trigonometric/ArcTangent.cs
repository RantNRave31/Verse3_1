using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary.Trigonometric
{
    public class ArcTangent : BaseCompViewModel
    {


        #region Constructors

        public ArcTangent() : base()
        {

        }

        public ArcTangent(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            double a = ChildElementManager.GetData(nodeBlock, 0);
            ChildElementManager.SetData(Math.Atan(a), nodeBlock2);

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Arc Tangent", "Trigonometry", "Double");


        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);

            ChildElementManager.AddDataInputNode(nodeBlock, "Number");

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            nodeBlock2.Width = 50;
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Radians", true);


        }
    }
}
