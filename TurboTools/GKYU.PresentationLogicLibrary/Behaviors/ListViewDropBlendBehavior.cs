using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace GKYU.PresentationLogicLibrary.Behaviors
{
    class ListViewDropBlendBehavior 
        : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += AssociatedObjectOnDrop;
        }

        private void AssociatedObjectOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            var dropTarget = sender as ListView;
            if ((dropTarget != null) && (dragEventArgs.Data.GetDataPresent("Custom")))
            {
                dropTarget.Items.Add(dragEventArgs.Data.GetData("Custom"));
            }
        }
    }
}
