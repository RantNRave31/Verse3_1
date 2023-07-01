using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using System.Collections;
using System.Collections.Generic;
using GKYU.TranslationLibrary.Translators;

namespace TextLibrary
{
    public class StreamLineScannerComponent : BaseCompViewModel
    {
        private int currentLine = 0;
        internal string _inputFileName = "";
        internal string _currentInput = "";
        StreamLineScanner _scanner;
        #region Constructors

        public StreamLineScannerComponent() : base()
        {
        }

        public StreamLineScannerComponent(int x, int y) : base(x, y)
        {
        }
        #endregion
        public override void Compute()
        {
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "StreamLineScaner", "Inputs", "Translation");

        private GenericEventNode eventIn;
        private GenericEventNode eventTrue;
        private BooleanDataNode state;
        internal TextBoxElementViewModel textBoxBlock = new TextBoxElementViewModel();
        internal IntegerDataNode nodeBlock;
        internal TextDataNode nodeBlock1;
        public override void Initialize()
        {
            eventIn = new GenericEventNode(this, NodeType.Input);
            eventIn.NodeEvent += eventIn_NodeEvent;
            this.ChildElementManager.AddEventInputNode(eventIn, "Read");

            eventTrue = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(eventTrue);

            state = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(state, "EOF");

            base.titleTextBlock.TextRotation = 0;

            textBoxBlock = new TextBoxElementViewModel();
            textBoxBlock.InputText = "";
            textBoxBlock.ValueChanged += TextBoxBlock_OnValueChanged;
            this.ChildElementManager.AddElement(textBoxBlock);

            nodeBlock = new IntegerDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock, "Line");
            
            nodeBlock1 = new TextDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock1, "Output", true);
        }

        private void TextBoxBlock_OnValueChanged(object? sender, EventArgs e)
        {
            this._inputFileName = textBoxBlock.InputText;
        }
        private void eventIn_NodeEvent(IEventNode container, EventArgData e)
        {
            if (_scanner == null)
            {
                _scanner = new StreamLineScanner(new System.IO.StreamReader(_inputFileName));
            }
            this.ChildElementManager.SetData<bool>(_scanner.EndOfFile, state);
            this.ChildElementManager.SetData<int>(currentLine++, nodeBlock);
            _currentInput = _scanner.Read();
            this.ChildElementManager.SetData<string>(_currentInput, nodeBlock1);
            this.previewTextBlock.DisplayedText = _currentInput.ToString();
            ComputationCore.Compute(this, false);
        }
    }
}

