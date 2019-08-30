using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class TreeViewItemExtension : FrameworkElementExtension
    {
        internal static DependencyProperty TemplateSettingsProperty =
            DependencyProperty.RegisterAttached("TemplateSettings",
                typeof(TreeViewItemTemplateSettings), typeof(TreeViewItemExtension));

        internal static DependencyProperty ItemIndentLengthProperty =
            DependencyProperty.RegisterAttached("ItemIndentLength",
                typeof(double), typeof(TreeViewItemExtension));

        internal static double GetItemIndentLength(TreeViewItem item) => (double)item.GetValue(ItemIndentLengthProperty);

        internal static void SetItemIndentLength(TreeViewItem item, double length) => item.SetValue(ItemIndentLengthProperty, length);

        internal static TreeViewItemTemplateSettings GetTemplateSettings(TreeViewItem item) => (TreeViewItemTemplateSettings)item.GetValue(TemplateSettingsProperty);

        internal static void SetTemplateSettings(TreeViewItem item, TreeViewItemTemplateSettings templateSettings) => item.SetValue(TemplateSettingsProperty, templateSettings);
    }
}
