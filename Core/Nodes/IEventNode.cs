namespace Core.Nodes
{
    public interface IEventNode : INode, IComputable
    {
        public delegate void NodeEventHandler(IEventNode container, EventArgData e);
        public event NodeEventHandler NodeEvent;
        public EventArgData EventArgData { get; set; }
        public void TriggerEvent(EventArgData e);
        public bool EventOccured(EventArgData e);
        void ToggleActive();
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
