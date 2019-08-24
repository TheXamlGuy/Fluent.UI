using System.Windows;

namespace Fluent.UI.Core
{
    public interface IAttachedFrameworkElementTemplate
    {
        void SetAttachedControl(FrameworkElement frameworkElement);
    }
}