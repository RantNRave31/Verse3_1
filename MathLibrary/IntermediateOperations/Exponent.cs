﻿using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Exponent : BaseCompViewModel
    {

        #region Constructors

        public Exponent() : base()
        {
         
        }

        public Exponent(int x, int y) : base(x, y)
        {
          
        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 1);
            double b = this.ChildElementManager.GetData<double>(nodeBlock1, 1);
            this.ChildElementManager.SetData<double>((Math.Pow(a, b)), nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "EXP(A,n)", "Intermidiate Operations", "Double");
        
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "n");

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
