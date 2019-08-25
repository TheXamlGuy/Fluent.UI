using Fluent.UI.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class MenuItemExtension : FrameworkElementExtension
    {
        public static Dictionary<MenuItem, string> ElementToGroupNames = new Dictionary<MenuItem, string>();

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.RegisterAttached("GroupName",
                typeof(string), typeof(MenuItemExtension),
                new PropertyMetadata(null, OnGroupNamePropertyChanged));

        public static void SetGroupName(MenuItem menuItem, string value) => menuItem.SetValue(GroupNameProperty, value);

        public static string GetGroupName(MenuItem menuItem) => (string)menuItem.GetValue(GroupNameProperty);

        private static void OnGroupNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (TryAttachTemplate(dependencyObject as FrameworkElement, out AttachedMenuItemTemplate attachedTemplate))
            {
                attachedTemplate.SetGroupName((string)args.NewValue);
            }
        }
    }
}