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
    public class StreamWriterComponent : BaseCompViewModel
    {
        private int _currentOutputPosition = 0;
        private int _currentOutputLinePosition = 0;
        internal string _outputFileName = "";
        internal int _currentOutput = 0;
        internal string _currentOutputLine = "";
        StreamWriter _stream;
        #region Constructors

        public StreamWriterComponent() : base()
        {
        }

        public StreamWriterComponent(int x, int y) : base(x, y)
        {
        }
        #endregion
        public override void Compute()
        {
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "StreamWriter", "Outputs", "Translation");

        private GenericEventNode eventOpen;
        private GenericEventNode eventWrite;
        private GenericEventNode eventWriteLine;
        private GenericEventNode eventClose;
        internal IntegerDataNode inputNode;
        internal TextDataNode inputLineNode;
        private BooleanDataNode eventTrue;
        internal TextBoxElementViewModel fileNameNode;
        internal IntegerDataNode positionNode;
        internal IntegerDataNode lineNode;
        public override void Initialize()
        {
            eventOpen = new GenericEventNode(this, NodeType.Input);
            eventOpen.NodeEvent += eventOpen_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventOpen, "Open");

            eventWrite = new GenericEventNode(this, NodeType.Input);
            eventWrite.NodeEvent += eventWrite_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventWrite, "Write");

            eventWriteLine = new GenericEventNode(this, NodeType.Input);
            eventWriteLine.NodeEvent += eventWriteLine_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventWriteLine, "Write Line");

            eventClose = new GenericEventNode(this, NodeType.Input);
            eventClose.NodeEvent += eventClose_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventClose, "Close");

            inputNode = new IntegerDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(inputNode, "Input");

            inputLineNode = new TextDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(inputLineNode, "Input Line");

            eventTrue = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(eventTrue, "Opened");

            base.titleTextBlock.TextRotation = 0;

            fileNameNode = new TextBoxElementViewModel();
            fileNameNode.InputText = "";
            fileNameNode.ValueChanged += TextBoxBlock_OnValueChanged;
            this.ChildElementManager.AddElement(fileNameNode);

            positionNode = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(positionNode, "Position");

            lineNode = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(lineNode, "Line");

        }

        private void TextBoxBlock_OnValueChanged(object? sender, EventArgs e)
        {
            this._outputFileName = fileNameNode.InputText;
        }
        private void eventOpen_NodeEvent(IEventNode container, EventArgData e)
        {
            try
            {
                if (_stream == null)
                {
                    _stream = new System.IO.StreamWriter(_outputFileName);
                }
                this.previewTextBlock.DisplayedText = "File Opened";
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Unauthorized Access";
            }
            ComputationCore.Compute(this, false);
        }
        private void eventWrite_NodeEvent(IEventNode container, EventArgData e)
        {
            if (_stream != null)
            {
                _stream.Write(_currentOutput);
                this.ChildElementManager.SetData<int>(_currentOutputPosition++, positionNode);
                this.ChildElementManager.SetData<int>(_currentOutput, inputNode);
            }
            this.previewTextBlock.DisplayedText = _currentOutput.ToString();
            ComputationCore.Compute(this, false);
        }
        private void eventWriteLine_NodeEvent(IEventNode container, EventArgData e)
        {
            if (_stream != null)
            {
                _stream.WriteLine(_currentOutputLine);
                this.ChildElementManager.SetData<int>(_currentOutputPosition++, positionNode);
                this.ChildElementManager.SetData<int>(_currentOutput, inputNode);
            }
            this.previewTextBlock.DisplayedText = _currentOutputLine.ToString();
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
                }
                this.previewTextBlock.DisplayedText = "File Closed";
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Error Closing File Stream";
            }
            ComputationCore.Compute(this, false);
        }
    }
}

