using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class NavigationHeaderList : ListBox
    {
        public NavigationHeaderList()
        {
            DefaultStyleKey = typeof(NavigationHeaderList);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NavigationHeaderItem();
        }
    }
}
