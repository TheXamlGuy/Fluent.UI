using System.Windows;

namespace Fluent.UI.Controls
{
    public class NavigationViewHeader : DependencyObject
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header),
                typeof(object), typeof(NavigationViewHeader),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate),
                typeof(DataTemplate), typeof(NavigationViewHeader),
                new PropertyMetadata(null));

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }
    }
}
