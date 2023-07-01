using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Verse3.Components;
using Verse3.Elements;

namespace Verse3.Components
{
    /// <summary>
    /// Interaction logic for BaseCompView.xaml
    /// </summary>
    public partial class BaseCompModelView : UserControl, IBaseCompView<BaseCompViewModel>
    {


        #region IBaseElementView Members

        private Type Y = default;
        //DEV NOTE: CAUTION! DYNAMIC TYPE IS USED
        private dynamic _element;
        public BaseCompViewModel Element
        {
            get
            {
                if (this._element == null)
                {
                    if (this.DataContext != null)
                    {
                        Y = this.DataContext.GetType();
                        if (Y.BaseType.Name == (typeof(BaseCompViewModel).Name))
                        {
                            //TODO: Log to Console and process
                            //if (this.DataContext.GetType().GenericTypeArguments.Length == 1)
                            //Y = this.DataContext.GetType().MakeGenericType(Y);
                            //_element = Convert.ChangeType(this.DataContext, U) as IRenderable;
                            Y = this.DataContext.GetType()/*.MakeGenericType(this.DataContext.GetType().GenericTypeArguments[0].GetType())*/;
                            _element = this.DataContext;
                            return _element;
                        }
                    }
                }
                return _element;
            }
            private set
            {
                if (Y != default)
                {
                    if (value.GetType().IsAssignableTo(Y))
                    {
                        _element = value;
                    }
                }
            }
        }
        IRenderable IRenderView.Element => Element;

        #endregion
        public BaseCompModelView()
        {
            if (this.DataContext is BaseCompViewModel) this.Element = (BaseCompViewModel)this.DataContext;
            InitializeComponent();
            Render();
        }

        public void Render()
        {
            if (this.Element != null)
            {
                if (this.Element.RenderView != this) this.Element.RenderView = this;
                this.Element.RenderComp();
                
                //InputsList.ItemsSource = this.Element.ChildElementManager.InputSide;
                //OutputsList.ItemsSource = this.Element.ChildElementManager.OutputSide;
                //if (this.Element.Width != (InputsList.ActualWidth + OutputsList.ActualWidth + CenterBar.Width))
                //{


                //    else
                //    {
                //        CenterBar.Width = 50;
                //        this.Element.Width = (InputsList.ActualWidth + OutputsList.ActualWidth + CenterBar.Width);
                //    }
                //}
                //if (this.Element.Height != MainStackPanel.ActualHeight) this.Element.Height = MainStackPanel.ActualHeight;
                //TODO: else log to console
            }
        }


        #region MouseEvents

        /// <summary>
        /// Event raised when a mouse button is clicked down over a Rectangle.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MouseButtonEventArgs
            MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements.Focus();
            Keyboard.Focus(MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements);

            BaseCompModelView compView = (BaseCompModelView)sender;
            BaseCompViewModel comp = compView.Element;
            
            if (e.ChangedButton == MouseButton.Left)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.AddToSelection(comp);
                else if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Deselect(comp);
                else if (!comp.IsSelected) MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.Select(comp);

                if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0 && MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements.SelectedItems.Count > 0)
                {
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode = MouseHandlingMode.CopyDraggingElements;
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.origContentMouseDownPoint = e.GetPosition(MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements);
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.mouseButtonDown = e.ChangedButton;

                    compView.CaptureMouse();

                    e.Handled = true;
                    return;
                }
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode != MouseHandlingMode.None)
                {
                    // We are in some other mouse handling mode, don't do anything.
                    return;
                }

                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode = MouseHandlingMode.DraggingElements;
                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.origContentMouseDownPoint = e.GetPosition(MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements);
                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.mouseButtonDown = e.ChangedButton;

                compView.CaptureMouse();

                e.Handled = true;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                //TODO: Mouse down Right & Middle
            }
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                //MouseButtonEventArgs
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode != MouseHandlingMode.DraggingElements)
                {
                    // We are not in rectangle dragging mode.
                    return;
                }

                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode = MouseHandlingMode.None;

                if (sender is BaseCompModelView compView)
                {
                    compView.ReleaseMouseCapture();
                }

                e.Handled = true;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (this.Element.ContextMenu != null)
                {
                    this.Element.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                    this.Element.ContextMenu.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// Event raised when the mouse cursor is moved when over a Rectangle.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //MouseEventArgs
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode == MouseHandlingMode.CopyDraggingElements && MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements.SelectedItems.Count > 0)
                {
                    List<BaseCompViewModel> copiedComps = new List<BaseCompViewModel>();
                    foreach (BaseCompViewModel comp in MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements.SelectedItems)
                    {
                        BaseCompViewModel comp1 = Activator.CreateInstance(comp.GetType()) as BaseCompViewModel;
                        ParameterInfo[] pi = comp1.GetCompInfo().ConstructorInfo.GetParameters();
                        object[] args = new object[pi.Length];
                        for (int i = 0; i < pi.Length; i++)
                        {
                            if (!(pi[i].DefaultValue is DBNull)) args[i] = pi[i].DefaultValue;
                            else
                            {
                                if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "x")
                                    args[i] = MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.GetMouseRelPosition().X;
                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().X;
                                else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
                                    args[i] = MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.GetMouseRelPosition().Y;
                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().Y;
                            }
                        }
                        BaseCompViewModel comp1instance = comp1.GetCompInfo().ConstructorInfo.Invoke(args) as BaseCompViewModel;
                        DataTemplateManager.RegisterDataTemplate(comp1instance);
                        DataViewModel.DataModel.Elements.Add(comp1instance);

                        copiedComps.Add(comp1instance);
                    }
                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ClearSelection();
                    foreach (BaseCompViewModel comp in copiedComps) MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.AddToSelection(comp);

                    MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode = MouseHandlingMode.DraggingElements;
                }
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.MouseHandlingMode != MouseHandlingMode.DraggingElements)
                {
                    //
                    // We are not in rectangle dragging mode, so don't do anything.
                    //
                    return;
                }

                Point curContentPoint = e.GetPosition(MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ContentElements);
                Vector rectangleDragVector = curContentPoint - MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.origContentMouseDownPoint;

                //
                // When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
                //

                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.origContentMouseDownPoint = curContentPoint;

                //TODO: If other BaseComps are also selected, Render all selected BaseComps together with rectangleDragVector translation
                if (MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.LBcontent.SelectedItems.Count > 1)
                {
                    foreach (IRenderable renderable in MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.LBcontent.SelectedItems)
                    {
                        if (renderable != null)
                        {
                            RenderPipeline.RenderRenderable(renderable, rectangleDragVector.X, rectangleDragVector.Y);
                        }
                    }
                }
                else
                {
                    RenderPipeline.RenderRenderable(this.Element, rectangleDragVector.X, rectangleDragVector.Y);
                }

                MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.ExpandContent();

                e.Handled = true;
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
                //throw ex;
            }
            
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
            Render();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            Render();
        }

        #endregion
    }
}
