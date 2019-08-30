using System.Windows;
using System.Windows.Controls;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class TabItemExtension : FrameworkElementExtension
    {
        internal static readonly DependencyProperty TabPresenterProperty =
            DependencyProperty.RegisterAttached("TabPresenter",
                typeof(TabItem), typeof(TabItemExtension));

        internal static TabItem GetTabPresenter(DependencyObject dependencyObject)
        {
            return (TabItem) dependencyObject.GetValue(TabPresenterProperty);
        }

        internal static void SetTabPresenter(DependencyObject dependencyObject, TabItem tabItem)
        {
            dependencyObject.SetValue(TabPresenterProperty, tabItem);
        }
    }
}