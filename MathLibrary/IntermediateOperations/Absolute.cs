﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Absolute : BaseCompViewModel
    {

        #region Constructors

        public Absolute() : base()
        {
           
        }

        public Absolute(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, 1);
         
            this.ChildElementManager.SetData<double>((Math.Abs(a)), nodeBlock2);

        }


        public override CompInfo GetCompInfo() => new CompInfo(this, "ABS(A,B)", "Intermidiate Operations", "Double");


        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

 

            nodeBlock2 = new NumberDataNode(this, NodeType.Output);
            nodeBlock2.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result");

        }
    }
}
