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
using Verse3.CorePresentation.Workspaces;
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
            WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements.Focus();
            Keyboard.Focus(WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements);

            BaseCompModelView compView = (BaseCompModelView)sender;
            BaseCompViewModel comp = compView.Element;
            
            if (e.ChangedButton == MouseButton.Left)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0) WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.AddToSelection(comp);
                else if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.Deselect(comp);
                else if (!comp.IsSelected) WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.Select(comp);

                if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0 && WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements.SelectedItems.Count > 0)
                {
                    WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode = MouseHandlingMode.CopyDraggingElements;
                    WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.origContentMouseDownPoint = e.GetPosition(WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements);
                    WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.mouseButtonDown = e.ChangedButton;

                    compView.CaptureMouse();

                    e.Handled = true;
                    return;
                }
                if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode != MouseHandlingMode.None)
                {
                    // We are in some other mouse handling mode, don't do anything.
                    return;
                }

                 WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode = MouseHandlingMode.DraggingElements;
                WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.origContentMouseDownPoint = e.GetPosition(WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements);
                WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.mouseButtonDown = e.ChangedButton;

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
                if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode != MouseHandlingMode.DraggingElements)
                {
                    // We are not in rectangle dragging mode.
                    return;
                }

                 WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode = MouseHandlingMode.None;

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
                if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode == MouseHandlingMode.CopyDraggingElements && WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements.SelectedItems.Count > 0)
                {
                    List<BaseCompViewModel> copiedComps = new List<BaseCompViewModel>();
                    foreach (BaseCompViewModel comp in WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements.SelectedItems)
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
                                    args[i] = WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.GetMouseRelPosition().X;
                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().X;
                                else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
                                    args[i] = WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.GetMouseRelPosition().Y;
                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().Y;
                            }
                        }
                        BaseCompViewModel comp1instance = comp1.GetCompInfo().ConstructorInfo.Invoke(args) as BaseCompViewModel;
                        DataTemplateManager.RegisterDataTemplate(comp1instance);
                        WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.Elements.Add(comp1instance);

                        copiedComps.Add(comp1instance);
                    }
                     WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ClearSelection();
                    foreach (BaseCompViewModel comp in copiedComps) WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.AddToSelection(comp);

                    WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode = MouseHandlingMode.DraggingElements;
                }
                if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.MouseHandlingMode != MouseHandlingMode.DraggingElements)
                {
                    //
                    // We are not in rectangle dragging mode, so don't do anything.
                    //
                    return;
                }

                Point curContentPoint = e.GetPosition(WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ContentElements);
                Vector rectangleDragVector = curContentPoint - WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.origContentMouseDownPoint;

                //
                // When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
                //

                WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.origContentMouseDownPoint = curContentPoint;

                //TODO: If other BaseComps are also selected, Render all selected BaseComps together with rectangleDragVector translation
                if (WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.LBcontent.SelectedItems.Count > 1)
                {
                    foreach (IRenderable renderable in WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.LBcontent.SelectedItems)
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

                 WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataModelView.ExpandContent();

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
