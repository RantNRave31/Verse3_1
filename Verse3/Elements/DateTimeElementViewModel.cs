using System;
using System.Windows;

namespace Verse3.Elements
{
    //[Serializable]
    public class DateTimeElementViewModel : BaseElement
    {
        public event EventHandler<RoutedEventArgs> DateTimeChanged;

        #region Properties
        
        public override Type ViewType => typeof(DateTimeElementModelView);
        
        private object displayedText;
        public object DisplayedText { get => displayedText; set => SetProperty(ref displayedText, value); }
        
        #endregion

        #region Constructors

        public DateTimeElementViewModel() : base()
        {
            this.DisplayedText = "Toggle";
        }

        #endregion

        public void OnSelectedDateTimeChanged(object sender, HandyControl.Data.FunctionEventArgs<DateTime?> e)
        {
            Value = e.Info;
            DateTimeChanged?.Invoke(sender, e);
        }

        
        private DateTime? value1;
        public DateTime? Value { get => value1; set => SetProperty(ref value1, value); }
    }
}