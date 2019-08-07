using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class NavigationViewItem : ListBoxItem
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon),
                typeof(IconElement), typeof(NavigationViewItem),
                new PropertyMetadata(null));

        public static readonly DependencyProperty PageNameProperty =
            DependencyProperty.Register(nameof(PageName),
                typeof(string), typeof(NavigationViewItem),
                new PropertyMetadata(null));

        public NavigationViewItem()
        {
            DefaultStyleKey = typeof(NavigationViewItem);
           
        }

        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string PageName
        {
            get => (string)GetValue(PageNameProperty);
            set => SetValue(PageNameProperty, value);
        }
    }
}
