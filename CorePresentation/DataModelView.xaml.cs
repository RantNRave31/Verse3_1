﻿using Core;
using Core.Elements;
using Core.Nodes;
using InfiniteCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Verse3.Components;
using Verse3.CorePresentation.Workspaces;
using Verse3.Elements;
using Verse3.Nodes;
using static Core.Geometry2D;

namespace Verse3
{
    /// <summary>
    /// Interaction logic for InfiniteCanvasWPFControl.xaml
    /// </summary>
    public partial class DataModelView : UserControl
    {
        public DataModelView()
        {
            InitializeComponent();
            CompositionTarget.Rendering += BeforeFrameRender;
            MouseDown += Canvas_MouseDown;
            MouseUp += Canvas_MouseUp;
            MouseMove += Canvas_MouseMove;
            DataContextChanged += DataModelView_DataContextChanged;
            //TODO Add different view templates
        }

        private void DataModelView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((DataViewModel)this.DataContext).DataModelView = this;
        }

        public DataViewModel DataViewModel
        {
            get
            {
                return (DataViewModel)this.DataContext;
            }
        }
        #region Fields

        private System.Windows.Forms.Cursor winFormsCursor = System.Windows.Forms.Cursors.Default;
        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;
        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        internal Point origZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        public Point origContentMouseDownPoint;

        /// <summary>
        /// Records which mouse button clicked during mouse dragging.
        /// </summary>
        public MouseButton mouseButtonDown;

        /// <summary>
        /// Saves the previous zoom rectangle, pressing the backspace key jumps back to this zoom rectangle.
        /// </summary>
        private Rect prevZoomRect;

        /// <summary>
        /// Save the previous content scale, pressing the backspace key jumps back to this scale.
        /// </summary>
        private double prevZoomScale;

        /// <summary>
        /// Set to 'true' when the previous zoom rect is saved.
        /// </summary>
        private bool prevZoomRectSet = false;

        ///// <summary>
        ///// Set to 'true' when the previous select rect is saved.
        ///// </summary>
        //private bool prevSelectRectSet = false;

        ///// <summary>
        ///// Saves the previous Select rectangle
        ///// </summary>
        //private Rect prevSelectRect;
        #endregion

        #region Properties

        public ListBox ContentElements
        {
            get
            {
                return LBcontent;
            }
        }
        public System.Windows.Forms.Cursor WinFormsCursor
        {
            get
            {
                return winFormsCursor;
            }
            set
            {
                winFormsCursor = value;
            }
        }
        public MouseHandlingMode MouseHandlingMode { get { return mouseHandlingMode; } set { mouseHandlingMode = value; } }

        public double AverageFPS
        {
            get
            {
                return avgfps;
            }
        }

        #endregion

        #region Methods

