using System;
using System.Windows;

namespace Verse3.Elements
{
    //[Serializable]
    public class IntegerSliderElementViewModel : BaseElement
    {
        #region Properties

        public override Type ViewType => typeof(IntegerSliderElementModelView);
        
        private int minimum;
        public int Minimum { get => minimum; set => SetProperty(ref minimum, value); }

        private int maximum;
        public int Maximum { get => maximum; set => SetProperty(ref maximum, value); }

        private int _value;
        public int Value { get => _value;
            set => SetProperty(ref _value, value); }

        private double tickFrequency;
        public double TickFrequency { get => tickFrequency; set => SetProperty(ref tickFrequency, value); }

        #endregion
        
        #region Constructors

        public IntegerSliderElementViewModel() : base()
        {
            this.Minimum = 0;
            this.Maximum = 100;
            this.Value = 50;
            this.TickFrequency = 0.001;
        }

        #endregion

        public event EventHandler<RoutedPropertyChangedEventArgs<int>> ValueChanged;
        public void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            this.Value = (int)e.NewValue;
            this.ValueChanged.Invoke(sender, e);
        }
    }
}