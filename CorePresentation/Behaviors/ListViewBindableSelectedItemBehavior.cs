using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Verse3.Behaviors
{
    /*
<ListBox>
    <e:Interaction.Behaviors>
        <behaviours:BindableSelectedItemBehavior SelectedItem="{Binding SelectedItem, Mode=TwoWay}" />
    </e:Interaction.Behaviors>
</ListBox>
     */
    public class ListBoxBindableSelectedItemBehavior : Behavior<ListBox>
    {
        #region SelectedItem Property

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(ListBoxBindableSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var item = e.NewValue as ListBoxItem;
            if (item != null)
            {
                item.SetValue(ListBoxItem.IsSelectedProperty, true);
            }
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                this.SelectedItem = e.AddedItems[0];
            }
            else
            {
                this.SelectedItem = null;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            }
        }
    }
}
