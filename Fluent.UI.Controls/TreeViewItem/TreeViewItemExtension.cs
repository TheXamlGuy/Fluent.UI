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

        internal static TreeViewItemTemplateSettings GetTemplateSettings(TreeViewItem item) => (TreeViewItemTemplateSettings)item.GetValue(TemplateSettingsProperty);

        internal static void SetTemplateSettings(TreeViewItem item, TreeViewItemTemplateSettings templateSettings) => item.SetValue(TemplateSettingsProperty, templateSettings);
    }
}
