using Core;
using IntegerLibrary.Utilities;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;


namespace IntegerLibrary.AdvancedOperations
{
    public class BinaryInterleave : BaseCompViewModel
    {


        #region Constructors

        public BinaryInterleave() : base()
        {

        }

        public BinaryInterleave(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            int x = ChildElementManager.GetData(nodeBlock, 1);
            int y = ChildElementManager.GetData(nodeBlock1, 1);
            int result = (int)x.Interleave2D(y);
            ChildElementManager.SetData(result, nodeBlock2);

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Interleave(x,y)", "Advanced Operations", "Integer");


        private IntegerDataNode nodeBlock;
        private IntegerDataNode nodeBlock1;
        private IntegerDataNode nodeBlock2;

        public override void Initialize()
        {
            nodeBlock = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "x");

            nodeBlock1 = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock1, "y");

            nodeBlock2 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);

        }
    }
}
