using Core;
using Core.Nodes;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace TranslationLibrary.Translators
{
    public class Parser : BaseCompViewModel
    {
        private bool _state = false;

        #region Constructors

        public Parser() : base()
        {
        }

        public Parser(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override CompInfo GetCompInfo() => new CompInfo(this, "TestParser", "Parsers", "Translation");

        public override void Compute()
        {
            ChildElementManager.SetData(false, nodeBlock);
            int a = ChildElementManager.GetData(nodeBlock, 0);
            ChildElementManager.SetData(a, nodeBlock2);
            ChildElementManager.SetData(true, nodeBlock);
        }

        private GenericEventNode eventIn;
        private GenericEventNode eventOut;
        private BooleanDataNode state;
        private IntegerDataNode nodeBlock;
        private IntegerDataNode nodeBlock2;
        public override void Initialize()
        {
            eventIn = new GenericEventNode(this, NodeType.Input);
            eventIn.NodeEvent += eventIn_NodeEvent;
            ChildElementManager.AddEventInputNode(eventIn);

            eventOut = new GenericEventNode(this, NodeType.Output);
            ChildElementManager.AddEventOutputNode(eventOut, "True");

            state = new BooleanDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(state, "Next", true);

            nodeBlock = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "Input");

            nodeBlock2 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Output", true);
        }
        private void eventIn_NodeEvent(IEventNode container, EventArgData e)
        {
            ChildElementManager.SetData(_state, state);
            ComputationCore.Compute(this, false);
        }
    }
}
