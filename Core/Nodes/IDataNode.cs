namespace Core.Nodes
{
    public interface IDataNode : INode, IDataGooContainer
    {
        //public event EventHandler<DataChangedEventArgs> DataChanged;
        public void ToggleActive();
    }

    public interface IDataNode<D> : IDataNode
    {
        //public new event EventHandler<DataChangedEventArgs<D>> DataChanged;
        public delegate void NodeDataChangedEventHandler(IDataNode<D> container, DataChangedEventArgs<D> e);
        public event NodeDataChangedEventHandler NodeDataChanged;
        new DataStructure<D> DataGoo { get; set; }
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
