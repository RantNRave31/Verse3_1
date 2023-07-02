using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using Core.Nodes;

namespace EventsLibrary
{
    public class ToggleBoolean : BaseCompViewModel
    {
        internal bool? _value = false;

        #region Constructors

        public ToggleBoolean() : base()
        {
        }

        public ToggleBoolean(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            if (_value.HasValue)
            {
                this.ChildElementManager.SetData<bool>(_value.Value, nodeBlock2);
                toggleBlock.DisplayedText = _value.ToString();
            }
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "BOOL", "Types", "Boolean");
        
        internal ToggleElementViewModel toggleBlock = new ToggleElementViewModel();
        internal GenericEventNode nodeBlock;
        internal GenericEventNode nodeBlock1;
        internal BooleanDataNode nodeBlock2;
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;

            toggleBlock = new ToggleElementViewModel();
            toggleBlock.Value = _value;
            toggleBlock.DisplayedText = _value.ToString();
            toggleBlock.ToggleChecked += ButtonBlock_ToggleChecked;
            toggleBlock.ToggleUnchecked += ButtonBlock_ToggleUnchecked;
            toggleBlock.Width = 200;
            this.ChildElementManager.AddElement(toggleBlock);

            nodeBlock = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(nodeBlock as IEventNode, "Checked");

            nodeBlock1 = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(nodeBlock1 as IEventNode, "Unchecked");

            nodeBlock2 = new BooleanDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode<bool>(nodeBlock2, "Value");
        }

        private void ButtonBlock_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _value = true;
            ComputationCore.Compute(this, false);
            this.ChildElementManager.EventOccured(nodeBlock, new EventArgData(new DataStructure(_value)));
        }

        private void ButtonBlock_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _value = false;
            ComputationCore.Compute(this, false);
            this.ChildElementManager.EventOccured(nodeBlock1, new EventArgData(new DataStructure(_value)));
        }
    }
}
