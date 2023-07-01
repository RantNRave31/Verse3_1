﻿using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;

namespace EventsLibrary
{
    public class ButtonTrigger : BaseCompViewModel
    {
        internal double _sliderValue = 0.0;
        //private double _inputValue = 0.0;

        public string? ElementText
        {
            get
            {
                string? name = this.GetType().FullName;
                string? viewname = this.ViewType.FullName;
                string? dataIN = "";
                //if (this.ComputationPipelineInfo.IOManager.DataOutputNodes != null && this.ComputationPipelineInfo.IOManager.DataOutputNodes.Count > 0)
                //dataIN = ((NumberDataNode)this.ComputationPipelineInfo.IOManager.DataOutputNodes[0])?.DataGoo.Data.ToString();
                //string? zindex = DataViewModel.WPFControl.Content.
                
                return $"Output Value: {dataIN}";
            }
        }

        #region Properties


        #endregion

        #region Constructors

        public ButtonTrigger() : base()
        {
        }

        public ButtonTrigger(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "Button Trigger", "Basic UI", "Events");

        internal ButtonElementViewModel buttonBlock = new ButtonElementViewModel();
        internal GenericEventNode nodeBlock;
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;

            buttonBlock = new ButtonElementViewModel();
            buttonBlock.DisplayedText = "Trigger";
            buttonBlock.OnButtonClicked += ButtonBlock_OnButtonClicked;
            buttonBlock.Width = 200;
            this.ChildElementManager.AddElement(buttonBlock);

            nodeBlock = new GenericEventNode(this, NodeType.Output);
            
            this.ChildElementManager.AddEventOutputNode(nodeBlock as IEventNode);
        }

        private void ButtonBlock_OnButtonClicked(object? sender, RoutedEventArgs e)
        {
            nodeBlock.EventOccured(new EventArgData());
            ComputationCore.Compute(this, false);
        }
    }
}
