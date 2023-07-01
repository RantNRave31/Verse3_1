﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace MathLibrary
{
    public class Concatenate : BaseCompViewModel
    {

        #region Constructors

        public Concatenate() : base()
        {
            
        }

        public Concatenate(int x, int y) : base(x, y)
        {
         
        }

        #endregion

        public override void Compute()
        {
            string a = this.ChildElementManager.GetData<string>(nodeBlock, "");
            string b = this.ChildElementManager.GetData<string>(nodeBlock1, "");
            this.ChildElementManager.SetData<string>((a + b), nodeBlock2);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Concatenate", "Operations", "Text");
        
        private TextDataNode nodeBlock;
        private TextDataNode nodeBlock1;
        private TextDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock, "A");

            nodeBlock1 = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(nodeBlock1, "B");

            nodeBlock2 = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Result", true);
        }
    }
}
