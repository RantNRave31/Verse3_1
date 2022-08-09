using Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Verse3;
using static Core.Geometry2D;

namespace Verse3.VanillaElements
{
    /// <summary>
    /// Visual Interaction logic for TestElement.xaml
    /// </summary>
    public partial class SliderElementView : UserControl, IRenderView
    {
        private IRenderable _element;
        
        public IRenderable Element
        {
            get { return _element; }
            private set
            {
                _element = value;
                //Update();
            }
        }
        public Guid? ElementGuid
        {
            get { return _element?.ID; }
        }

        public SliderElementView()
        {
            InitializeComponent();

            Resources["PrimaryBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff6700"));
        }

        public void Render()
        {
            if (this.Element != null)
            {
                //this.Element.BoundingBox.Size.Height = SliderBlock.ActualHeight;
                //this.Element.BoundingBox.Size.Width = SliderBlock.ActualWidth;
                //this.Element.OnPropertyChanged("Width");
                //this.Element.OnPropertyChanged("Height");
            }
        }

        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ////MouseButtonEventArgs
            //DataViewModel.WPFControl.ContentElements.Focus();
            //Keyboard.Focus(DataViewModel.WPFControl.ContentElements);

            //SliderElementView rectangle = (SliderElementView)sender;
            //IRenderable myRectangle = (IRenderable)rectangle.DataContext;

            ////myRectangle.IsSelected = true;

            ////mouseButtonDown = e.ChangedButton;

            //if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            //{
            //    //
            //    // When the shift key is held down special zooming logic is executed in content_MouseDown,
            //    // so don't handle mouse input here.
            //    //
            //    return;
            //}

            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.None)
            //{
            //    //
            //    // We are in some other mouse handling mode, don't do anything.
            //    return;
            //}

            //DataViewModel.WPFControl.MouseHandlingMode = MouseHandlingMode.DraggingRectangles;
            //DataViewModel.WPFControl.origContentMouseDownPoint = e.GetPosition(DataViewModel.WPFControl.ContentElements);

            //rectangle.CaptureMouse();

            //e.Handled = true;
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ////MouseButtonEventArgs
            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            //{
            //    //
            //    // We are not in rectangle dragging mode.
            //    //
            //    return;
            //}

            //DataViewModel.WPFControl.MouseHandlingMode = MouseHandlingMode.None;

            //SliderElementView rectangle = (SliderElementView)sender;
            //rectangle.ReleaseMouseCapture();

            //e.Handled = true;
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            ////MouseEventArgs
            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            //{
            //    //
            //    // We are not in rectangle dragging mode, so don't do anything.
            //    //
            //    return;
            //}

            //Point curContentPoint = e.GetPosition(DataViewModel.WPFControl.ContentElements);
            //Vector rectangleDragVector = curContentPoint - DataViewModel.WPFControl.origContentMouseDownPoint;

            ////
            //// When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
            ////

            //DataViewModel.WPFControl.origContentMouseDownPoint = curContentPoint;

            //SliderElementView rectangle = (SliderElementView)sender;
            //IRenderable myRectangle = (IRenderable)rectangle.DataContext;
            //myRectangle.SetX(myRectangle.X + rectangleDragVector.X);
            //myRectangle.SetY(myRectangle.Y + rectangleDragVector.Y);

            //DataViewModel.WPFControl.ExpandContent();

            //e.Handled = true;
        }

        void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //MouseWheelEventArgs
        }

        #endregion

        #region UserControlEvents

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //DependencyPropertyChangedEventArgs
            Element = this.DataContext as IRenderable;
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Element = this.DataContext as IRenderable;
            Render();
        }

        #endregion

        private void SliderBlock_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //((SliderElement)this.Element).Value = (double)e.NewValue;
        }
    }

    public class SliderElement : IRenderable
    {        
        #region Data Members

        private BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
        private static Type view = typeof(SliderElementView);
        internal SliderElementView elView;
        public IRenderView RenderView
        {
            get
            {
                return elView;
            }
            set
            {
                if (value is SliderElementView)
                {
                    elView = (SliderElementView)value;
                }
            }
        }

        #endregion

        #region Properties

        public Type ViewType { get { return view; } }
        public object ViewKey { get; set; }

        public Guid ID { get => _id; private set => _id = value; }

        public bool IsSelected { get; set; }

        public BoundingBox BoundingBox { get => boundingBox; private set => boundingBox = value; }

        public double X { get => boundingBox.Location.X; }

        public double Y { get => boundingBox.Location.Y; }

        public double Width
        {
            get
            {
                return boundingBox.Size.Width;
            }
            set
            {
                boundingBox.Size.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return boundingBox.Size.Height;
            }
            set
            {
                boundingBox.Size.Height = value;
            }
        }

        private IRenderable _zPrev;
        public IRenderable ZPrev => _zPrev;
        private IRenderable _zNext;
        public IRenderable ZNext => _zNext;
        private IRenderable _parent;
        public IRenderable Parent => _parent;
        private ElementsLinkedList<IRenderable> _children = new ElementsLinkedList<IRenderable>();
        public ElementsLinkedList<IRenderable> Children => _children;

        public void AddChild(IRenderable child)
        {
            if (!this.Children.Contains(child))
            {
                this.Children.Add(child);
                child.SetParent(this);
            }
        }

        public void SetParent(IRenderable parent)
        {
            this._parent = parent;
        }

        public ElementState State { get; set; }

        //public IRenderView ElementView { get; }

        public ElementState ElementState { get; set; }
        public ElementType ElementType { get; set; }
        bool IRenderable.Visible { get; set; }

        #endregion

        #region Constructors

        public SliderElement()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.Value = 50;
        }

        public SliderElement(int x, int y, int width, int height)
        {
            this.boundingBox = new BoundingBox(x, y, width, height);

            this.Minimum = 0;
            this.Maximum = 100;
            this.Value = 50;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private double minimum;

        public double Minimum { get => minimum; set => SetProperty(ref minimum, value); }

        private double maximum;

        public double Maximum { get => maximum; set => SetProperty(ref maximum, value); }

        private double value1;

        public double Value { get => value1; set => SetProperty(ref value1, value); }

        private int interval;

        public int Interval { get => interval; set => SetProperty(ref interval, value); }

        #endregion
    }
}