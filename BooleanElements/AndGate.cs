using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class AndGate : BaseCompViewModel
    {

        #region Constructors

        public AndGate() : base()
        {
        }

        public AndGate(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override CompInfo GetCompInfo() => new CompInfo(this, "AND", "Logical Operations", "Boolean");

        public override void Compute()
        {
            bool a = this.ChildElementManager.GetData(nodeBlock, false);
            bool b = this.ChildElementManager.GetData(nodeBlock1, false);
            this.ChildElementManager.SetData((a && b), nodeBlock2);
        }
        
        private BooleanDataNode nodeBlock;
        private BooleanDataNode nodeBlock1;
        private BooleanDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new BooleanDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");
            
            nodeBlock1 = new BooleanDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
