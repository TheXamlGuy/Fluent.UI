using Fluent.UI.Core;
using System.Windows;

namespace Fluent.UI.Controls
{
    public abstract partial class FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> : DependencyObject where TFrameworkElement : FrameworkElement where TFrameworkElementExtension : FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>, new()
    {
        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached("IsAttached",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>),
                new PropertyMetadata(false, OnIsAttachedPropertyChanged));

        internal static readonly DependencyProperty IsRequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("IsRequestedThemePropagated",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>),
                new PropertyMetadata((ElementTheme)(int)ApplicationExtension.RequestedTheme, OnRequestedThemePropertyChanged));

        internal static readonly DependencyProperty RequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("RequestedThemePropagated",
                typeof(ElementTheme), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>),
                new PropertyMetadata(ElementTheme.Default, OnRequestedThemePropertyChanged));

        internal static DependencyProperty AttachedFrameworkElementProperty =
            DependencyProperty.RegisterAttached("AttachedFrameworkElement",
              typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>));

        public static ElementTheme GetRequestedTheme(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemeProperty);

        public static void SetRequestedTheme(DependencyObject dependencyObject, ElementTheme value)
        {
            dependencyObject.SetValue(IsRequestedThemeProperty, true);
            dependencyObject.SetValue(RequestedThemeProperty, value);
        }

        internal static FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> GetAttachedFrameworkElement(DependencyObject dependencyObject) => (FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>)dependencyObject.GetValue(AttachedFrameworkElementProperty);

        internal static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        internal static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);

        internal static bool GetIsRequestedThemePropagated(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemePropagatedProperty);

        internal static void SetAttachedFrameworkElement(DependencyObject dependencyObject, FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> extension) => dependencyObject.SetValue(AttachedFrameworkElementProperty, extension);

        internal static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        internal static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);

        internal static void SetIsRequestedThemePropagated(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemePropagatedProperty, value);
    }
}
