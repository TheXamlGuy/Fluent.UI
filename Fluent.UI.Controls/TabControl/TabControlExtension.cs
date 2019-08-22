using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    public class TabControlExtension : FrameworkElementExtension
    {
        public static readonly DependencyProperty AddTabButtonCommandParameterProperty =
            DependencyProperty.RegisterAttached("AddTabButtonCommandParameter",
                typeof(object), typeof(TabControlExtension));

        public static readonly DependencyProperty AddTabButtonCommandProperty =
            DependencyProperty.RegisterAttached("AddTabButtonCommand",
                typeof(ICommand), typeof(TabControlExtension));

        public static readonly DependencyProperty IsAddTabButtonVisibleProperty =
            DependencyProperty.RegisterAttached("IsAddTabButtonVisible",
                typeof(bool), typeof(TabControlExtension));

        public static readonly DependencyProperty TabStripFooterProperty =
            DependencyProperty.RegisterAttached("TabStripFooter",
                typeof(object), typeof(TabControlExtension));

        public static readonly DependencyProperty TabStripFooterTemplateProperty =
            DependencyProperty.RegisterAttached("TabStripFooterTemplate",
                typeof(DataTemplate), typeof(TabControlExtension));

        public static readonly DependencyProperty TabStripHeaderProperty =
            DependencyProperty.RegisterAttached("TabStripHeader",
                typeof(object), typeof(TabControlExtension));

        public static readonly DependencyProperty TabStripHeaderTemplateProperty =
            DependencyProperty.RegisterAttached("TabStripHeaderTemplate",
                typeof(DataTemplate), typeof(TabControlExtension));

        public static object GetAddTabButtonCommandParameter(TabControl tabControl) => tabControl.GetValue(AddTabButtonCommandParameterProperty);

        public static ICommand GetAddTabButtonCommand(TabControl tabControl) => (ICommand)tabControl.GetValue(AddTabButtonCommandProperty);

        public static bool GetIsAddTabButtonVisible(TabControl tabControl) => (bool)tabControl.GetValue(IsAddTabButtonVisibleProperty);
       
        public static object GetTabStripFooter(TabControl tabControl) => tabControl.GetValue(TabStripFooterProperty);

        public static DataTemplate GetTabStripFooterTemplate(TabControl tabControl) => (DataTemplate)tabControl.GetValue(TabStripFooterTemplateProperty);

        public static object GetTabStripHeader(TabControl tabControl) => tabControl.GetValue(TabStripHeaderProperty);

        public static DataTemplate GetTabStripHeaderTemplate(TabControl tabControl) => (DataTemplate)tabControl.GetValue(TabStripHeaderTemplateProperty);

        public static void SetAddTabButtonCommandParameter(TabControl tabControl, object value) => tabControl.SetValue(AddTabButtonCommandParameterProperty, value);

        public static void SetAddTabButtonCommand(TabControl tabControl, ICommand value) => tabControl.SetValue(AddTabButtonCommandProperty, value);

        public static void SetIsAddTabButtonVisible(TabControl tabControl, bool value) => tabControl.SetValue(IsAddTabButtonVisibleProperty, value);
       
        public static void SetTabStripFooter(TabControl tabControl, object value) => tabControl.SetValue(TabStripFooterProperty, value);
       
        public static void SetTabStripFooterTemplate(TabControl tabControl, DataTemplate value) => tabControl.SetValue(TabStripFooterTemplateProperty, value);
       
        public static void SetTabStripHeader(TabControl tabControl, object value) => tabControl.SetValue(TabStripHeaderProperty, value);
       
        public static void SetTabStripHeaderTemplate(TabControl tabControl, DataTemplate value) => tabControl.SetValue(TabStripHeaderTemplateProperty, value);
    }
}
