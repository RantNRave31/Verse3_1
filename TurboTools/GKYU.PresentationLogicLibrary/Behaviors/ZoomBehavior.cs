using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace GKYU.PresentationLogicLibrary.Behaviors
{
    public class ZoomBehavior
        : Behavior<ScrollViewer>
    {
        double _scale = 1;
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
        }

        private void PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (null != scrollViewer)
            {
                int delta = e.Delta;
                int sign = (delta < 0) ? -1 : 1;
                _scale += (0.1 * sign);
                Point p = e.MouseDevice.GetPosition(scrollViewer);
                if (_scale < 1 && _scale > -1)
                    ((Canvas)scrollViewer.Content).RenderTransform = new ScaleTransform(_scale, _scale, p.X, p.Y);
                else
                    ((Canvas)scrollViewer.Content).LayoutTransform = new ScaleTransform(_scale, _scale, p.X, p.Y);

            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
            }
        }

    }
}
