using System.Windows;

namespace Fluent.UI.Core
{
    public interface IAttachedFrameworkElementTemplate
    {
        void SetAttachedControl(FrameworkElement frameworkElement);

        void SetRequestedTheme(ElementTheme requestedTheme);

        void SetRequestedThemePropagated(ElementTheme requestedTheme);
    }
}