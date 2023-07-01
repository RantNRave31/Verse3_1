using Core;
using System;
using System.Windows;
using Verse3.Components;
using Verse3.Nodes;

namespace TranslationLibrary.Scanners
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

        public override CompInfo GetCompInfo() => new CompInfo(this, "Scanner", "Scanners", "Translation");

        public override void Compute()
        {
            ChildElementManager.SetData(false, nodeBlock);
            int a = ChildElementManager.GetData(nodeBlock, 0);
            this.ChildElementManager.SetData<int>(a, nodeBlock2);
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
            this.ChildElementManager.AddEventInputNode(eventIn);

            eventOut = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(eventOut, "True");

            state = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(state, "Next", true);

            nodeBlock = new IntegerDataNode(this, NodeType.Input);
            ChildElementManager.AddDataInputNode(nodeBlock, "Input");

            nodeBlock2 = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock2, "Output", true);
        }
        private void eventIn_NodeEvent(IEventNode container, EventArgData e)
        {           
            this.ChildElementManager.SetData<bool>(_state, state);
            ComputationCore.Compute(this, false);
        }
    }
}
