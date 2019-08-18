using System.Windows;

namespace Fluent.UI.Controls
{
    public interface IFrameworkExtensionHandler
    {
        void SetAttachedControl(FrameworkElement frameworkElement);
        void SetRequestedTheme(ElementTheme requestedTheme);
        void SetRequestedThemePropagated(ElementTheme requestedTheme);
    }

    public interface IFrameworkExtensionHandler<TFrameworkElement> : IFrameworkExtensionHandler where TFrameworkElement : FrameworkElement
    {

    }
}
