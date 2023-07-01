﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;
using TextElement = Verse3.Elements.TextElementViewModel;

namespace MathLibrary
{
    public class RandomNumber : BaseCompViewModel
    {

        #region Constructors

        public RandomNumber() : base()
        {
        }

        public RandomNumber(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData<double>(nodeBlock, int.MinValue);
            double b = this.ChildElementManager.GetData<double>(nodeBlock1, int.MaxValue);
            Random rd = new Random();
            this.ChildElementManager.SetData<double>(rd.Next((int)b, (int)a), nodeBlock3);
        }


        public override CompInfo GetCompInfo() => new CompInfo(this, "Random", "Miscellaneous", "Double");
        
        private NumberDataNode nodeBlock;
        private NumberDataNode nodeBlock1;
        private NumberDataNode nodeBlock3;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            
            this.ChildElementManager.AddDataInputNode(nodeBlock, "Maximum");
            
            nodeBlock1 = new NumberDataNode(this, NodeType.Input);
            nodeBlock1.Width = 50;
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "Minimum");

            nodeBlock3 = new NumberDataNode(this, NodeType.Output);
            nodeBlock3.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock3, "Random between the limits");
        }
    }
}
