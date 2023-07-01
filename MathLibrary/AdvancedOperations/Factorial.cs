﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Factorial : BaseCompViewModel
    {


        #region Constructors

        public Factorial() : base()
        {

        }

        public Factorial(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 1);

            this.ChildElementManager.SetData<double>((MathUtils.GetFactorial(a)), nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "FAC(A)", "Advanced Operations", "Double");
       
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            nodeBlock2.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
