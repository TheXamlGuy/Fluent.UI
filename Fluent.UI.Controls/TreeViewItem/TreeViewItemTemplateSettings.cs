using System.Windows;

namespace Fluent.UI.Controls
{
    public class TreeViewItemTemplateSettings : DependencyObject
    {
        public static readonly DependencyProperty ItemIndentThickessDeltaProperty =
            DependencyProperty.Register(nameof(ItemIndentThickessDelta),
                typeof(Thickness), typeof(TreeViewItemTemplateSettings));

        public Thickness ItemIndentThickessDelta
        {
            get => (Thickness)GetValue(ItemIndentThickessDeltaProperty);
            set => SetValue(ItemIndentThickessDeltaProperty, value);
        }
    }
}
