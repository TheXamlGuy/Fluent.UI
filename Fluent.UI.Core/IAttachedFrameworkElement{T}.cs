using System.Windows;

namespace Fluent.UI.Core
{
    public interface IAttachedFrameworkElementTemplate<TFrameworkElement> : IAttachedFrameworkElementTemplate where TFrameworkElement : FrameworkElement
    {
    }
}