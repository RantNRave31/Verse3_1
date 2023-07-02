using Core;
using System;
using System.Windows.Input;

namespace Verse3.Components
{
    //string? txt = this.ElementText;
    //textBlock = new TextElement();
    //textBlock.DisplayedText = txt;
    //    textBlock.TextAlignment = TextAlignment.Left;
    //    DataTemplateManager.RegisterDataTemplate(textBlock);
    //    this.RenderPipelineInfo.AddChild(textBlock);

    //    sliderBlock = new SliderElement();
    //sliderBlock.Minimum = 0;
    //    sliderBlock.Maximum = 100;
    //    sliderBlock.Value = 50;
    //    sliderBlock.ValueChanged += SliderBlock_OnValueChanged;
    //    DataTemplateManager.RegisterDataTemplate(sliderBlock);
    //    this.RenderPipelineInfo.AddChild(sliderBlock);

    //    var buttonBlock = new ButtonElement();
    //buttonBlock.DisplayedText = "Click me";
    //    buttonBlock.OnButtonClicked += ButtonBlock_OnButtonClicked;
    //    DataTemplateManager.RegisterDataTemplate(buttonBlock);
    //    this.RenderPipelineInfo.AddChild(buttonBlock);

    //    var textBoxBlock = new TextBoxElement();
    //textBoxBlock.InputText = "Enter text";
    //    DataTemplateManager.RegisterDataTemplate(textBoxBlock);
    //    this.RenderPipelineInfo.AddChild(textBoxBlock);

    //nodeBlock2 = new NumberDataNode(this, NodeType.Output);
    //DataTemplateManager.RegisterDataTemplate(nodeBlock2);
    //    this.RenderPipelineInfo.AddChild(nodeBlock2);
    //    this.ComputationPipelineInfo.IOManager.AddDataOutputNode<double>(nodeBlock2 as IDataNode<double>);

    //    string? txt = this.ElementText;
    //textBlock = new TextElement();
    //textBlock.DisplayedText = txt;
    //    textBlock.TextAlignment = TextAlignment.Left;
    //    DataTemplateManager.RegisterDataTemplate(textBlock);
    //    this.RenderPipelineInfo.AddChild(textBlock);

    public interface IBaseCompView<R> : IRenderView where R : BaseCompViewModel
    {
        public new R Element { get; }
        public Guid? ElementGuid
        {
            get { return Element?.ID; }
        }


        public new virtual void Render()
        {
            if (Element != null)
            {
                if (Element.RenderView != this) Element.RenderView = this;
                Element.RenderComp();
            }
        }

        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when the mouse wheel is moved.
        /// </summary>
        public void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        #endregion

        #region UserControlEvents

        //public void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (this.GetType().IsAssignableTo(typeof(UserControl)))
        //    {
        //        UserControl uc = this as UserControl;
        //        if (uc.DataContext is R)
        //        {
        //            Element = (R)uc.DataContext;
        //        }
        //    }
        //    Render();
        //}

        //public void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    if (this.GetType().IsAssignableTo(typeof(UserControl)))
        //    {
        //        UserControl uc = this as UserControl;
        //        if (uc.DataContext is R)
        //        {
        //            Element = (R)uc.DataContext;
        //        }
        //    }
        //    Render();
        //}

        #endregion
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
