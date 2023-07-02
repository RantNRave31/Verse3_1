using Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Core.Geometry2D;

namespace Verse3.Elements
{
    /// <summary>
    /// Visual Interaction logic for TestElement.xaml
    /// </summary>
    public partial class ToggleElementModelView : UserControl, IBaseElementView<ToggleElementViewModel>
    {
        #region IBaseElementView Members

        private ToggleElementViewModel _element;
        public ToggleElementViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    _element = this.DataContext as ToggleElementViewModel;
                }
                return _element;
            }
            private set
            {
                _element = value as ToggleElementViewModel;
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion

        #region Constructor and Render

        public ToggleElementModelView()
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
            Element = this.DataContext as ToggleElementViewModel;
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Element = this.DataContext as ToggleElementViewModel;
            Render();
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Element.OnUnchecked(sender, e);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.Element.OnChecked(sender, e);
        }

        #endregion
    }
}