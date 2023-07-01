using Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Core.Geometry2D;

namespace Verse3.Elements
{
    /// <summary>
    /// Visual Interaction logic for TestElement.xaml
    /// </summary>
    public partial class IntegerSliderElementModelView : UserControl, IBaseElementView<IntegerSliderElementViewModel>
    {

        #region IBaseElementView Members

        private IntegerSliderElementViewModel _element;
        public IntegerSliderElementViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    _element = this.DataContext as IntegerSliderElementViewModel;
                }
                return _element;
            }
            private set
            {
                _element = value as IntegerSliderElementViewModel;
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion

        #region Constructor and Render

        public IntegerSliderElementModelView()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if (this.Element != null)
            {
            }
        }

        #endregion

        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
        }

        void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
        }

        #endregion

        #region UserControlEvents

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //DependencyPropertyChangedEventArgs
            Element = this.DataContext as IntegerSliderElementViewModel;
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Element = this.DataContext as IntegerSliderElementViewModel;
            Render();
        }

        #endregion

        private void SliderBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Element.OnValueChanged(sender, new RoutedPropertyChangedEventArgs<int>((int)e.OldValue, (int)e.NewValue));
            //ComputationPipeline.ComputeComputable(this.Element.RenderPipelineInfo.Parent as IComputable);
            //RenderPipeline.Render();
        }

    }
}