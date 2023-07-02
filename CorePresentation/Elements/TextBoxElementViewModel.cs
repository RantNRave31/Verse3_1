using System;
using System.Windows.Controls;

namespace Verse3.Elements
{
    //[Serializable]
    public class TextBoxElementViewModel : BaseElement
    {
        #region Properties

        public override Type ViewType => typeof(TextBoxElementModelView);

        private string inputText;
        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }
        public EventHandler<TextChangedEventArgs> ValueChanged { get; set; }

        #endregion

        #region Constructors

        public TextBoxElementViewModel() : base()
        {
            this.InputText = "<empty>";
        }

        #endregion
    }
}