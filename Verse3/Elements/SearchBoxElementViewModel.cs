using System;
using System.Windows.Controls;

namespace Verse3.Elements
{
    //[Serializable]
    public class SearchBoxElementViewModel : BaseElement
    {
        #region Properties

        public override Type ViewType => typeof(SearchBoxElementModelView);

        private string inputText;
        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }
        public EventHandler<TextChangedEventArgs> ValueChanged { get; set; }
        public EventHandler<HandyControl.Data.FunctionEventArgs<string>> SearchStarted { get; set; }

        #endregion

        #region Constructors

        public SearchBoxElementViewModel() : base()
        {
            this.InputText = "<empty>";
        }

        #endregion
    }
}