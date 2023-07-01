using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GKYU.PresentationLogicLibrary.Shapes
{
    public class ShapeFactory
    {
        public static Line Line(double x1, double y1, double x2, double y2, Brush brush, bool snapToPixels = false)
        {
            Line line = new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = brush };
            if (snapToPixels)
            {
                line.SnapsToDevicePixels = true;
                line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            }
            return line;
        }
        public static Rectangle Rectangle(double width, double height, Brush borderBrush, Brush fillBrush, bool snapToPixels = false)
        {
            Rectangle rectangle = new Rectangle { Width = width, Height = height };
            if (borderBrush != null)
                rectangle.Stroke = borderBrush;
            if (fillBrush != null)
                rectangle.Fill = fillBrush;
            if (snapToPixels)
            {
                rectangle.SnapsToDevicePixels = true;
                rectangle.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            }
            return rectangle;
        }
    }
}
