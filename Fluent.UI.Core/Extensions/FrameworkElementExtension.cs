using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Core.Extensions
{
    public static class FrameworkElementExtension
    {
        public static IEnumerable<VisualStateGroup> FindVisualStateGroups(this FrameworkElement parent)
        {
            var visualStateGroups = (IEnumerable<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(parent);
            if (visualStateGroups == null)
            {
                return Enumerable.Empty<VisualStateGroup>();
            }

            return visualStateGroups;
        }

        public static VisualTransition FindVisualTransition(this FrameworkElement parent, string name)
        {
            var visualStateGroups = (Collection<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(parent);
            return visualStateGroups?.SelectMany(visualStateGroup => visualStateGroup.Transitions.Cast<VisualTransition>()).FirstOrDefault(visualTransition => visualTransition.To == name);
        }
    }
}
