﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;


namespace MathLibrary
{
    public class BinomialEquation : BaseCompViewModel
    {


        #region Constructors

        public BinomialEquation() : base()
        {

        }

        public BinomialEquation(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData(nodeBlock, 1);
            double n = this.ChildElementManager.GetData(nodeBlock1, 1);

            this.ChildElementManager.SetData(MathUtils.BinomialEquation(a, n), nodeBlock2);

        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Binomial(a,n)", "Advanced Operations", "Double");

       
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private TextDataNode nodeBlock2;

        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);         
            this.ChildElementManager.AddDataInputNode(nodeBlock, "a");

            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "n");

            nodeBlock2 = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Equation", true);

        }
    }
}
