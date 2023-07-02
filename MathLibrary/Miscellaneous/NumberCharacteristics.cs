﻿using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class NumberCharacteristics : BaseCompViewModel
    {


        #region Constructors

        public NumberCharacteristics() : base()
        {

        }

        public NumberCharacteristics(int x, int y) : base(x, y)
        {
    
        }

        #endregion

        public override void Compute()
        {
            double a = this.ChildElementManager.GetData(nodeBlock, 1);
         
            this.ChildElementManager.SetData(MathUtils.IsPrime(a), nodeBlock1);
            this.ChildElementManager.SetData(MathUtils.IsOdd(a), nodeBlock2);
            this.ChildElementManager.SetData(!(MathUtils.IsOdd(a)), nodeBlock3);

        }


        public override CompInfo GetCompInfo() => new CompInfo(this, "Number Characteristics", "Miscellaneous", "Double");

        private NumberDataNode nodeBlock;
        private BooleanDataNode nodeBlock1;
        private BooleanDataNode nodeBlock2;
        private BooleanDataNode nodeBlock3;
        public override void Initialize()
        {
            nodeBlock = new NumberDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "Number");

            nodeBlock1 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock1, "IsPrime", true);

            nodeBlock2 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "IsOdd");

            nodeBlock3 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock3, "IsEven");

        }
    }
}
