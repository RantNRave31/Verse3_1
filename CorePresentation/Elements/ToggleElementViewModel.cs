using System;
using System.Windows;

namespace Verse3.Elements
{
    //[Serializable]
    public class ToggleElementViewModel : BaseElement
    {

        #region Properties

        public event EventHandler<RoutedEventArgs> ToggleChecked;
        public event EventHandler<RoutedEventArgs> ToggleUnchecked;

        public override Type ViewType => typeof(ToggleElementModelView);
        
        private object displayedText;
        public object DisplayedText { get => displayedText; set => SetProperty(ref displayedText, value); }
        
        #endregion

        #region Constructors

        public ToggleElementViewModel() : base()
        {
            this.DisplayedText = "Toggle";
        }

        #endregion

        internal void OnChecked(object sender, RoutedEventArgs e)
        {
            Value = true;
            this.ToggleChecked?.Invoke(sender, e);
        }
        internal void OnUnchecked(object sender, RoutedEventArgs e)
        {
            Value = false;
            this.ToggleUnchecked?.Invoke(sender, e);
        }

        private bool? value1;
        public bool? Value { get => value1; set => SetProperty(ref value1, value); }
    }
}