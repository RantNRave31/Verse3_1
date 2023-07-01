using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;
using TextElement = Verse3.Elements.TextElementViewModel;

namespace IntegerLibrary.BasicOperations
{
    public class IntegerDivision : BaseCompViewModel
    {

        #region Constructors

        public IntegerDivision() : base()
        {
        }

        public IntegerDivision(int x, int y) : base(x, y)
        {
        }

        #endregion

        private IntegerDataNode nodeBlock;
        private IntegerDataNode nodeBlock1;
        private IntegerDataNode nodeBlock2;
        private IntegerDataNode nodeBlock3;

        public override void Initialize()
        {
            nodeBlock = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);

            nodeBlock3 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock3, "Remainder");

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "DIV(int)", "Basic Operations", "Integer");

        public override void Compute()
        {
            int a = ChildElementManager.GetData(nodeBlock, 0);
            int b = ChildElementManager.GetData(nodeBlock1, 1);
            if (b == 0) return;
            ChildElementManager.SetData(a / b, nodeBlock2);
            ChildElementManager.SetData(a % b, nodeBlock3);

        }
    }
}
