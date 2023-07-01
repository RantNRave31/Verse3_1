using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;
using TextElement = Verse3.Elements.TextElementViewModel;

namespace IntegerLibrary.Conversions
{
    public class Boolean8ToInteger : BaseCompViewModel
    {

        #region Constructors

        public Boolean8ToInteger() : base()
        {
        }

        public Boolean8ToInteger(int x, int y) : base(x, y)
        {
        }

        #endregion

        private BooleanDataNode nodeBlock;
        private BooleanDataNode nodeBlock1;
        private BooleanDataNode nodeBlock2;
        private BooleanDataNode nodeBlock3;
        private BooleanDataNode nodeBlock4;
        private BooleanDataNode nodeBlock5;
        private BooleanDataNode nodeBlock6;
        private BooleanDataNode nodeBlock7;
        private IntegerDataNode nodeBlock8;

        public override void Initialize()
        {
            nodeBlock = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock2, "C");

            nodeBlock3 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock3, "D");

            nodeBlock4 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock4, "E");

            nodeBlock5 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock5, "F");

            nodeBlock6 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock6, "G");

            nodeBlock7 = new BooleanDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock7, "H");

            nodeBlock8 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock8, "Result", true);

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "BOOL8->INT", "Advanced Operations", "Integer");

        public override void Compute()
        {
            bool a = ChildElementManager.GetData(nodeBlock, false);
            bool b = ChildElementManager.GetData(nodeBlock1, false);
            bool c = ChildElementManager.GetData(nodeBlock2, false);
            bool d = ChildElementManager.GetData(nodeBlock3, false);
            bool e = ChildElementManager.GetData(nodeBlock4, false);
            bool f = ChildElementManager.GetData(nodeBlock5, false);
            bool g = ChildElementManager.GetData(nodeBlock6, false);
            bool h = ChildElementManager.GetData(nodeBlock7, false);
            int n = a ? 1 : 0;
            n += (b ? 1 : 0) << 1;
            n += (c ? 1 : 0) << 2;
            n += (d ? 1 : 0) << 3;
            n += (e ? 1 : 0) << 4;
            n += (f ? 1 : 0) << 5;
            n += (g ? 1 : 0) << 6;
            n += (h ? 1 : 0) << 7;
            ChildElementManager.SetData(n, nodeBlock8);

        }
    }
}
