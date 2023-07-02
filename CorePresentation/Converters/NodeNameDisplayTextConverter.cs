using Core;
using Core.Nodes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Verse3.Elements;
using Verse3.Nodes;

namespace Verse3.Converters
{
    //public class NodeNameDisplaySideConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value is NodeType)
    //        {
    //            if ((NodeType)value == NodeType.Input)
    //            {
    //                if (parameter is bool)
    //                {
    //                    if (!(bool)parameter)
    //                    {
    //                        return true;
    //                    }
    //                }
    //            }
    //            else if ((NodeType)value == NodeType.Output)
    //            {
    //                if (parameter is bool)
    //                {
    //                    if ((bool)parameter)
    //                    {
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //        return false;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class NodeNameDisplayTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is FrameworkElement)
            {
                FrameworkElement f = values[0] as FrameworkElement;
                //bool.TryParse((string)parameter, out bool b);
                bool b = f.Name.Contains("Right");
                if (values[1] is RenderPipelineInfo)
                {
                    IRenderable renderable = ((RenderPipelineInfo)values[1]).Renderable;
                    if (renderable != null && renderable is INode)
                    {
                        INode n = (INode)renderable;
                        if (n.NodeType == NodeType.Input)
                        {
                            if (b)
                            {
                                //Set Width
                                if (renderable.RenderView is EventNodeElementModelView)
                                {
                                    TextBlock t = (renderable.RenderView as EventNodeElementModelView).NodeRightText;
                                    t.Text = n.Name;
                                    t.UpdateLayout();
                                    renderable.Width = t.ActualWidth + 50;
                                }
                                else if (renderable.RenderView is DataNodeElementModelView)
                                {
                                    TextBlock t = (renderable.RenderView as DataNodeElementModelView).NodeRightText;
                                    t.Text = n.Name;
                                    t.UpdateLayout();
                                    renderable.Width = t.ActualWidth + 50;
                                }
                                return n.Name;
                            }
                        }
                        else if (n.NodeType == NodeType.Output)
                        {
                            if (!b)
                            {
                                //Set Width
                                if (renderable.RenderView is EventNodeElementModelView)
                                {
                                    TextBlock t = (renderable.RenderView as EventNodeElementModelView).NodeLeftText;
                                    t.Text = n.Name;
                                    t.UpdateLayout();
                                    renderable.Width = t.ActualWidth + 50;
                                }
                                else if (renderable.RenderView is DataNodeElementModelView)
                                {
                                    TextBlock t = (renderable.RenderView as DataNodeElementModelView).NodeLeftText;
                                    t.Text = n.Name;
                                    t.UpdateLayout();
                                    renderable.Width = t.ActualWidth + 50;
                                }
                                return n.Name;
                            }
                        }
                    }
                }
            }
            return "";
            //if (parameter is bool)
            //{
            //    if ((bool)parameter)
            //    {
            //        if (value is string)
            //        {
            //            return (string)value;
            //        }
            //    }
            //}
            //return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}