using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class DoubleSubtraction : BaseCompViewModel
    {

        #region Constructors

        public DoubleSubtraction() : base()
        {
        }

        public DoubleSubtraction(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 0);
            double b = this.ChildElementManager.GetData<double>(nodeBlock1, 0);
            this.ChildElementManager.SetData<double>((a - b), nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "SUB", "Basic Operations", "Double");
        
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "B");
            
            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
