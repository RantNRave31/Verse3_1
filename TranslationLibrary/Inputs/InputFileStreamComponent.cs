using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using System.Collections;
using System.Collections.Generic;
using GKYU.TranslationLibrary.Translators;
using System.IO;
using Core.Nodes;
using System.Linq;

namespace TextLibrary
{
    public class InputFileStreamComponent : BaseCompViewModel
    {
        internal string _inputFileName = "";
        internal Stream _stream = null;
        private GenericEventNode eventOpen;
        private GenericEventNode eventClose;
        private StreamDataNode outputNode;
        private GenericEventNode eventOut;
        #region Constructors

        public InputFileStreamComponent() : base()
        {
        }

        public InputFileStreamComponent(int x, int y) : base(x, y)
        {
        }
        #endregion
        public override void Compute()
        {
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "Input File Stream", "Inputs", "Translation");

        internal TextBoxElementViewModel fileNameNode = new TextBoxElementViewModel();
        public override void Initialize()
        {

            eventOpen = new GenericEventNode(this, NodeType.Input);
            eventOpen.NodeEvent += eventOpen_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventOpen, "Open");

            eventClose = new GenericEventNode(this, NodeType.Input);
            eventClose.NodeEvent += eventClose_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventClose, "Close");

            base.titleTextBlock.TextRotation = 0;
            fileNameNode = new TextBoxElementViewModel();
            fileNameNode.InputText = "";
            fileNameNode.ValueChanged += TextBoxBlock_OnValueChanged;
            this.ChildElementManager.AddElement(fileNameNode);

            eventOut = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(eventOut, "Opened");

            outputNode = new StreamDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(outputNode, "Output Stream");
        }

        private void TextBoxBlock_OnValueChanged(object? sender, EventArgs e)
        {
            this._inputFileName = fileNameNode.InputText;
            if(!File.Exists(this._inputFileName))
            {
                this.previewTextBlock.DisplayedText = "File Not Found";
            }
        }
        private void eventOpen_NodeEvent(IEventNode container, EventArgData e)
        {
            bool bFileExists = false;
            try
            {
                bFileExists = File.Exists(_inputFileName);
                if (!bFileExists)
                {
                    this.previewTextBlock.DisplayedText = "File Not Found";
                }
                if (_stream == null)
                {
                    _stream = new FileStream(this._inputFileName, FileMode.Open);
                    this.previewTextBlock.DisplayedText = "File Opened";
                    this.ChildElementManager.SetData(_stream, outputNode);
                    eventOut.EventOccured(e);
                }
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Error Opening File";
            }
            ComputationCore.Compute(this, false);
        }
        private void eventClose_NodeEvent(IEventNode container, EventArgData e)
        {
            try
            {
                if (_stream != null)
                {
                    _stream.Close();
                    _stream = null;
                    this.previewTextBlock.DisplayedText = "File Closed";
                    this.ChildElementManager.SetData(_stream, outputNode);
                    eventOut.EventOccured(e);
                }
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Error Closing File";
            }
            ComputationCore.Compute(this, false);
        }
    }
}