        private void BeforeFrameRender(object sender, EventArgs e)
        {
            UpdateFrameStats(sender, e);
            //TODO: Perform other tasks here before each frame render
            //RenderPipeline.Render();
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {

            WorkspaceViewModel.StaticWorkspaceViewModel.LoadLibraries(sender, e);
            ExpandContent();
        }

        /// <summary>
        /// Expand the content area to fit the rectangles.
        /// </summary>
        public void ExpandContent()
        {
            if (DataViewModel.DataModelView != this)
                DataViewModel.InitDataViewModel(this);
            if (LBcontent.ItemsSource !=this.DataViewModel.Elements)
                LBcontent.ItemsSource = DataViewModel.Elements;

            double xOffset = 0;
            double yOffset = 0;
            Rect contentRect = new Rect(0, 0, 0, 0);
            ElementsLinkedList<IElement> _elementsBuffer = DataViewModel.Elements;
            foreach (IRenderable elementsData in _elementsBuffer)
            {
                if (elementsData.X < xOffset)
                {
                    xOffset = elementsData.X;
                }

                if (elementsData.Y < yOffset)
                {
                    yOffset = elementsData.Y;
                }

                contentRect.Union(new Rect(elementsData.X, elementsData.Y, elementsData.Width, elementsData.Height));
            }

            //
            // Translate all rectangles so they are in positive space.
            //
            xOffset = Math.Abs(xOffset);
            yOffset = Math.Abs(yOffset);
            foreach (IRenderable el in _elementsBuffer)
            {
                if (el != null)
                {
                    el.SetX(el.X + xOffset);
                    el.SetY(el.Y + yOffset);
                    el.Render();
                    if (el.Children != null)
                    {
                        if (el.Children.Count > 0)
                        {
                            foreach (IRenderable c in el.Children)
                            {
                                c.SetX(c.X + xOffset);
                                c.SetY(c.Y + yOffset);
                                c.Render();
                            }
                        }
                    }
                }
            }

            WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.ContentWidth = contentRect.Width;
            WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.ContentHeight = contentRect.Height;
        }

        private Point currCanvasMousePosition = new Point();
        public System.Drawing.Point GetMouseRelPosition(object sender = null)
        {
            System.Drawing.Point p = new System.Drawing.Point((int)currCanvasMousePosition.X, (int)currCanvasMousePosition.Y);
            MousePositionChanged?.Invoke(sender, p);
            return p;
        }

        //Event for mouse position update
        public event EventHandler<System.Drawing.Point> MousePositionChanged;

        public System.Drawing.Point GetRelPosition(CanvasPoint pos)
        {
            //Canvas c = LBcontent.Style.Resources.FindName("InfiniteCanvasBackground") as Canvas;
            //pos = LBcontent.PointToScreen(pos);
            return new System.Drawing.Point((int)pos.X, (int)pos.Y);
        }

        public void Select(IRenderable el)
        {
            if (el != null)
            {
                el.IsSelected = true;
                this.ContentElements.SelectedItem = el;
            }
        }
        
        public void AddToSelection(IRenderable el)
        {
            if (el != null)
            {
                el.IsSelected = true;
                this.ContentElements.SelectedItems.Add(el);
            }
        }

        public void Deselect(IRenderable el)
        {
            if (el != null)
            {
                el.IsSelected = false;
                this.ContentElements.SelectedItems.Remove(el);
            }
        }

        public void ClearSelection()
        {
            foreach (IRenderable renderable in this.ContentElements.SelectedItems)
            {
                if (renderable != null)
                {
                    renderable.IsSelected = false;
                }
            }
            this.ContentElements.SelectedItems.Clear();
        }

        //TODO: LBcontent.Items and LBcontent.HitTest
        #endregion

        #region MouseEvents
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                this.Cursor = infiniteCanvas.Cursor;
            }
            if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.SelectedConnection != default)
            {
                //DataViewModel.ActiveConnection.Destination.
            }
            if (WorkspaceViewModel.compsPendingInst.Count > 0)
            {
                WorkspaceViewModel.StaticWorkspaceViewModel.AddToCanvas_OnCall(sender, e);
            }
        }

        private void Canvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                if (infiniteCanvas.MouseHandlingMode == MouseHandlingMode.None)
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
            //if (DataViewModel.ActiveNode != default /*&& started*/)
            //{
            //    //DrawBezierCurve(drawstart, InfiniteCanvasWPFControl.GetMouseRelPosition(), rtl);

            //    if (DataViewModel.ActiveConnection == default)
            //    {
            //        DataViewModel.ActiveConnection = CreateConnection(DataViewModel.ActiveNode);
            //    }
            //    else
            //    {
            //        ((BezierElement)DataViewModel.ActiveConnection).SetDestination(DataViewModel.ActiveNode);
            //        DataViewModel.ActiveConnection = default;
            //        DataViewModel.ActiveNode = default;
            //    }
            //    //started = false;
            //}
        }

        //public INode drawstart = default;
        private void Canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataModelView)
            {
                DataModelView infiniteCanvas = (DataModelView)sender;
                if (infiniteCanvas.MouseHandlingMode == MouseHandlingMode.Panning)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                //if (started)
                //{
                //}
            }
        }

        /// <summary>
        /// Event raised on mouse down in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LBcontent.Focus();
            Keyboard.Focus(LBcontent);

            mouseButtonDown = e.ChangedButton;
            origZoomAndPanControlMouseDownPoint = e.GetPosition(InfiniteCanvasControl1);
            origContentMouseDownPoint = e.GetPosition(LBcontent);

            if (mouseButtonDown == MouseButton.Left)
            {
                mouseHandlingMode = MouseHandlingMode.Selecting;
                if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
                {
                    // Shift + left- or right-down initiates zooming mode.
                    mouseHandlingMode = MouseHandlingMode.Zooming;
                }
            }
            if (mouseButtonDown == MouseButton.Right)
            {
                // Just a plain old left-down initiates panning mode.
                mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                // Capture the mouse so that we eventually receive the mouse up event.
                InfiniteCanvasControl1.CaptureMouse();
                ClearSelection();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                if (mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (mouseButtonDown == MouseButton.Left)
                    {
                        // Shift + left-click zooms in on the content.
                        ZoomIn(origContentMouseDownPoint);
                    }
                    else if (mouseButtonDown == MouseButton.Right)
                    {
                        // Shift + left-click zooms out from the content.
                        ZoomOut(origContentMouseDownPoint);
                    }
                }
                else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
                {
                    // When drag-zooming has finished we zoom in on the rectangle that was highlighted by the user.
                    ApplyDragZoomRect();
                }
                else if (mouseHandlingMode == MouseHandlingMode.DragSelecting)
                {
                    // When drag-zooming has finished we zoom in on the rectangle that was highlighted by the user.
                    ApplyDragSelectRect(origContentMouseDownPoint, ((Keyboard.Modifiers & ModifierKeys.Shift) != 0));
                }

                InfiniteCanvasControl1.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                e.Handled = true;
            }
        }



        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            currCanvasMousePosition = e.GetPosition(LBcontent);
            MousePositionNode.RefreshPosition();

            //TODO: Re-render every IRenderable
            //foreach (IRenderable renderable in DataViewModel.Instance.Elements)
            //{
            //    if (renderable != null)
            //    {
            //        renderable.Render();
            //        if (renderable.Children != null)
            //        {
            //            if (renderable.Children.Count() > 0)
            //            {
            //                foreach (IRenderable r in renderable.Children)
            //                {
            //                    if (r != null)
            //                    {
            //                        r.Render();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //DataViewModel.WPFControl.ExpandContent();

            if (mouseHandlingMode == MouseHandlingMode.Panning)
            {
                //
                // The user is left-dragging the mouse.
                // Pan the viewport by the appropriate amount.
                //
                Point curContentMousePoint = e.GetPosition(LBcontent);
                Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

                InfiniteCanvasControl1.ContentOffsetX -= dragOffset.X;
                InfiniteCanvasControl1.ContentOffsetY -= dragOffset.Y;

                this.Cursor = Cursors.SizeAll;
                this.WinFormsCursor = System.Windows.Forms.Cursors.SizeAll;

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                Point curZoomAndPanControlMousePoint = e.GetPosition(InfiniteCanvasControl1);
                Vector dragOffset = curZoomAndPanControlMousePoint - origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 10;
                if (mouseButtonDown == MouseButton.Left &&
                    (Math.Abs(dragOffset.X) > dragThreshold ||
                        Math.Abs(dragOffset.Y) > dragThreshold))
                {
                    //
                    // When Shift + left-down zooming mode and the user drags beyond the drag threshold,
                    // initiate drag zooming mode where the user can drag out a rectangle to select the area
                    // to zoom in on.
                    //
                    mouseHandlingMode = MouseHandlingMode.DragZooming;
                    this.Cursor = Cursors.SizeNWSE;
                    this.WinFormsCursor = System.Windows.Forms.Cursors.SizeNWSE;
                    Point curContentMousePoint = e.GetPosition(LBcontent);
                    InitDragZoomRect(origContentMouseDownPoint, curContentMousePoint);
                }

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.WinFormsCursor = System.Windows.Forms.Cursors.SizeNWSE;
                Point curContentMousePoint = e.GetPosition(LBcontent);
                InitDragZoomRect(origContentMouseDownPoint, curContentMousePoint);
            }
            else if (mouseHandlingMode == MouseHandlingMode.Selecting)
            {
                Point curZoomAndPanControlMousePoint = e.GetPosition(InfiniteCanvasControl1);
                Vector dragOffset = curZoomAndPanControlMousePoint - origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 5;
                if ((Math.Abs(dragOffset.X) > dragThreshold) ||
                        (Math.Abs(dragOffset.Y) > dragThreshold))
                {
                    //
                    // When left-down selecting mode and the user drags beyond the drag threshold,
                    // initiate drag selecting mode where the user can drag out a rectangle to select the area
                    // to select objects from.
                    //
                    mouseHandlingMode = MouseHandlingMode.DragSelecting;
                    this.Cursor = Cursors.Cross;
                    this.WinFormsCursor = System.Windows.Forms.Cursors.Cross;
                    Point curContentMousePoint = e.GetPosition(LBcontent);
                    InitDragSelectRect(origContentMouseDownPoint, curContentMousePoint);
                }

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.DragSelecting)
            {
                this.Cursor = Cursors.Cross;
                this.WinFormsCursor = System.Windows.Forms.Cursors.Cross;
                Point curContentMousePoint = e.GetPosition(LBcontent);
                InitDragSelectRect(origContentMouseDownPoint, curContentMousePoint);
            }
            else if (this.WinFormsCursor != System.Windows.Forms.Cursors.Default)
            {
                this.Cursor = Cursors.Arrow;
                this.WinFormsCursor = System.Windows.Forms.Cursors.Default;
            }
            //else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
            //{
            //    //
            //    // When in drag zooming mode continously update the position of the rectangle
            //    // that the user is dragging out.
            //    //
            //    Point curContentMousePoint = e.GetPosition(LBcontent);
            //    SetDragZoomRect(origContentMouseDownPoint, curContentMousePoint);

            //    e.Handled = true;
            //}
            //else if (mouseHandlingMode == MouseHandlingMode.DragSelecting)
            //{
            //    //
            //    // When in drag zooming mode continously update the position of the rectangle
            //    // that the user is dragging out.
            //    //
            //    Point curContentMousePoint = e.GetPosition(LBcontent);
            //    SetDragSelectRect(origContentMouseDownPoint, curContentMousePoint);
            //}
            //}


        }

        /// <summary>
        /// Event raised by rotating the mouse wheel
        /// </summary>
        private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                Point curContentMousePoint = e.GetPosition(LBcontent);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                Point curContentMousePoint = e.GetPosition(LBcontent);
                ZoomOut(curContentMousePoint);
            }
        }

        private void zoomAndPanControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                Point doubleClickPoint = e.GetPosition(LBcontent);
                //InfiniteCanvasControl1.AnimatedSnapTo(doubleClickPoint);
            }
        }

        #endregion

        private void InfiniteCanvasControl1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    {
                        if (this.LBcontent.SelectedItems != null && this.LBcontent.SelectedItems.Count > 0)
                        {
                            List<IRenderable> toBeDeleted = new List<IRenderable>();
                            for (int i = 0; i < this.LBcontent.SelectedItems.Count; i++)
                            {
                                if (this.LBcontent.SelectedItems.Count > i)
                                {
                                    IRenderable renderable = this.LBcontent.SelectedItems[i] as IRenderable;
                                    if (renderable != null)
                                    {
                                        toBeDeleted.Add(renderable);
                                    }
                                }
                            }
                            this.ClearSelection();
                            foreach (IRenderable renderable1 in toBeDeleted)
                            {
                                if (renderable1 is BaseCompViewModel bc)
                                {
                                    List<IConnection> toBeDeletedConnections = new List<IConnection>();
                                    if (bc.ComputationPipelineInfo.IOManager.DataInputNodes.Count > 0)
                                    {
                                        foreach (IDataNode dataInputNode in bc.ComputationPipelineInfo.IOManager.DataInputNodes)
                                        {
                                            if (dataInputNode.Connections.Count > 0)
                                            {
                                                foreach (IConnection dataConnection in dataInputNode.Connections)
                                                {
                                                    if (dataConnection is BezierElementViewModel be) toBeDeletedConnections.Add(be);
                                                }
                                            }
                                        }
                                    }
                                    if (bc.ComputationPipelineInfo.IOManager.DataOutputNodes.Count > 0)
                                    {
                                        foreach (IDataNode dataOutputNode in bc.ComputationPipelineInfo.IOManager.DataOutputNodes)
                                        {
                                            if (dataOutputNode.Connections.Count > 0)
                                            {
                                                foreach (IConnection dataConnection in dataOutputNode.Connections)
                                                {
                                                    if (dataConnection is BezierElementViewModel be) toBeDeletedConnections.Add(be);
                                                }
                                            }
                                        }
                                    }
                                    if (bc.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 0)
                                    {
                                        foreach (IEventNode eventInputNode in bc.ComputationPipelineInfo.IOManager.EventInputNodes)
                                        {
                                            if (eventInputNode.Connections.Count > 0)
                                            {
                                                foreach (IConnection eventConnection in eventInputNode.Connections)
                                                {
                                                    if (eventConnection is BezierElementViewModel be) toBeDeletedConnections.Add(be);
                                                }
                                            }
                                        }
                                    }
                                    if (bc.ComputationPipelineInfo.IOManager.EventOutputNodes.Count > 0)
                                    {
                                        foreach (IEventNode eventOutputNode in bc.ComputationPipelineInfo.IOManager.EventOutputNodes)
                                        {
                                            if (eventOutputNode.Connections.Count > 0)
                                            {
                                                foreach (IConnection eventConnection in eventOutputNode.Connections)
                                                {
                                                    if (eventConnection is BezierElementViewModel be) toBeDeletedConnections.Add(be);
                                                }
                                            }
                                        }
                                    }
                                    if (toBeDeletedConnections.Count > 0)
                                    {
                                        foreach (IConnection connection in toBeDeletedConnections)
                                        {
                                            if (connection is BezierElementViewModel bezierElement)
                                            {
                                                bezierElement.Remove();
                                            }
                                        }
                                    }
                                }
                                renderable1.Dispose();
                            }
                            e.Handled = true;
                        }
                        break;
                    }
                case Key.Escape:
                    {
                        if (DataViewModel.SelectedConnection != default)
                        {
                            DataViewModel.SelectedConnection.Origin.Connections.Remove(DataViewModel.SelectedConnection);
                            DataViewModel.SelectedConnection.Destination.Connections.Remove(DataViewModel.SelectedConnection);
                            DataViewModel.Elements.Remove(DataViewModel.SelectedConnection);
                            DataViewModel.SelectedConnection.Dispose();
                            DataViewModel.SelectedConnection = default;
                        }
                        break;
                    }
                case Key.Space:
                    {
                        if (DataViewModel.SearchBarCompInfo.ConstructorInfo != null)
                        {
                            if (DataViewModel.SearchBarCompInfo.ConstructorInfo.GetParameters().Length > 0)
                            {
                                ParameterInfo[] pi = DataViewModel.SearchBarCompInfo.ConstructorInfo.GetParameters();
                                object[] args = new object[pi.Length];
                                for (int i = 0; i < pi.Length; i++)
                                {
                                    if (!(pi[i].DefaultValue is DBNull)) args[i] = pi[i].DefaultValue;
                                    else
                                    {
                                        if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "x")
                                            args[i] = DataViewModel.DataModelView.GetMouseRelPosition().X;
                                        else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
                                            args[i] = DataViewModel.DataModelView.GetMouseRelPosition().Y;
                                    }
                                }
                                IElement elInst = DataViewModel.SearchBarCompInfo.ConstructorInfo.Invoke(args) as IElement;
                                DataViewModel.Elements.Add(elInst);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }


        #region ZoomFunctions

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut(Point contentZoomCenter)
        {
            InfiniteCanvasControl1.ZoomAboutPoint(InfiniteCanvasControl1.ContentScale - 0.1, contentZoomCenter);
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomIn(Point contentZoomCenter)
        {
            InfiniteCanvasControl1.ZoomAboutPoint(InfiniteCanvasControl1.ContentScale + 0.1, contentZoomCenter);
        }

        /// <summary>
        /// The 'ZoomIn' command (bound to the plus key) was executed.
        /// </summary>
        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn(new Point(InfiniteCanvasControl1.ContentZoomFocusX, InfiniteCanvasControl1.ContentZoomFocusY));
        }

        /// <summary>
        /// The 'ZoomOut' command (bound to the minus key) was executed.
        /// </summary>
        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut(new Point(InfiniteCanvasControl1.ContentZoomFocusX, InfiniteCanvasControl1.ContentZoomFocusY));
        }

        /// <summary>
        /// The 'JumpBackToPrevZoom' command was executed.
        /// </summary>
        private void JumpBackToPrevZoom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            JumpBackToPrevZoom();
        }

        /// <summary>
        /// Determines whether the 'JumpBackToPrevZoom' command can be executed.
        /// </summary>
        private void JumpBackToPrevZoom_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = prevZoomRectSet;
        }

        /// <summary>
        /// The 'Fill' command was executed.
        /// </summary>
        private void Fill_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            InfiniteCanvasControl1.AnimatedScaleToFit();
        }

        /// <summary>
        /// The 'OneHundredPercent' command was executed.
        /// </summary>
        private void OneHundredPercent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            InfiniteCanvasControl1.AnimatedZoomTo(1.0);
        }

        /// <summary>
        /// Jump back to the previous zoom level.
        /// </summary>
        private void JumpBackToPrevZoom()
        {
            InfiniteCanvasControl1.AnimatedZoomTo(prevZoomScale, prevZoomRect);

            ClearPrevZoomRect();
        }

        /// <summary>
        /// Initialise the rectangle that the use is dragging out.
        /// </summary>
        private void InitDragZoomRect(Point pt1, Point pt2)
        {
            SetDragZoomRect(pt1, pt2);

            dragZoomCanvas.Visibility = Visibility.Visible;
            dragZoomBorder.Opacity = 0.5;
        }

        /// <summary>
        /// Update the position and size of the rectangle that user is dragging out.
        /// </summary>
        private void SetDragZoomRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Deterine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle that is being dragged out by the user.
            // The we offset and rescale to convert from content coordinates.
            //
            Canvas.SetLeft(dragZoomBorder, x);
            Canvas.SetTop(dragZoomBorder, y);
            dragZoomBorder.Width = width;
            dragZoomBorder.Height = height;
        }

        /// <summary>
        /// When the user has finished dragging out the rectangle the zoom operation is applied.
        /// </summary>
        private void ApplyDragZoomRect()
        {
            //
            // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
            //
            SavePrevZoomRect();

            //
            // Retreive the rectangle that the user draggged out and zoom in on it.
            //
            double contentX = Canvas.GetLeft(dragZoomBorder);
            double contentY = Canvas.GetTop(dragZoomBorder);
            double contentWidth = dragZoomBorder.Width;
            double contentHeight = dragZoomBorder.Height;
            InfiniteCanvasControl1.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

            FadeOutDragZoomRect();
        }
        //
        // Fade out the drag zoom rectangle.
        //
        private void FadeOutDragZoomRect()
        {
            AnimationHelper.StartAnimation(dragZoomBorder, Border.OpacityProperty, 0.0, 0.1,
                delegate (object sender, EventArgs e)
                {
                    dragZoomCanvas.Visibility = Visibility.Collapsed;
                });
        }

        //
        // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
        //
        private void SavePrevZoomRect()
        {
            prevZoomRect = new Rect(InfiniteCanvasControl1.ContentOffsetX, InfiniteCanvasControl1.ContentOffsetY, InfiniteCanvasControl1.ContentViewportWidth, InfiniteCanvasControl1.ContentViewportHeight);
            prevZoomScale = InfiniteCanvasControl1.ContentScale;
            prevZoomRectSet = true;
        }

        /// <summary>
        /// Clear the memory of the previous zoom level.
        /// </summary>
        private void ClearPrevZoomRect()
        {
            prevZoomRectSet = false;
        }
        #endregion

        #region SelectFunctions


        /// <summary>
        /// Initialise the rectangle that the use is dragging out.
        /// </summary>
        private void InitDragSelectRect(Point pt1, Point pt2)
        {
            SetDragSelectRect(pt1, pt2);

            dragSelectCanvas.Visibility = Visibility.Visible;
            dragSelectBorder.Opacity = 0.5;
        }

        /// <summary>
        /// Update the position and size of the rectangle that user is dragging out.
        /// </summary>
        private void SetDragSelectRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Deterine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle that is being dragged out by the user.
            // The we offset and rescale to convert from content coordinates.
            //
            Canvas.SetLeft(dragSelectBorder, x);
            Canvas.SetTop(dragSelectBorder, y);
            dragSelectBorder.Width = width;
            dragSelectBorder.Height = height;
        }

        /// <summary>
        /// When the user has finished dragging out the rectangle the zoom operation is applied.
        /// </summary>
        private void ApplyDragSelectRect(Point mousePos, bool add = false)
        {
            //
            // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
            //
            //SavePrevSelectRect();

            //
            // Retreive the rectangle that the user draggged out and zoom in on it.
            //
            double contentX = Canvas.GetLeft(dragSelectBorder);
            double contentY = Canvas.GetTop(dragSelectBorder);
            double contentWidth = dragSelectBorder.Width;
            double contentHeight = dragSelectBorder.Height;

            //TODO: Actually select the items in the select rectangle

            if (!add) this.ClearSelection();
            
            BoundingBox selectionBounds = new BoundingBox(contentX, contentY, contentWidth, contentHeight);
            ElementsLinkedList<IElement> _elementsBuffer = DataViewModel.Elements;
            foreach (IRenderable renderable in _elementsBuffer)
            {
                if (renderable != null)
                {
                    if (contentX == mousePos.X)
                    {
                        if (selectionBounds.Contains(renderable.BoundingBox.Location))
                        {
                            this.AddToSelection(renderable);
                        }
                    }
                    else if (contentX < mousePos.X)
                    {
                        //if (MassBool_OR_util(selectionBounds.Contains(renderable.BoundingBox.GetPoints())))
                        if (BoundingBox.CheckIntersection(selectionBounds, renderable.BoundingBox))
                        {
                            this.AddToSelection(renderable);
                        }
                    }
                }
            }

            FadeOutDragSelectRect();
        }

        public static bool MassBool_OR_util(bool[] bools)
        {
            foreach (bool b in bools)
            {
                if (b) return true;
            }
            return false;
        }

        //
        // Fade out the drag zoom rectangle.
        //
        private void FadeOutDragSelectRect()
        {
            AnimationHelper.StartAnimation(dragSelectBorder, Border.OpacityProperty, 0.0, 0.07,
                delegate (object sender, EventArgs e)
                {
                    dragSelectCanvas.Visibility = Visibility.Collapsed;
                });
        }

        ////
        //// Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
        ////
        //private void SavePrevSelectRect()
        //{
        //    prevSelectRect = new Rect(InfiniteCanvasControl1.ContentOffsetX, InfiniteCanvasControl1.ContentOffsetY, InfiniteCanvasControl1.ContentViewportWidth, InfiniteCanvasControl1.ContentViewportHeight);
        //    prevSelectRectSet = true;
        //}

        ///// <summary>
        ///// Clear the memory of the previous zoom level.
        ///// </summary>
        //private void ClearPrevSelectRect()
        //{
        //    prevSelectRectSet = false;
        //}

        #endregion

        TimeOnly lastFrameTime = TimeOnly.FromDateTime(DateTime.Now);
        double fps = 0.0;
        double[] lfps = Array.Empty<double>();
        double avgfps = 0.0;
        private void UpdateFrameStats(object sender, EventArgs e)
        {
            TimeOnly frameTime = TimeOnly.FromDateTime(DateTime.Now);
            fps = 1 / (frameTime - lastFrameTime).TotalSeconds;
            if (lfps.Length < 255)
            {
                lfps = lfps.Concat(new double[] { fps }).ToArray();
            }
            else
            {
                lfps = lfps.Skip(1).Concat(new double[] { fps }).ToArray();
            }
            avgfps = lfps.Average();
            avgfps = Math.Round(avgfps, 3);
            lastFrameTime = frameTime;
        }
    }
}
