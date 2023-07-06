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
    public class StreamReaderComponent : BaseCompViewModel
    {
        private bool _eof;
        private int currentPosition = 0;
        private int currentLinePosition = 0;
        internal Stream _inputStream = null;
        internal int _currentOutput = -1;
        internal string _currentOutputLine = "";
        StreamReader _streamReader;
        #region Constructors

        public StreamReaderComponent() : base()
        {
        }

        public StreamReaderComponent(int x, int y) : base(x, y)
        {
        }
        private bool CreateStreamReader()
        {
            _eof = true;
            try
            {
                _inputStream = this.ChildElementManager.GetData(streamDataNode, null);
                if (_inputStream == null)
                {
                    this.previewTextBlock.DisplayedText = "Input Stream == null";
                    return false;
                }
                if (_streamReader == null)
                {
                    _streamReader = new System.IO.StreamReader(_inputStream);
                    _eof = _streamReader.EndOfStream;
                }
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Error Opening File";
            }
            return !_eof;
        }
        #endregion
        public override void Compute()
        {
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "StreamReader", "Inputs", "Translation");

        private GenericEventNode eventOpen;
        private GenericEventNode eventRead;
        private GenericEventNode eventReadLine;
        private GenericEventNode eventClose;
        private GenericEventNode eventWrite;
        private StreamDataNode streamDataNode;

        private BooleanDataNode eofNode;
        internal IntegerDataNode positionNode;
        internal IntegerDataNode lineNode;
        internal IntegerDataNode outputNode;
        internal TextDataNode outputLineNode;
        public override void Initialize()
        {
            eventOpen = new GenericEventNode(this, NodeType.Input);
            eventOpen.NodeEvent += eventOpen_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventOpen, "Open");

            eventRead = new GenericEventNode(this, NodeType.Input);
            eventRead.NodeEvent += eventRead_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventRead, "Read");

            eventReadLine = new GenericEventNode(this, NodeType.Input);
            eventReadLine.NodeEvent += eventReadLine_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventReadLine, "ReadLine");

            eventClose = new GenericEventNode(this, NodeType.Input);
            eventClose.NodeEvent += eventClose_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventClose, "Close");

            eventWrite = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(eventWrite, "Write");

            eofNode = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(eofNode, "EOF");

            base.titleTextBlock.TextRotation = 0;

            streamDataNode = new StreamDataNode(this, NodeType.Input);
            this.ChildElementManager.AddDataInputNode(streamDataNode, "InputStream");

            positionNode = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(positionNode, "Position");
            
            lineNode = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(lineNode, "Line#");

            outputNode = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(outputNode, "Output");
            
            outputLineNode = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(outputLineNode, "Output Line", true);
        }

        private void eventOpen_NodeEvent(IEventNode container, EventArgData e)
        {
            _eof = true;
            CreateStreamReader();
            //_inputStream = ChildElementManager.GetData(streamDataNode);
            if (_streamReader != null)
            {
                this.previewTextBlock.DisplayedText = "File Opened";
                ChildElementManager.SetData(_streamReader, streamDataNode);
                eventOpen.EventOccured(new EventArgData(true));
            }
            else
            {
                this.ChildElementManager.SetData<bool>(_eof, eofNode);
            }
            ComputationCore.Compute(this, false);
        }
        private void eventRead_NodeEvent(IEventNode container, EventArgData e)
        {
            if (_streamReader != null && !_streamReader.EndOfStream)
            {
                _currentOutput = _streamReader.Read();
                this.ChildElementManager.SetData<int>(_currentOutput, outputNode);
                this.ChildElementManager.SetData<int>(currentPosition++, positionNode);
                eventRead.EventOccured(new EventArgData(true));
            }
            else
            {
                _eof = true;
                this.ChildElementManager.SetData<bool>(_eof, eofNode);
            }
            this.previewTextBlock.DisplayedText = _currentOutput.ToString();
            ComputationCore.Compute(this, false);
        }
        private void eventReadLine_NodeEvent(IEventNode container, EventArgData e)
        {
            if (_streamReader != null && !_streamReader.EndOfStream)
            {
                _currentOutputLine = _streamReader.ReadLine();
                this.ChildElementManager.SetData<string>(_currentOutputLine, outputLineNode);
                this.ChildElementManager.SetData<int>(currentLinePosition++, lineNode);
                eventReadLine.EventOccured(new EventArgData(true));
            }
            else
            {
                _eof = true;
                this.ChildElementManager.SetData<bool>(_eof, eofNode);
            }
            this.previewTextBlock.DisplayedText = _currentOutputLine.ToString();
            ComputationCore.Compute(this, false);
        }
        private void eventClose_NodeEvent(IEventNode container, EventArgData e)
        {
            try
            {
                if (_streamReader != null)
                {
                    _eof = true;
                    _streamReader.Close();
                    _streamReader = null;
                    this.ChildElementManager.SetData<bool>(_eof, eofNode);
                    this.previewTextBlock.DisplayedText = "File Closed";
                }
            }
            catch (System.Exception se)
            {
                this.previewTextBlock.DisplayedText = "Error Closing File Stream";
            }
            ComputationCore.Compute(this, false);
        }
    }
}

