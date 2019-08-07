using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class NavigationViewList : ListBox
    {
        public NavigationViewList()
        {
            DefaultStyleKey = typeof(NavigationViewList);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigationViewItem();
        }
    }
}
