using Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Verse3.Elements
{
    /// <summary>
    /// Visual Interaction logic for TestElement.xaml
    /// </summary>
    public partial class BezierElementModelView : UserControl, IBaseElementView<BezierElementViewModel>
    {
        #region IBaseElementView Members

        private BezierElementViewModel _element;
        public BezierElementViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    _element = this.DataContext as BezierElementViewModel;
                }
                return _element;
            }
            private set
            {
                _element = value as BezierElementViewModel;
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion

        #region Constructor and Render

        public BezierElementModelView()
        {
            InitializeComponent();
        }
        
        internal bool _rendering = false;
        public void Render()
        {
            if (this._rendering) return;
            this._rendering = true;
            if (this.Element != null)
            {
                if (this.Element.RenderView != this) this.Element.RenderView = this;
                DrawBezierCurve(MainGrid, (this.Element).Direction);
            }

            this._rendering = false;
        }

        #endregion

        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MouseButtonEventArgs
            MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements.Focus();
            Keyboard.Focus(MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements);

            BezierElementModelView rectangle = (BezierElementModelView)sender;
            Cursor = Cursors.Hand;
            //IRenderable myRectangle = (IRenderable)rectangle.DataContext;

            ////myRectangle.IsSelected = true;

            ////mouseButtonDown = e.ChangedButton;

            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.None)
            //{
            //    //
            //    // We are in some other mouse handling mode, don't do anything.
            //    return;
            //}

            //DataViewModel.WPFControl.MouseHandlingMode = MouseHandlingMode.DraggingRectangles;
            //DataViewModel.WPFControl.origContentMouseDownPoint = e.GetPosition(DataViewModel.WPFControl.ContentElements);

            rectangle.CaptureMouse();

            e.Handled = true;
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            //MouseButtonEventArgs
            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            //{
            //    //
            //    // We are not in rectangle dragging mode.
            //    //
            //    return;
            //}

            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                try
                {
                    //TODO: Delete connection
                    IRenderable start = this.Element.Origin as IRenderable;
                    IRenderable end = this.Element.Destination as IRenderable;
                    this.Element.Remove();
                    RenderPipeline.RenderRenderable(start);
                    RenderPipeline.RenderRenderable(end);
                    //return;
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }

            MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode = MouseHandlingMode.None;

            BezierElementModelView rectangle = (BezierElementModelView)sender;
            rectangle.ReleaseMouseCapture();

            e.Handled = true;
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            //if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            //{
            //    Cursor = Cursors.Hand;
                //TODO: Prep to delete connection

                //DataViewModel.WPFControl.ExpandContent();

                //e.Handled = true;

                //return;
            //}
            //MouseEventArgs
            //if (DataViewModel.WPFControl.MouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            //{
            //    //
            //    // We are not in rectangle dragging mode, so don't do anything.
            //    //
            //    return;
            //}

            //Point curContentPoint = e.GetPosition(DataViewModel.WPFControl.ContentElements);
            //System.Windows.Vector rectangleDragVector = curContentPoint - DataViewModel.WPFControl.origContentMouseDownPoint;

            ////
            //// When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
            ////

            //DataViewModel.WPFControl.origContentMouseDownPoint = curContentPoint;

            //BezierElementView rectangle = (BezierElementView)sender;
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
            if (this.DataContext != null)
            {
                this.Element = this.DataContext as BezierElementViewModel;
                this.Element.RenderView = this;
                //Render();
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            if (this.DataContext != null)
            {
                this.Element = this.DataContext as BezierElementViewModel;
                this.Element.RenderView = this;
                //Render();
            }
        }

        #endregion

        Point start = default;
        Point end = default;
        public void DrawBezierCurve(Grid grid, BezierDirection dir = BezierDirection.Default)
        {
            //TODO: Adjustable x and y pull settings
            double xPull = 0.5;
            double yPull = 0.75;
            double hmargin = 5.0, vmargin = 5.0;
            double pull = yPull * this.Element.InnerBoundingBox.Size.Height;
            if ((yPull * this.Element.InnerBoundingBox.Size.Height)
                < (Math.Abs(xPull * this.Element.InnerBoundingBox.Size.Width)))
            {
                pull = xPull * this.Element.InnerBoundingBox.Size.Width;
            }
            //TODO: Calculate required width without having to calculate the curve first
            //Math.Abs((end.X - pull) - (start.X + pull)) CONTROL POINTS WIDTH
            //Math.Abs((end.X - (pull * 0.5)) - (start.X + (pull * 0.5))) FINAL WIDTH
            double sx = 0.0, sy = 0.0, ex = 0.0, ey = 0.0;
            double dx = (4 * pull / 5) + (2 * hmargin);
            double dy = (2 * vmargin);
            double newBBWidth = this.Element.InnerBoundingBox.Size.Width + dx;
            double newBBHeight = this.Element.InnerBoundingBox.Size.Height + dy;
            if (this.Element.InnerBoundingBox.Size.Width < pull)
            {
                if (this.Element.LeftToRight)
                {
                    sx = (dx / 2);
                    ex = this.Element.InnerBoundingBox.Size.Width + (dx / 2);
                }
                else
                {
                    sx = this.Element.InnerBoundingBox.Size.Width + (dx / 2);
                    ex = (dx / 2);
                }
                if (!this.Element.inflatedX)
                {
                    (this.Element as IRenderable).SetWidth(newBBWidth);
                    (this.Element as IRenderable).SetX(this.Element.X - (dx / 2));
                    this.Element.inflatedX = true;
                }
            }
            else
            {
                if (this.Element.LeftToRight)
                {
                    dx = (2 * hmargin);
                    sx = (dx / 2);
                    ex = this.Element.InnerBoundingBox.Size.Width + (dx / 2);
                }
                else
                {
                    sx = this.Element.InnerBoundingBox.Size.Width + (dx / 2);
                    ex = (dx / 2);
                }
                if (!this.Element.inflatedX)
                {
                    (this.Element as IRenderable).SetWidth(newBBWidth);
                    (this.Element as IRenderable).SetX(this.Element.X - (dx / 2));
                    this.Element.inflatedX = true;
                }
            }

            if (this.Element.TopToBottom)
            {
                ey = this.Element.InnerBoundingBox.Size.Height + (dy / 2);
                sy = (dy / 2);
                if (!this.Element.inflatedY)
                {
                    (this.Element as IRenderable).SetHeight(newBBHeight);
                    (this.Element as IRenderable).SetY(this.Element.Y - (dy / 2));
                    this.Element.inflatedY = true;
                }
            }
            else
            {
                sy = this.Element.InnerBoundingBox.Size.Height + (dy / 2);
                ey = (dy / 2);
                if (!this.Element.inflatedY)
                {
                    (this.Element as IRenderable).SetHeight(newBBHeight);
                    (this.Element as IRenderable).SetY(this.Element.Y - (dy / 2));
                    this.Element.inflatedY = true;
                }
            }

            start = new Point(sx, sy);
            end = new Point(ex, ey);

            Point[] curvePoints = GetBezierApproximation(start, end, pull, 256);

            PolyLineSegment segment = new PolyLineSegment(curvePoints, true);
            PathFigure pf = new PathFigure(segment.Points[0], new[] { segment }, false);
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            var pge = new PathGeometry();
            pge.Figures = pfc;
            Path p = new Path();
            p.Data = pge;
            Random rnd = new Random();
            //byte rc = (byte)Math.Round(rnd.NextDouble() * 255.0);
            //byte gc = (byte)Math.Round(rnd.NextDouble() * 255.0);
            //byte bc = (byte)Math.Round(rnd.NextDouble() * 255.0);
            //p.Stroke = new SolidColorBrush(Color.FromRgb(rc, gc, bc));
            p.Stroke = new SolidColorBrush(Colors.White);
            p.StrokeThickness = 3.0;
            grid.Children.Clear();
            //TODO: CREATE ENV VARIABLE for Debug Mode
            //if (false)
            //{
            //    LineGeometry lg = new LineGeometry(new Point((start.X + pull), start.Y), new Point((end.X - pull), end.Y));
            //    Path lp = new Path();
            //    lp.Data = lg;
            //    lp.Stroke = new SolidColorBrush(Color.FromRgb(bc, rc, gc));
            //    lp.StrokeThickness = 3.0;
            //    lp.StrokeStartLineCap = PenLineCap.Round;
            //    lp.StrokeEndLineCap = PenLineCap.Triangle;
            //    grid.Children.Add(lp);

            //    RectangleGeometry rg = new RectangleGeometry(new Rect(start, end));
            //    Path rp = new Path();
            //    rp.Data = rg;
            //    rp.Stroke = new SolidColorBrush(Color.FromRgb(gc, bc, rc));
            //    rp.StrokeThickness = 2.0;
            //    grid.Children.Add(rp);

            //}
            grid.Children.Add(p);
        }

        Point[] GetBezierApproximation(Point start, Point end, double pull, int outputSegmentCount)
        {
            Point[] controlPoints = new[] {
                // START
                start,
                // TANGENT START
                new Point((start.X + pull), start.Y),
                // MID
                new Point(((end.X + start.X) / 2), ((end.Y + start.Y) / 2)),
                // TANGENT END
                new Point((end.X - pull), end.Y),
                // END
                end
            };

            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return points;
        }

        Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }

    public enum BezierDirection
    {
        Default = 0,
        ForceLeftToRight = 1,
        ForceRightToLeft = 2
    }
}