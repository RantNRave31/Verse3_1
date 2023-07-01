using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors;

namespace GKYU.PresentationLogicLibrary.Behaviors
{
    public class DragBehavior : Behavior<UIElement>
    {
        private Canvas canvas;
        private bool isDragging = false;
        private Point mouseStartPosition;
        private TranslateTransform transform = new TranslateTransform();
        private Rectangle startCaret;

        protected override void OnAttached()
        {
            base.OnAttached();
            canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
            AssociatedObject.RenderTransform = transform;

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }
        private void AssociatedObject_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseStartPosition = e.GetPosition(AssociatedObject);
            isDragging = true;
            Point point = e.GetPosition(canvas);
            startCaret = new Rectangle { Width = 5, Height = 5, Fill = Brushes.Black };
            Canvas.SetLeft(startCaret, point.X);
            Canvas.SetTop(startCaret, point.Y);
            canvas.Children.Add(startCaret);
            this.AssociatedObject.CaptureMouse();
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                Point point = e.GetPosition(canvas);
                transform.X = point.X - mouseStartPosition.X;
                transform.Y = point.Y - mouseStartPosition.Y;
            }
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                this.AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
                canvas.Children.Remove(startCaret);
                startCaret = null;
            }
        }

    }
}
