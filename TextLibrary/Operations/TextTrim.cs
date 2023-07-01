﻿using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace TextLibrary
{
    public class TextTrim : BaseCompViewModel
    {


        #region Constructors

        public TextTrim() : base()
        {

        }

        public TextTrim(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            string a = this.ChildElementManager.GetData(nodeBlock, "");
            a = a.Trim();
            this.ChildElementManager.SetData(a, nodeBlock2);    
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Text Trim", "Operations", "Text");

        private TextDataNode nodeBlock;
 
        private TextDataNode nodeBlock2;
        public override void Initialize()
        {
            nodeBlock = new TextDataNode(this, NodeType.Input);      
            this.ChildElementManager.AddDataInputNode(nodeBlock, "Text");

            nodeBlock2 = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Trimmed Text", true);
        }
    }
}
