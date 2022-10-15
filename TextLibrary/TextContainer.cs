﻿using Core;
using System;
using System.Windows;
using Verse3;
using Verse3.VanillaElements;

namespace TextLibrary
{
    public class TextContainer : BaseComp
    {
        internal string _inputText = "";

        public string? ElementText
        {
            get
            {
                string? name = this.GetType().FullName;
                string? viewname = this.ViewType.FullName;
                string? dataIN = _inputText;
                //string? zindex = DataViewModel.WPFControl.Content.
                
                return $"Output Value: {dataIN}";
            }
        }

        #region Constructors

        public TextContainer() : base(0, 0)
        {
        }

        public TextContainer(int x, int y, int width = 250, int height = 300) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            this._inputText = textBoxBlock.InputText;
            this.ChildElementManager.SetData<string>(this._inputText, 0);
            textBlock.DisplayedText = this.ElementText;
        }
        public override CompInfo GetCompInfo()
        {
            Type[] types = { typeof(int), typeof(int), typeof(int), typeof(int) };
            CompInfo ci = new CompInfo
            {
                ConstructorInfo = this.GetType().GetConstructor(types),
                Name = "Text Input",
                Group = "Inputs",
                Tab = "Text",
                Description = "",
                Author = "",
                License = "",
                Repository = "",
                Version = "",
                Website = ""
            };
            return ci;
        }

        internal TextElement textBlock = new TextElement();
        internal TextBoxElement textBoxBlock = new TextBoxElement();
        internal TextDataNode nodeBlock;
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;

            //sliderBlock = new SliderElement();
            //sliderBlock.Minimum = 0;
            //sliderBlock.Maximum = 100;
            //sliderBlock.Value = 50;
            //sliderBlock.ValueChanged += SliderBlock_OnValueChanged;
            //sliderBlock.Width = 200;
            //this.ChildElementManager.AddElement(sliderBlock);

            textBoxBlock = new TextBoxElement();
            textBoxBlock.InputText = "";
            textBoxBlock.ValueChanged += TextBoxBlock_OnValueChanged;
            this.ChildElementManager.AddElement(textBoxBlock);

            nodeBlock = new TextDataNode(this, NodeType.Output);
            nodeBlock.Width = 50;
            this.ChildElementManager.AddDataOutputNode(nodeBlock, "Text");

            textBlock = new TextElement();
            textBlock.DisplayedText = this.ElementText;
            textBlock.TextAlignment = TextAlignment.Left;
            this.ChildElementManager.AddElement(textBlock);
        }
        
        private void TextBoxBlock_OnValueChanged(object? sender, EventArgs e)
        {
            this._inputText = textBoxBlock.InputText;
            ComputationPipeline.ComputeComputable(this);
        }

        //private IRenderable _parent;
        //public IRenderable Parent => _parent;
        //private ElementsLinkedList<IRenderable> _children = new ElementsLinkedList<IRenderable>();
        //public ElementsLinkedList<IRenderable> Children => _children;
    }
}
