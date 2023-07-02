﻿using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Contains : BaseCompViewModel
    {


        #region Constructors

        public Contains() : base()
        {
         
        }

        public Contains(int x, int y) : base(x, y)
        {
    
        }

        #endregion

        public override void Compute()
        {
            string a = this.ChildElementManager.GetData<string>(nodeBlock, "");
            string b = this.ChildElementManager.GetData<string>(nodeBlock1, "");
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
            {
                this.ChildElementManager.SetData(false, nodeBlock2);
            }
            else
            {
                this.ChildElementManager.SetData(a.Contains(b), nodeBlock2);
            }
       
        }

 

        public override CompInfo GetCompInfo() => new CompInfo(this, "Text Contains", "Operations", "Text");

        
        private TextDataNode nodeBlock;
        private TextDataNode nodeBlock1;
        private BooleanDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "Text");

            nodeBlock1 = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "Contains");

            nodeBlock2 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result");

    
        }
    }
}
