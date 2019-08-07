using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class ContentTile : ContentControl
    {
        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title),
                typeof(string), typeof(ContentTile),
                new PropertyMetadata(null));

        public static DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(nameof(TitleTemplate),
                typeof(DataTemplate), typeof(ContentTile),
                new PropertyMetadata(null));

        public object Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public ContentTile()
        {
            DefaultStyleKey = typeof(ContentTile);
        }
    }
}
