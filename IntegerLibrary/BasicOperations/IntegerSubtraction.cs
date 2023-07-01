using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace IntegerLibrary.BasicOperations
{
    public class IntegerSubtraction : BaseCompViewModel
    {

        #region Constructors

        public IntegerSubtraction() : base()
        {
        }

        public IntegerSubtraction(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            int a = ChildElementManager.GetData(nodeBlock, 0);
            int b = ChildElementManager.GetData(nodeBlock1, 0);
            ChildElementManager.SetData(a - b, nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "SUB(int)", "Basic Operations", "Integer");

        private IntegerDataNode nodeBlock;
        private IntegerDataNode nodeBlock1;
        private IntegerDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
