using System;
using System.Windows;
using System.Windows.Media;

namespace Verse3.Elements
{
    //[Serializable]
    public class TextElementViewModel : BaseElement
    {
        #region Properties
                
        //public override ElementType ElementType => ElementType.DisplayUIElement;
        public override Type ViewType { get { return typeof(TextElementModelView); } }
        
        
        private TextAlignment textAlignment;
        public TextAlignment TextAlignment { get => textAlignment; set => SetProperty(ref textAlignment, value); }
        
        private double textRotation;
        public double TextRotation { get => textRotation; set => SetProperty(ref textRotation, value); }

        private string displayedText;
        public string DisplayedText { get => displayedText; set => SetProperty(ref displayedText, value); }

        private FontStyle fontStyle;
        public FontStyle FontStyle { get => fontStyle; set => SetProperty(ref fontStyle, value); }

        private FontFamily fontFamily;
        public FontFamily FontFamily { get => fontFamily; set => SetProperty(ref fontFamily, value); }

        private double fontSize;
        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }

        private FontWeight fontWeight;
        public FontWeight FontWeight { get => fontWeight; set => SetProperty(ref fontWeight, value); }

        private Brush foreground;
        public Brush Foreground { get => foreground; set => SetProperty(ref foreground, value); }

        private Brush background;
        public Brush Background { get => background; set => SetProperty(ref background, value); }

        #endregion

        #region Constructors

        public TextElementViewModel() : base()
        {
            this.FontFamily = new FontFamily("Maven Pro");
            this.FontSize = 12;
            this.FontStyle = FontStyles.Normal;
            this.FontWeight = FontWeights.Normal;
            this.Foreground = Brushes.White;
            this.Background = Brushes.Transparent;
            this.TextAlignment = TextAlignment.Center;
            this.TextRotation = 0;
        }


        #endregion
    }
}