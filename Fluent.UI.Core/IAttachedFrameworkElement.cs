using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public interface IAttachedFrameworkElementTemplate
    {
        void SetAttachedControl(FrameworkElement frameworkElement);
        void SetRequestedTheme(ElementTheme requestedTheme);
        void SetRequestedThemePropagated(ElementTheme requestedTheme);
    }
}
