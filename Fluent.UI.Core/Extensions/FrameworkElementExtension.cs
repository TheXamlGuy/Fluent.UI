using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Core.Extensions
{
    public static class FrameworkElementExtension
    {
        public static VisualTransition GetVisualTransition(this FrameworkElement frameworkElement, string name)
        {
            var visualStateGroups = (Collection<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(frameworkElement);
            return visualStateGroups?.SelectMany(visualStateGroup => visualStateGroup.Transitions.Cast<VisualTransition>()).FirstOrDefault(visualTransition => visualTransition.To == name);
        }
    }
}
