using Core;
using System;
using System.Windows;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using Postgrest;
using Core.Nodes;

namespace IntegerLibrary.Inputs
{
    public class ToggleBit8 : BaseCompViewModel
    {
        internal bool?[] _bits = { false, false, false, false, false, false, false, false };
        internal int _value = 0;

        #region Constructors

        public ToggleBit8() : base()
        {
        }

        public ToggleBit8(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            _value = 0;
            _value |= _bits[0] == true ? 1 : 0;
            toggleBlock.DisplayedText = _bits[0].ToString();
            _value |= (_bits[1] == true ? 1 : 0) << 1;
            toggleBlock1.DisplayedText = _bits[1].ToString();
            _value |= (_bits[2] == true ? 1 : 0) << 2;
            toggleBlock2.DisplayedText = _bits[2].ToString();
            _value |= (_bits[3] == true ? 1 : 0) << 3;
            toggleBlock3.DisplayedText = _bits[3].ToString();
            _value |= (_bits[4] == true ? 1 : 0) << 4;
            toggleBlock4.DisplayedText = _bits[4].ToString();
            _value |= (_bits[5] == true ? 1 : 0) << 5;
            toggleBlock5.DisplayedText = _bits[5].ToString();
            _value |= (_bits[6] == true ? 1 : 0) << 6;
            toggleBlock6.DisplayedText = _bits[6].ToString();
            _value |= (_bits[7] == true ? 1 : 0) << 7;
            toggleBlock7.DisplayedText = _bits[7].ToString();
            ChildElementManager.SetData(_value, nodeBlock);
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "BIT8", "Types", "Integer");

        internal ToggleElementViewModel toggleBlock = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock1 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock2 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock3 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock4 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock5 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock6 = new ToggleElementViewModel();
        internal ToggleElementViewModel toggleBlock7 = new ToggleElementViewModel();
        internal IntegerDataNode nodeBlock;

        public override void Initialize()
        {
            titleTextBlock.TextRotation = 0;

            toggleBlock = new ToggleElementViewModel();
            toggleBlock.Value = _bits[0];
            toggleBlock.DisplayedText = _bits[0].ToString();
            toggleBlock.ToggleChecked += ButtonBlock_ToggleChecked;
            toggleBlock.ToggleUnchecked += ButtonBlock_ToggleUnchecked;
            toggleBlock.Width = 200;
            ChildElementManager.AddElement(toggleBlock);

            toggleBlock1 = new ToggleElementViewModel();
            toggleBlock1.Value = _bits[1];
            toggleBlock1.DisplayedText = _bits[1].ToString();
            toggleBlock1.ToggleChecked += ButtonBlock1_ToggleChecked;
            toggleBlock1.ToggleUnchecked += ButtonBlock1_ToggleUnchecked;
            toggleBlock1.Width = 200;
            ChildElementManager.AddElement(toggleBlock1);

            toggleBlock2 = new ToggleElementViewModel();
            toggleBlock2.Value = _bits[2];
            toggleBlock2.DisplayedText = _bits[2].ToString();
            toggleBlock2.ToggleChecked += ButtonBlock2_ToggleChecked;
            toggleBlock2.ToggleUnchecked += ButtonBlock2_ToggleUnchecked;
            toggleBlock2.Width = 200;
            ChildElementManager.AddElement(toggleBlock2);

            toggleBlock3 = new ToggleElementViewModel();
            toggleBlock3.Value = _bits[3];
            toggleBlock3.DisplayedText = _bits[3].ToString();
            toggleBlock3.ToggleChecked += ButtonBlock3_ToggleChecked;
            toggleBlock3.ToggleUnchecked += ButtonBlock3_ToggleUnchecked;
            toggleBlock3.Width = 200;
            ChildElementManager.AddElement(toggleBlock3);

            toggleBlock4 = new ToggleElementViewModel();
            toggleBlock4.Value = _bits[4];
            toggleBlock4.DisplayedText = _bits[4].ToString();
            toggleBlock4.ToggleChecked += ButtonBlock4_ToggleChecked;
            toggleBlock4.ToggleUnchecked += ButtonBlock4_ToggleUnchecked;
            toggleBlock4.Width = 200;
            ChildElementManager.AddElement(toggleBlock4);

            toggleBlock5 = new ToggleElementViewModel();
            toggleBlock5.Value = _bits[5];
            toggleBlock5.DisplayedText = _bits[5].ToString();
            toggleBlock5.ToggleChecked += ButtonBlock5_ToggleChecked;
            toggleBlock5.ToggleUnchecked += ButtonBlock5_ToggleUnchecked;
            toggleBlock5.Width = 200;
            ChildElementManager.AddElement(toggleBlock5);

            toggleBlock6 = new ToggleElementViewModel();
            toggleBlock6.Value = _bits[6];
            toggleBlock6.DisplayedText = _bits[6].ToString();
            toggleBlock6.ToggleChecked += ButtonBlock6_ToggleChecked;
            toggleBlock6.ToggleUnchecked += ButtonBlock6_ToggleUnchecked;
            toggleBlock6.Width = 200;
            ChildElementManager.AddElement(toggleBlock6);

            toggleBlock7 = new ToggleElementViewModel();
            toggleBlock7.Value = _bits[7];
            toggleBlock7.DisplayedText = _bits[7].ToString();
            toggleBlock7.ToggleChecked += ButtonBlock7_ToggleChecked;
            toggleBlock7.ToggleUnchecked += ButtonBlock7_ToggleUnchecked;
            toggleBlock7.Width = 200;
            ChildElementManager.AddElement(toggleBlock7);

            nodeBlock = new IntegerDataNode(this, NodeType.Output);
            ChildElementManager.AddDataOutputNode(nodeBlock, "Value", true);
        }

        private void ButtonBlock_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[0] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[0] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock1_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[1] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock1_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[1] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock2_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[2] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock2_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[2] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock3_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[3] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock3_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[3] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock4_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[4] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock4_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[4] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock5_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[5] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock5_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[5] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock6_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[6] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock6_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[6] = false;
            ComputationCore.Compute(this, false);
        }
        private void ButtonBlock7_ToggleChecked(object? sender, RoutedEventArgs e)
        {
            _bits[7] = true;
            ComputationCore.Compute(this, false);
        }

        private void ButtonBlock7_ToggleUnchecked(object? sender, RoutedEventArgs e)
        {
            _bits[7] = false;
            ComputationCore.Compute(this, false);
        }
    }
}
