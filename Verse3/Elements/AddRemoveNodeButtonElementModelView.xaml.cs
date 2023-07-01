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
    public partial class AddRemoveNodeButtonElementModelView : UserControl, IBaseElementView<AddRemoveNodeButtonElementViewModel>
    {
        #region IBaseElementView Members

        private AddRemoveNodeButtonElementViewModel _element;
        public AddRemoveNodeButtonElementViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    _element = this.DataContext as AddRemoveNodeButtonElementViewModel;
                }
                return _element;
            }
            private set
            {
                _element = value as AddRemoveNodeButtonElementViewModel;
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion

        #region Constructor and Render

        public AddRemoveNodeButtonElementModelView()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if (this.Element != null)
            {
                if (this.Element.AllowRearrangement)
                {
                    this.MoveUpBtn.Visibility = Visibility.Visible;
                    this.MoveDownBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    this.MoveUpBtn.Visibility = Visibility.Hidden;
                    this.MoveDownBtn.Visibility = Visibility.Hidden;
                }
                if (this.Element.IsFirst)
                {
                    this.RemoveBtn.Visibility = Visibility.Hidden;
                }
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
            Element = this.DataContext as AddRemoveNodeButtonElementViewModel;
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Element = this.DataContext as AddRemoveNodeButtonElementViewModel;
            Render();
        }

        #endregion

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Element.RemoveClicked(sender, e);
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Element.AddClicked(sender, e);
        }
        private void MoveUpBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Element.MoveUpClicked(sender, e);
        }
        private void MoveDownBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Element.MoveDownClicked(sender, e);
        }
    }
}