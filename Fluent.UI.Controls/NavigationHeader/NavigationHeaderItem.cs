using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class NavigationHeaderItem : ListBoxItem
    {
        public static readonly DependencyProperty PageNameProperty =
            DependencyProperty.Register(nameof(PageName),
                typeof(string), typeof(NavigationHeaderItem),
                new PropertyMetadata(null));

        public NavigationHeaderItem()
        {
            DefaultStyleKey = typeof(NavigationHeaderItem);
        }

        public string PageName
        {
            get => (string)GetValue(PageNameProperty);
            set => SetValue(PageNameProperty, value);
        }
    }
}
