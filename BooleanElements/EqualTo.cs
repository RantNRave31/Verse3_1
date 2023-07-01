﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class EqualTo : BaseCompViewModel
    {

        

        #region Constructors

        public EqualTo() : base()
        {
        }

        public EqualTo(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override CompInfo GetCompInfo() => new CompInfo(this, "Equal To", "Logical Operations", "Double");

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 0);
            double b = this.ChildElementManager.GetData<double>(nodeBlock1, 0);
            this.ChildElementManager.SetData<bool>((a == b), nodeBlock2);
        }
        
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private BooleanDataNode nodeBlock2;

        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Equal To", true);
        }
    }
}
