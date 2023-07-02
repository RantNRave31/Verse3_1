using Core.Elements;
using Newtonsoft.Json;
using static Core.Geometry2D;

namespace Core.Nodes
{
    public interface INode : IElement
    {
        [JsonIgnore]
        public IElement Parent { get; set; }
        [JsonIgnore]
        public ElementsLinkedList<IConnection> Connections { get; }
        [JsonIgnore]
        public NodeType NodeType { get; }
        [JsonIgnore]
        public CanvasPoint Hotspot { get; }
        public string Name { get; set; }
        public new ElementType ElementType { get/* => ElementType.Node*/; }
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
