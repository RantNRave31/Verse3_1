using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Round : BaseCompViewModel
    {

        #region Constructors

        public Round() : base()
        {
          
        }

        public Round(int x, int y) : base(x, y)
        {
   
        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 1);
            this.ChildElementManager.SetData<double>((Math.Round(a)), nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "ROUND(A)", "Intermidiate Operations", "Double");

        
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
