﻿using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace TextLibrary
{
    public class TextTransform : BaseCompViewModel
    {

        #region Constructors

        public TextTransform() : base()
        {
            
        }

        public TextTransform(int x, int y) : base(x, y)
        {
            
        }

        #endregion

        public override void Compute()
        {
            string a = this.ChildElementManager.GetData<string>(nodeBlock, "");
            this.ChildElementManager.SetData<string>(a.ToUpper(), nodeBlock2);
            this.ChildElementManager.SetData<string>(a.ToLower(), nodeBlock3);
        }

        public override CompInfo GetCompInfo() => new CompInfo(this, "Text Transform", "Operations", "Text");
        
        private TextDataNode nodeBlock;
        private TextDataNode nodeBlock2;
        private TextDataNode nodeBlock3;
        public override void Initialize()
        {
            nodeBlock = new TextDataNode(this, NodeType.Input);
            
            this.ChildElementManager.AddDataInputNode(nodeBlock, "Text");


            nodeBlock2 = new TextDataNode(this, NodeType.Output);
            nodeBlock2.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock2, "Upper", true);

            nodeBlock3 = new TextDataNode(this, NodeType.Output);
            nodeBlock3.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock3, "Lower");
        }
    }
}
