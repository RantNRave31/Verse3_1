using Core;
using IntegerLibrary.Inputs;
using System;
using System.Windows;
using System.Windows.Controls;
using Verse3;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using Core.Nodes;
using Verse3.CorePresentation.Workspaces;

namespace IntegerLibrary.Inputs
{
    public class IntegerContainer : BaseCompViewModel
    {
        internal int? _sliderValue = 0;

        #region Constructors

        public IntegerContainer() : base()
        {

        }

        public IntegerContainer(int x, int y) : base(x, y)
        {

        }

        #endregion

        public override void Compute()
        {
            if (_sliderValue.HasValue)
            {
                ChildElementManager.SetData(_sliderValue.Value, nodeBlock);
                previewTextBlock.DisplayedText = $"Value = {_sliderValue.Value}";
            }
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "INT32", "Types", "Integer");

        internal IntegerSliderElementViewModel sliderBlock = new IntegerSliderElementViewModel();
        internal IntegerDataNode nodeBlock;
        internal GenericEventNode nodeBlock1;
        public override void Initialize()
        {
            titleTextBlock.TextRotation = 0;

            nodeBlock1 = new GenericEventNode(this, NodeType.Output);
            ChildElementManager.AddEventOutputNode(nodeBlock1, "Changed");

            nodeBlock = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock, "Integer");

            sliderBlock = new IntegerSliderElementViewModel();
            sliderBlock.Minimum = -200;
            sliderBlock.Maximum = 200;
            sliderBlock.Value = 10;
            sliderBlock.TickFrequency = 0.001;
            sliderBlock.ValueChanged += SliderBlock_OnValueChanged;
            ChildElementManager.AddElement(sliderBlock);
        }

        private void SliderBlock_OnValueChanged(object? sender, RoutedPropertyChangedEventArgs<int> e)
        {
            _sliderValue = sliderBlock.Value;
            ComputationCore.Compute(this, false);
            ChildElementManager.EventOccured(nodeBlock1, new EventArgData(new DataStructure(_sliderValue)));
        }

        public override ContextMenu ContextMenu
        {
            get
            {
                ContextMenu contextMenu = new ContextMenu();

                //Delete
                MenuItem menuItem = new MenuItem();
                menuItem.Header = "Delete";
                menuItem.Click += (s, e) =>
                {
                    WorkspaceViewModel.StaticSelectedDataViewModel.Elements.Remove(this);
                };
                contextMenu.Items.Add(menuItem);

                //Show EditIntegerSliderDialog
                MenuItem menuItem1 = new MenuItem();
                menuItem1.Header = "Edit";
                menuItem1.Click += (s, e) =>
                {
                    EditIntegerSliderDialog editIntegerSliderDialog = new EditIntegerSliderDialog(this);
                    if (editIntegerSliderDialog.ShowDialog() == true)
                    {

                    }
                };
                contextMenu.Items.Add(menuItem1);

                return contextMenu;
            }
        }
        //private IRenderable _parent;
        //public IRenderable Parent => _parent;
        //private ElementsLinkedList<IRenderable> _children = new ElementsLinkedList<IRenderable>();
        //public ElementsLinkedList<IRenderable> Children => _children;
    }
}
