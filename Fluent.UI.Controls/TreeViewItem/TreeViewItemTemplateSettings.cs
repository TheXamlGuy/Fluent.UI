using System.Windows;

namespace Fluent.UI.Controls
{
    public class TreeViewItemTemplateSettings : DependencyObject
    {
        public static readonly DependencyProperty ItemIndentThicknessDeltaProperty =
            DependencyProperty.Register(nameof(ItemIndentThicknessDelta),
                typeof(Thickness), typeof(TreeViewItemTemplateSettings));

        public Thickness ItemIndentThicknessDelta
        {
            get => (Thickness) GetValue(ItemIndentThicknessDeltaProperty);
            set => SetValue(ItemIndentThicknessDeltaProperty, value);
        }
    }
}