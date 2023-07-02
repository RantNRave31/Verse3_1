using System;
using System.Windows;

namespace Verse3.Elements
{
    //[Serializable]
    public class SliderElementViewModel : BaseElement
    {
        #region Properties

        public override Type ViewType => typeof(SliderElementModelView);
        
        private double minimum;
        public double Minimum { get => minimum; set => SetProperty(ref minimum, value); }

        private double maximum;
        public double Maximum { get => maximum; set => SetProperty(ref maximum, value); }

        private double _value;
        public double Value { get => _value;
            set => SetProperty(ref _value, value); }

        private double tickFrequency;
        public double TickFrequency { get => tickFrequency; set => SetProperty(ref tickFrequency, value); }

        #endregion
        
        #region Constructors

        public SliderElementViewModel() : base()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.Value = 50;
            this.TickFrequency = 0.001;
        }

        #endregion

        public event EventHandler<RoutedPropertyChangedEventArgs<double>> ValueChanged;
        public void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Value = (double)e.NewValue;
            this.ValueChanged.Invoke(sender, e);
        }
    }
}