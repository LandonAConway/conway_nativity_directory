using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Conway_Nativity_Directory
{
    public class NativityVirtualizingStackPanel : VirtualizingStackPanel
    {
        protected override void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
        {
            var item = e.UIElement as ListBoxItem;
            if (item != null && item.IsSelected)
            {
                e.Cancel = true;
                e.Handled = true;
                return;
            }

            var item2 = e.UIElement as TreeViewItem;
            if (item2 != null && item2.IsSelected)
            {
                e.Cancel = true;
                e.Handled = true;
                return;
            }

            var item3 = e.UIElement as Nativity;
            if (item3 != null && item3.IsSelected)
            {
                e.Cancel = true;
                e.Handled = true;
                return;
            }

            base.OnCleanUpVirtualizedItem(e);
        }
    }
}
