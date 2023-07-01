using GKYU.BusinessLogicLibrary.Bitmaps;
using GKYU.PresentationLogicLibrary.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GKYU.PresentationLogicLibrary.ModelViews
{
    /// <summary>
    /// Interaction logic for BitmapModelView.xaml
    /// </summary>
    public partial class BitmapModelView : UserControl
    {
        ScaleTransform _scaleTransform;
        private BitmapFileModel.Factory bitmapFactory;
        private BitmapFileModel bitmapModel;
        public BitmapModelView()
        {
            InitializeComponent();
            imageViewer.LayoutTransform = _scaleTransform = new ScaleTransform();
        }
        private void Command_OnTest(object sender, object e)
        {
            //MessageBox.Show("Test Executed");
            for (int n = 0; n < 50; n++)
            {
                //tempCanvas.Children.Add(ShapeFactory.Line(n * 10, 0, n * 10, tempCanvas.ActualHeight, Brushes.Black, true));
                Rectangle rectangle = null;
                Brush brush = null;
                if (n % 4 == 0)
                    brush = Brushes.Black;
                else if (n % 4 == 1)
                {
                    brush = new LinearGradientBrush { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };
                    ((LinearGradientBrush)brush).GradientStops.Add(new GradientStop(Colors.Black, 0));
                    ((LinearGradientBrush)brush).GradientStops.Add(new GradientStop(Colors.White, 1));
                }
                else if (n % 4 == 2)
                    brush = Brushes.White;
                else
                {
                    brush = new LinearGradientBrush { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };
                    ((LinearGradientBrush)brush).GradientStops.Add(new GradientStop(Colors.White, 0));
                    ((LinearGradientBrush)brush).GradientStops.Add(new GradientStop(Colors.Black, 1));

                }
                rectangle = ShapeFactory.Rectangle(20, tempCanvas.ActualHeight, null, brush);
                rectangle.SetValue(Canvas.TopProperty, (double)0);
                rectangle.SetValue(Canvas.LeftProperty, (double)n * 20);
                tempCanvas.Children.Add(rectangle);

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bitmapFactory = new BitmapFileModel.Factory();
            bitmapModel = bitmapFactory.Bitmap(640, 480, 96, 96, PixelFormats.Bgr32);
            bitmapModel.BeginRender();
            bitmapModel.Clear();
            bitmapModel.EndRender();
            imageViewer.Source = bitmapModel.mWriteableBitmap;
        }
    }
}
