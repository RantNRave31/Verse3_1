namespace Core.Nodes
{
    public class EventArgData : DataStructure<object>
    {
        public EventArgData() : base()
        {
        }
        public EventArgData(object eventargs) : base(eventargs)
        {
            if (_metadata != null)
            {
                _metadata.DataStructurePattern = DataStructurePattern.EventArgData;
            }
        }
        public EventArgData(DataStructure argdata) : base(argdata)
        {
            if (_metadata != null)
            {
                _metadata.DataStructurePattern = DataStructurePattern.EventArgData;
            }
        }

        public new DataStructure Data
        {
            get
            {
                if (Count == 1 && base[0] is DataStructure data)
                    return data;
                else return null;
            }
        }

        public override string ToString()
        {
            if (Count == 1 && base[0] is DataStructure data)
                return data.ToString();
            else return null;
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
