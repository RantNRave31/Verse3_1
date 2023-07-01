using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;

namespace TextLibrary
{
    public class OutputStream : BaseCompViewModel
    {
        internal string _inputText = "";

        #region Constructors

        public OutputStream() : base()
        {
        }

        public OutputStream(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            this._inputText = textBoxBlock.InputText;
            this.ChildElementManager.SetData<string>(this._inputText, nodeBlock);
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "Output File Stream", "Outputs", "Translation");

        internal TextBoxElementViewModel textBoxBlock = new TextBoxElementViewModel();
        internal TextDataNode nodeBlock;
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;

            textBoxBlock = new TextBoxElementViewModel();
            textBoxBlock.InputText = "";
            textBoxBlock.ValueChanged += TextBoxBlock_OnValueChanged;
            this.ChildElementManager.AddElement(textBoxBlock);

            nodeBlock = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock, "Text");
        }
        
        private void TextBoxBlock_OnValueChanged(object? sender, EventArgs e)
        {
            this._inputText = textBoxBlock.InputText;
            ComputationPipeline.ComputeComputable(this);
        }
    }
}
