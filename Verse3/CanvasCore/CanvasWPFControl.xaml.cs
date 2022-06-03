﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Verse3.CanvasCore
{
    /// <summary>
    /// Interaction logic for CanvasWPFControl.xaml
    /// </summary>
    public partial class CanvasWPFControl : UserControl
    {
        public CanvasWPFControl()
        {
            InitializeComponent();
        }

        public CanvasCamera Canvas
        {
            get
            {
                return this.MainCanvas;
            }
        }
    }

    public enum MouseMode
    {
        Nothing,
        Panning,
        Selection,
        PreSelectionRectangle,
        SelectionRectangle,
        DraggingPort,
        ResizingComment,
    }

    public enum WireMode
    {
        Nothing,
        FirstPortSelected
    }

    public class CanvasCamera : Canvas
    {
        public readonly ScaleTransform ScaleTransform = new ScaleTransform();
        public readonly TranslateTransform TranslateTransform = new TranslateTransform();
        public MouseMode MouseMode = MouseMode.Nothing;
        protected Point Origin;
        //public ObservableCollection<Node> SelectedNodes = new ObservableCollection<Node>();
        protected Point Start;
        public WireMode WireMode = WireMode.Nothing;
        
        protected Border _selectionZone;
        protected Point _startPoint;
        protected bool _wiresDisabled;
        public bool NeedsRefresh;
        
        private Point BasisOrigin => new Point(ActualWidth / 2, ActualHeight / 2);

        public CanvasCamera()
        {
            Style s = TryFindResource("VirtualControlStyle") as Style;
            if (s != null) this.Style = s;
            ApplyTemplate();
            MouseWheel += HandleMouseWheel;
            MouseDown += HandleMouseDown;
            PreviewMouseMove += HandleMouseMove;
            RenderTransformOrigin = new Point(0.5, 0.5);

            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.None);
            PreviewMouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
            PreviewKeyDown += OnKeyDown;
            AllowDrop = true;
            Drop += VirtualControl_Drop;
            var init = false;
            Loaded += (sender, args) =>
            {
                if (!init)
                {
                    InitRoot();
                    init = true;
                }
            };
        }
        private void InitRoot()
        {
            //var startNode = new StartNode(this);
            //RootNode = startNode;
            //RootNode.Id = "0";
            //Task.Factory.StartNew(() =>
            //{
            //    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => { GoForNode(RootNode); }));
            //});
        }

        private void VirtualControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                for (var index = 0; index < files.Length; index++)
                {
                    var file = files[index];
                    //var node = new ReadFile(this);
                    //node.OutputPorts[0].Data.Value = file;
                    //AddChildren(node, ActualWidth / 2, ActualHeight / 2);
                    //node.X = node.X - node.ActualWidth;
                }
            }
        }

        public Point UIelementCoordinates(UIElement element)
        {
            var relativePoint = element.TransformToAncestor(this)
                .Transform(new Point(0, 0));
            return relativePoint;
        }

        public void AddChildren(UIElement child)
        {
            Initialize(child);
            if (!Children.Contains(child)) Children.Add(child);
        }

        public void AddChildren(UIElement child, double x, double y)
        {
            Initialize(child);
            if (!Children.Contains(child))
                Children.Add(child);
            SetLeft(child, x);
            SetTop(child, y);
        }

        public void Initialize(UIElement element)
        {
            if (element != null)
            {
                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(ScaleTransform);
                transformGroup.Children.Add(TranslateTransform);
                element.RenderTransform = transformGroup;
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        public void Reset()
        {
            // reset zoom
            ScaleTransform.ScaleX = 1.0;
            ScaleTransform.ScaleY = 1.0;

            // reset pan
            TranslateTransform.X = ActualWidth / 2;
            TranslateTransform.Y = ActualHeight / 2;
        }

        public void ZoomOut(int times = 1)
        {
            for (var i = 0; i < times; i++)
            {
                _zoom -= 0.05;
                if (_zoom < _zoomMin) _zoom = _zoomMin;
                if (_zoom > _zoomMax) _zoom = _zoomMax;
                var scaler = LayoutTransform as ScaleTransform;

                if (scaler == null)
                {
                    scaler = new ScaleTransform(01, 01, Mouse.GetPosition(this).X, Mouse.GetPosition(this).Y);
                    LayoutTransform = scaler;
                }
                var animator = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    To = _zoom
                };
                scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
                scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
            }
        }

        public void ZoomIn(int times = 1)
        {
            for (var i = 0; i < times; i++)
            {
                _zoom += 0.05;
                if (_zoom < _zoomMin) _zoom = _zoomMin;
                if (_zoom > _zoomMax) _zoom = _zoomMax;
                var scaler = LayoutTransform as ScaleTransform;

                if (scaler == null)
                {
                    scaler = new ScaleTransform(01, 01, Mouse.GetPosition(this).X, Mouse.GetPosition(this).Y);
                    LayoutTransform = scaler;
                }
                var animator = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    To = _zoom
                };
                scaler.CenterX = ActualWidth / 2;
                scaler.CenterY = ActualHeight / 2;
                scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
                scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);
            }
        }

        #region Events

        private readonly double _zoomMax = 1.5;
        private readonly double _zoomMin = 0.7;
        private readonly double _zoomSpeed = 0.0005;
        private double _zoom = 0.9;

        protected virtual void HandleMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _zoom += _zoomSpeed * e.Delta;
            if (_zoom < _zoomMin) _zoom = _zoomMin;
            if (_zoom > _zoomMax) _zoom = _zoomMax;
            var scaler = LayoutTransform as ScaleTransform;

            if (scaler == null)
            {                
                scaler = new ScaleTransform(01, 01, e.GetPosition(this).X, e.GetPosition(this).Y);
                LayoutTransform = scaler;
            }
            scaler.CenterX = e.GetPosition(this).X;
            scaler.CenterY = e.GetPosition(this).Y;

            var animator = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                To = _zoom
            };
            scaler.BeginAnimation(ScaleTransform.ScaleXProperty, animator);
            scaler.BeginAnimation(ScaleTransform.ScaleYProperty, animator);

            MouseMode = MouseMode.Nothing;
            e.Handled = true;
        }

        protected virtual void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            Start = e.GetPosition(this);
            Origin = new Point(TranslateTransform.X, TranslateTransform.Y);
            if (MouseMode != MouseMode.Selection && e.ChangedButton == MouseButton.Middle)
            {
                Cursor = Cursors.Hand;
                MouseMode = MouseMode.Panning;
            }
        }

        protected virtual void HandleMouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseMode = MouseMode.PreSelectionRectangle;
        }

        //public Comment SelectedComment;

        protected virtual void HandleMouseMove(object sender, MouseEventArgs e)
        {
            var v = Start - e.GetPosition(this);

            if (MouseMode == MouseMode.Panning)
            {
                TranslateTransform.X = Origin.X - v.X;
                TranslateTransform.Y = Origin.Y - v.Y;

                for (var index = 0; index < Children.Count; index++)
                {
                    var element = Children[index];
                    //if (element is Wire) continue;
                    //if (element is Node)
                    //{
                    //    var node = element as Node;
                    //    node.X = node.X - v.X;
                    //    node.Y = node.Y - v.Y;
                    //}
                }
                Start = e.GetPosition(this);
            }
            else if (MouseMode == MouseMode.Selection)
            {
                //for (var index = 0; index < SelectedNodes.Count; index++)
                //{
                //    var child = SelectedNodes[index];
                //    child.X = child.X - v.X;
                //    child.Y = child.Y - v.Y;
                //}

                Start = e.GetPosition(this);
            }
            //if (SelectedComment != null)
            //{
            //    SelectedComment.X = SelectedComment.X - v.X;
            //    SelectedComment.Y = SelectedComment.Y - v.Y;
            //    Start = e.GetPosition(this);
            //}
        }

        #endregion

        #region EventCaptures

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            switch (keyEventArgs.Key)
            {
                //case Key.Down:
                //    for (var index = 0; index < SelectedNodes.Count; index++)
                //    {
                //        var node = SelectedNodes[index];
                //        node.Y = node.Y + 5;
                //        node.Focus();
                //    }
                //    break;
                //case Key.Up:
                //    for (var index = 0; index < SelectedNodes.Count; index++)
                //    {
                //        var node = SelectedNodes[index];
                //        node.Y = node.Y - 5;
                //        node.Focus();
                //    }
                //    break;
                //case Key.Left:
                //    for (var index = 0; index < SelectedNodes.Count; index++)
                //    {
                //        var node = SelectedNodes[index];
                //        node.X = node.X - 5;
                //        node.Focus();
                //    }
                //    break;
                //case Key.Right:
                //    for (var index = 0; index < SelectedNodes.Count; index++)
                //    {
                //        var node = SelectedNodes[index];
                //        node.X = node.X + 5;
                //        node.Focus();
                //    }
                //    break;
                //case Key.Delete:
                //    if (SelectedNodes.Contains(RootNode))
                //        SelectedNodes.Remove(RootNode);
                //    for (var index = 0; index < SelectedNodes.Count; index++)
                //    {
                //        var node = SelectedNodes[index];
                //        Nodes.Remove(node);
                //        if (node.IsCollapsed == false)
                //            node.Delete();
                //    }
                //    break;

                //case Key.C:
                //    if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift))
                //    {
                //        //This code is still under exp
                //        //var collapsedRegion = new NodesContainer(this, SelectedNodes.ToList());
                //    }
                //    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                //        Copy();
                //    break;
                //case Key.V:
                //    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                //        Paste();
                //    break;
                case Key.Subtract:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        ZoomOut();
                        keyEventArgs.Handled = true;
                    }
                    break;
                case Key.Add:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        ZoomIn();
                        keyEventArgs.Handled = true;
                    }
                    break;
                    //case Key.A:
                    //    SelectedNodes.Clear();
                    //    if (Keyboard.FocusedElement is Node)

                    //        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    //            if (Nodes != null)
                    //                for (var index = 0; index < Nodes.Count; index++)
                    //                {
                    //                    var node = Nodes[index];
                    //                    node.Focus();

                    //                    SelectedNodes.Add(node);
                    //                }

                    //    break;
                    //case Key.F:
                    //    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    //        SearchForm.Show();
                    //    break;
                    //case Key.X:
                    //    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    //    {
                    //        Copy();
                    //        DeleteSelected();
                    //    }
                    //    break;
                    //case Key.Escape:
                    //    if (Keyboard.FocusedElement is Node)
                    //        if (SelectedNodes.Count != 0)
                    //        {
                    //            for (var index = 0; index < SelectedNodes.Count; index++)
                    //            {
                    //                var node = SelectedNodes[index];
                    //                node.IsSelected = false;
                    //            }
                    //            SelectedNodes.Clear();
                    //        }

                    //    break;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            //if (Nodes?.Count > 0) Keyboard.Focus(Nodes[0]);
            if (mouseButtonEventArgs.ChangedButton == MouseButton.Left && mouseButtonEventArgs.ClickCount == 2)
            {
                //if (!Children.Contains(NodesTree))
                //    NodesTree.Show();
                //MouseMode = MouseMode.PreSelectionRectangle;
            }
            else if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = mouseButtonEventArgs.GetPosition(this);

                if (Children.Contains(_selectionZone))
                    Children.Remove(_selectionZone);

                _selectionZone = new Border
                {
                    BorderBrush = Brushes.WhiteSmoke,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(6)
                };

                SetLeft(_selectionZone, _startPoint.X);
                SetTop(_selectionZone, _startPoint.X);

                MouseMode = MouseMode.SelectionRectangle;
                WireMode = WireMode.Nothing;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (WireMode == WireMode.FirstPortSelected && MouseMode == MouseMode.DraggingPort)
            {
                //TempConn.EndPoint = new Point(mouseEventArgs.GetPosition(this).X - 1,
                //    mouseEventArgs.GetPosition(this).Y);
                //if (_wiresDisabled) return;
                //for (var index = 0; index < Children.Count - 1; index++)
                //{
                //    var uielement = Children[index];
                //    if (uielement is Wire)
                //        MagicLaboratory.GrayWiresOut(uielement as Wire);
                //}
                _wiresDisabled = true;
                return;
            }
            if (mouseEventArgs.LeftButton == MouseButtonState.Released)
                if (_wiresDisabled)
                {
                    //for (var index = 0; index < Children.Count - 1; index++)
                    //{
                    //    var uielement = Children[index];
                    //    if (uielement is Wire)
                    //        MagicLaboratory.GrayWiresOut_Reverse(uielement as Wire);
                    //}
                    _wiresDisabled = false;
                }

            if (mouseEventArgs.LeftButton != MouseButtonState.Released && MouseMode == MouseMode.SelectionRectangle)
            {
                if (!Children.Contains(_selectionZone))
                    AddChildren(_selectionZone);
                var pos = mouseEventArgs.GetPosition(this);
                var x = Math.Min(pos.X, _startPoint.X);
                var y = Math.Min(pos.Y, _startPoint.Y);
                var w = Math.Max(pos.X, _startPoint.X) - x;
                var h = Math.Max(pos.Y, _startPoint.Y) - y;
                _selectionZone.Width = w;
                _selectionZone.Height = h;
                SetLeft(_selectionZone, x);
                SetTop(_selectionZone, y);
                return;
            }
            if (MouseMode == MouseMode.ResizingComment && mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                //Cursor = Cursors.SizeNWSE;
                //var currentPoint = Mouse.GetPosition(this);
                //if (currentPoint.Y - TempComment.Y > 0 && currentPoint.X - TempComment.X > 0)
                //{
                //    TempComment.Height = /*TempComment.Top +*/ currentPoint.Y - TempComment.Y;
                //    TempComment.Width = /* TempComment.Left +*/ currentPoint.X - TempComment.X;
                //    TempComment.LocateHandler();
                //}
                //else
                //{
                //    TempComment.Height = 32;
                //    TempComment.Width = 32;
                //    TempComment.LocateHandler();
                //}
                //return;
            }
            if (NeedsRefresh)
            {
                //foreach (var node in Nodes)
                //    node.Refresh();

                //TemExecPort = null;
                //TemObjectPort = null;
                NeedsRefresh = false;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (MouseMode == MouseMode.SelectionRectangle && Children.Contains(_selectionZone))
            {
                Children.Remove(_selectionZone);
                //SelectedNodes.Clear();
                //for (var index = 0; index < Nodes.Count; index++)
                //{
                //    var node = Nodes[index];
                //    if (node.IsSelected) SelectedNodes.Add(node);
                //}

                MouseMode = MouseMode.Nothing;
            }
            if (MouseMode == MouseMode.ResizingComment)
            {
                //TempComment.Cursor = Cursors.Arrow;
                //TempComment = null;
            }
            //HideLinkingPossiblity();
            Cursor = Cursors.Arrow;
            //if (WireMode == WireMode.FirstPortSelected && (TemExecPort != null || TemObjectPort != null))
            //{
            //    NodesTree.Show();
            //    MouseMode = MouseMode.Nothing;
            //    WireMode = WireMode.Nothing;
            //}
            //else
            //{
            //    Children.Remove(TempConn);
            //    MouseMode = MouseMode.Nothing;
            //    WireMode = WireMode.Nothing;
            //}

            //_mouseEffect.Show(this, Colors.DarkGray);
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(300);
            //    Application.Current.Dispatcher.BeginInvoke(new Action(() => { _mouseEffect.Remove(this); }));
            //});
        }

        #endregion
        
        public void IndicateStatus()
        {
            var indicator = new Border
            {
                Width = ActualWidth,
                Height = ActualHeight,
                Background = Brushes.LawnGreen,
                Opacity = 0.1
            };
            AddChildren(indicator, 0, 0);
        }
    }    
}
