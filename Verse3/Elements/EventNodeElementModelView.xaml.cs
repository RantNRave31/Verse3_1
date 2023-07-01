using Core;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Verse3.Elements
{
    /// <summary>
    /// Visual Interaction logic for TestElement.xaml
    /// </summary>
    public partial class EventNodeElementModelView : UserControl, IBaseElementView<IRenderable>
    {
        #region IBaseElementView Members

        private Type Y = default;
        //DEV NOTE: CAUTION! DYNAMIC TYPE IS USED
        private dynamic _element;
        public IRenderable Element
        {
            get
            {
                if (this._element == null)
                {
                    if (this.DataContext != null)
                    {
                        Y = this.DataContext.GetType();
                        if (Y.BaseType.Name == (typeof(EventNodeElementViewModel).Name))
                        {
                            //TODO: Log to Console and process
                            //if (this.DataContext.GetType().GenericTypeArguments.Length == 1)
                            //Y = this.DataContext.GetType().MakeGenericType(Y);
                            //_element = Convert.ChangeType(this.DataContext, U) as IRenderable;
                            Y = this.DataContext.GetType()/*.MakeGenericType(this.DataContext.GetType().GenericTypeArguments[0].GetType())*/;
                            _element = this.DataContext;
                            return _element;
                        }
                        else if (Y.BaseType.Name == typeof(EventNodeElementViewModel).Name)
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
        //public static T ForceCast<T>(object obj)
        //{
        //    try
        //    {
        //        return
        //    }
        //    catch/* (Exception ex)*/
        //    {
        //        //CoreConsole.Log(ex);
        //    }
        //}

        #endregion

        #region Constructor and Render

        public EventNodeElementModelView()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if (this.Element != null && this.Element is IEventNode)
            {
                IEventNode node = this.Element as IEventNode;
                //if (!string.IsNullOrEmpty(this._element.Text))
                //{
                //    if (this.NodeRightText.Text != node.Name && this.NodeLeftText.Text != node.Name)
                //    {
                //        if (node.NodeType == NodeType.Input)
                //        {
                //            this.NodeRightText.Text = node.Name;
                //        }
                //        else if (node.NodeType == NodeType.Output)
                //        {
                //            this.NodeLeftText.Text = node.Name;
                //        }
                //    }
                //}
                if (Element.RenderView != this) Element.RenderView = this;
                if (node.Connections != null)
                {
                    if (node.Connections.Count > 0)
                    {
                        (this._element as EventNodeElementViewModel).NodeContentColor = Brushes.White;
                        foreach (BezierElementViewModel bezier in node.Connections)
                        {
                            if (bezier != null)
                            {
                                if (bezier.Origin == this.Element)
                                {

                                }
                                else if (bezier.Destination == this.Element)
                                {

                                }
                                bezier.RedrawBezier(bezier.Origin, bezier.Destination);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            (this._element as EventNodeElementViewModel).NodeContentColor = Brushes.Transparent;
                            //RenderingCore.Render((this._element as EventNodeElement).Parent);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    try
                    {
                        (this._element as EventNodeElementViewModel).NodeContentColor = Brushes.Transparent;
                        //RenderingCore.Render((this._element as EventNodeElement).Parent);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
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
            if (this.DataContext != null)
            {
                this.Element = this.DataContext as IRenderable;
                Render();
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            //RoutedEventArgs
            if (this.DataContext != null)
            {
                //this._element = this.DataContext as NodeElement;
                //Render();
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Element != null)
            {
                (this.Element as IEventNode).ToggleActive();
            }
            //DEV NOTE: CAUTION! DYNAMIC TYPE IS USED
            //try
            //{
            //    dynamic element = Convert.ChangeType(this.Element, Y);
            //    element.ToggleActive(this, e);
            //}
            //catch (Exception)
            //{
            //    //Log to Console and process
            //    throw;
            //}
        }
    }
}