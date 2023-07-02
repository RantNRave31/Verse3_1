using System;
using System.Windows;

namespace Verse3.Elements
{
    //[Serializable]
    public class RangeSliderElementViewModel : BaseElement
    {
        public event EventHandler<RoutedEventArgs> ValuesChanged;

        #region Properties
        
        public override Type ViewType => typeof(RangeSliderElementModelView);
        
        #endregion

        #region Constructors

        public RangeSliderElementViewModel() : base()
        {
            this.minimum = -200.0;
            this.maximum = 200.0;
            this.valueStart = -100.0;
            this.valueEnd = 100.0;
            this.tickFrequency = 0.001;
        }

        #endregion

        public void OnValuesChanged(object sender, RoutedPropertyChangedEventArgs<HandyControl.Data.DoubleRange> e)
        {
            this.valueStart = e.NewValue.Start;
            this.valueEnd = e.NewValue.End;
            ValuesChanged?.Invoke(sender, e);
        }

        private double valueStart;

        public double ValueStart { get => valueStart; set => SetProperty(ref valueStart, value); }

        private double valueEnd;

        public double ValueEnd { get => valueEnd; set => SetProperty(ref valueEnd, value); }

        private double tickFrequency;

        public double TickFrequency { get => tickFrequency; set => SetProperty(ref tickFrequency, value); }

        private double minimum;

        public double Minimum { get => minimum; set => SetProperty(ref minimum, value); }

        private double maximum;

        public double Maximum { get => maximum; set => SetProperty(ref maximum, value); }

    }
}