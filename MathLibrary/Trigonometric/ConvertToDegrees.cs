using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary.Trigonometric
{
    public class ConvertToDegrees : BaseCompViewModel
    {



        #region Constructors

        public ConvertToDegrees() : base()
        {

        }

        public ConvertToDegrees(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            double a = ChildElementManager.GetData(nodeBlock, 0);
            ChildElementManager.SetData(180 / Math.PI * a, nodeBlock2);

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Convert to Degrees", "Conversions", "Double");

        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);

            ChildElementManager.AddDataInputNode(nodeBlock, "Radians");


            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Degrees", true);


        }
    }
}
