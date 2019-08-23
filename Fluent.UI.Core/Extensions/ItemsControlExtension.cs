using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core.Extensions
{
    public static class ItemsControlExtension
    {
        public static ListBoxItem GetListBoxItem(this ItemsControl itemsControl, DependencyObject dependencyObject)
        {
            if (itemsControl == null || dependencyObject == null)
            {
                return null;
            }

            return ItemsControl.ContainerFromElement(itemsControl, dependencyObject) as ListBoxItem;
        }
    }
}