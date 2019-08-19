using System.Windows;

namespace Fluent.UI.Core
{
    public interface IFrameworkExtensionHandler
    {
        void SetAttachedControl(FrameworkElement frameworkElement);
        void SetRequestedTheme(ElementTheme requestedTheme);
        void SetRequestedThemePropagated(ElementTheme requestedTheme);
    }
}
