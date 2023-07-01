using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace GKYU.PresentationLogicLibrary.Behaviors
{
    class TreeViewDragBlendBehavior : Behavior<TreeView>
    {
        private TreeView _dragSource;
        private object _dragData;
        private Point _dragStart;

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseLeftButtonDown += TreeViewOnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseMove += TreeViewOnPreviewMouseMove;
            AssociatedObject.PreviewMouseLeftButtonUp += TreeViewOnPreviewMouseLeftButtonUp;
        }

        private void TreeViewOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _dragStart = mouseButtonEventArgs.GetPosition(null);
            _dragSource = sender as TreeView;
            if (_dragSource == null) return;
            var i = IndexUnderDragCursor;
            _dragData = i != -1 ? _dragSource.Items.GetItemAt(i) : null;
        }

        void TreeViewOnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragData == null) return;

            var currentPosition = e.GetPosition(null);
            var difference = _dragStart - currentPosition;

            if ((MouseButtonState.Pressed == e.LeftButton) &&
                ((Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                 (Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                var data = new DataObject("Custom", _dragData);
                DragDrop.DoDragDrop(_dragSource, data, DragDropEffects.Copy);

                _dragData = null;
            }
        }

        private void TreeViewOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _dragData = null;
        }

        int IndexUnderDragCursor
        {
            get
            {
                var index = -1;
                for (var i = 0; i < AssociatedObject.Items.Count; ++i)
                {
                    var item = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;

                    if (item != null && item.IsMouseOver)
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
        }
    }
}
