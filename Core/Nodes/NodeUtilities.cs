using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Nodes
{

    public static class NodeUtilities
    {
        public static bool CheckCompatibility(INode origin, INode destination)
        {
            if (origin.NodeType == NodeType.Input && destination.NodeType == NodeType.Output
                || origin.NodeType == NodeType.Output && destination.NodeType == NodeType.Input)
            {
                if (origin is IDataNode && destination is IDataNode)
                {
                    if ((origin as IDataNode).DataValueType == (destination as IDataNode).DataValueType)
                    {
                        return true;
                    }
                    else if ((destination as IDataNode).DataValueType.IsAssignableFrom((origin as IDataNode).DataValueType))
                    {
                        return true;
                    }
                    else if ((origin as IDataNode).DataValueType.IsAssignableFrom((destination as IDataNode).DataValueType))
                    {
                        //CAST!!
                        return true;
                    }
                }
                else if (origin is IEventNode && destination is IEventNode)
                {
                    if ((origin as IEventNode).EventArgData != null && (destination as IEventNode).EventArgData != null)
                    {
                        if ((origin as IEventNode).EventArgData.DataType == (destination as IEventNode).EventArgData.DataType)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //TODO: Warn user that event data type does not match or are null!!!!
                        //DISALLOW if a flag is true
                        return true;
                    }
                }
            }
            return false;
        }
    }

    //public class HorizontalAlignmentConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((NodeType)value == NodeType.Input)
    //            return HorizontalAlignment.Left;
    //        else if ((NodeType)value == NodeType.Output)
    //            return HorizontalAlignment.Right;
    //        else return HorizontalAlignment.Left;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
