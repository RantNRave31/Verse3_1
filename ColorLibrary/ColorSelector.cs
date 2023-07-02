﻿using Core;
using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Verse3.Nodes;
using Verse3.Elements;
using Verse3.Components;
using Core.Nodes;

namespace ColorLibrary
{
    public class ColorSelector : BaseCompViewModel
    {
        internal Color? _value = Color.FromArgb(255, 255, 255, 255);
        
        #region Constructors

        public ColorSelector() : base()
        {
        }

        public ColorSelector(int x, int y) : base(x, y)
        {
        }

        #endregion

        public override void Compute()
        {
            if (_value.HasValue && b != null)
            {
                buttonBlock.BackgroundColor = b;
                buttonBlock.DisplayedText = _value.ToString();
                textBoxElement.InputText = _value.ToString();
                this.ChildElementManager.SetData<Color>(_value.Value, nodeBlock);
            }
        }
        public override CompInfo GetCompInfo() => new CompInfo(this, "Color Picker", "Basic UI", "Color");

        internal ColorPicker colorPicker;
        internal PopupWindow window;

        internal ButtonElementViewModel buttonBlock = new ButtonElementViewModel();
        internal TextBoxElementViewModel textBoxElement = new TextBoxElementViewModel();
        internal ColorDataNode nodeBlock;
        internal GenericEventNode nodeBlock1;
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;

            nodeBlock1 = new GenericEventNode(this, NodeType.Output);
            this.ChildElementManager.AddEventOutputNode(nodeBlock1, "Changed");

            nodeBlock = new ColorDataNode(this, NodeType.Output);
            this.ChildElementManager.AddDataOutputNode(nodeBlock, "Color");

            buttonBlock = new ButtonElementViewModel();
            buttonBlock.DisplayedText = _value.GetValueOrDefault(Color.FromArgb(255, 255, 255, 255)).ToString();
            buttonBlock.BackgroundColor = new System.Windows.Media.SolidColorBrush(_value.GetValueOrDefault(Color.FromArgb(255, 255, 255, 255)));
            buttonBlock.OnButtonClicked += ButtonBlock_OnButtonClicked;
            this.ChildElementManager.AddElement(buttonBlock);

            textBoxElement = new TextBoxElementViewModel();
            textBoxElement.InputText = _value.GetValueOrDefault(Color.FromArgb(255, 255, 255, 255)).ToString();
            textBoxElement.ValueChanged += TextBoxElement_ValueChanged;
            this.ChildElementManager.AddElement(textBoxElement);
        }

        private void TextBoxElement_ValueChanged(object? sender, TextChangedEventArgs e)
        {
            try
            {
                object tryColor = ColorConverter.ConvertFromString(textBoxElement.InputText);
                if (tryColor is Color parsedColor)
                {
                    _value = parsedColor;
                    b = new SolidColorBrush(_value.Value);
                    ComputationCore.Compute(this, false);
                    this.ChildElementManager.EventOccured(nodeBlock1, new EventArgData(new DataStructure(_value)));
                }
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        private void ButtonBlock_OnButtonClicked(object? sender, RoutedEventArgs e)
        {

            //create a winforms color dialog box
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            colorDialog.AllowFullOpen = true;
            switch (colorDialog.ShowDialog())
            {
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    {
                        ColorPicker_SelectedColorChanged(colorDialog, colorDialog.Color);
                        break;
                    }
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.TryAgain:
                    break;
                case DialogResult.Continue:
                    break;
                default:
                    break;
            }
        }

        internal SolidColorBrush? b = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

        private void ColorPicker_SelectedColorChanged(object? sender, System.Drawing.Color e)
        {
            _value = Color.FromArgb(e.A, e.R, e.G, e.B);
            b = new SolidColorBrush(_value.Value);
            ComputationCore.Compute(this, false);
            this.ChildElementManager.EventOccured(nodeBlock1, new EventArgData(new DataStructure(_value)));
        }
    }
}
