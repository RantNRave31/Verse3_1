using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Verse3.Converters
{
    #region DataTemplateManager
    #endregion

    #region Converters and Utilities

    /// <summary>
    /// Converts a color value to a brush.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    #endregion
}
