using Core.Elements;
using Newtonsoft.Json;

namespace Core.Nodes
{
    public interface IConnection : IElement
    {
        public INode Origin { get; }
        public INode Destination { get; }
        [JsonIgnore]
        public ConnectionType ConnectionType { get; }
        public new ElementType ElementType { get/* => ElementType.Connection*/; }
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
