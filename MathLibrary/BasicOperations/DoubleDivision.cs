using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;
using TextElement = Verse3.Elements.TextElementViewModel;

namespace MathLibrary
{
    public class DoubleDivision : BaseCompViewModel
    {

        #region Constructors

        public DoubleDivision() : base()
        {
        }

        public DoubleDivision(int x, int y) : base(x, y)
        {
        }

        #endregion

        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private NumberDataNode nodeBlock2;
        private NumberDataNode nodeBlock3;

        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");
            
            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);

            nodeBlock3 = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock3, "Remainder");

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "DIV", "Basic Operations", "Double");

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData(nodeBlock, 0);
            double b = this.ChildElementManager.GetData(nodeBlock1, 1);
            if (b == 0) return;
            this.ChildElementManager.SetData((a / b), nodeBlock2);
            this.ChildElementManager.SetData((a % b), nodeBlock3);

        }
    }
}
