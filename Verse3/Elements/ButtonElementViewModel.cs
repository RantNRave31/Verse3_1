using System;
using System.Windows;
using System.Windows.Media;

namespace Verse3.Elements
{
    //[Serializable]
    public class ButtonElementViewModel : BaseElement
    {

        #region Properties

        public event EventHandler<RoutedEventArgs> OnButtonClicked;
        
        public override Type ViewType => typeof(ButtonElementModelView);
        
        private object displayedText;
        public object DisplayedText { get => displayedText; set => SetProperty(ref displayedText, value); }

        private System.Windows.Media.Brush backgroundColor;
        public System.Windows.Media.Brush BackgroundColor { get => backgroundColor; set => SetProperty(ref backgroundColor, value); }

        #endregion

        #region Constructors

        public ButtonElementViewModel() : base()
        {
            this.DisplayedText = "Button";
            this.BackgroundColor = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff6700"));
        }

        #endregion

        internal void ButtonClicked(object sender, RoutedEventArgs e)
        {
            OnButtonClicked.Invoke(sender, e);
        }

    }
}