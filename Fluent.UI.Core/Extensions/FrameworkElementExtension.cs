using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        public static bool TryIsThemeRequestSupported(this FrameworkElement frameworkElement, out Type supportedType)
        {
            supportedType = null;
            var frameworkElementType = frameworkElement.GetType();
            if (typeof(Panel).IsAssignableFrom(frameworkElementType))
            {
                supportedType = typeof(Panel);
                return true;
            }

            if (typeof(Decorator).IsAssignableFrom(frameworkElementType))
            {
                supportedType = typeof(Decorator);
                return true;
            }

            if (typeof(ItemsControl).IsAssignableFrom(frameworkElementType))
            {
                supportedType = typeof(ItemsControl);
                return true;
            }

            if (typeof(Control).IsAssignableFrom(frameworkElementType))
            {
                supportedType = typeof(Control);
                return true;
            }
            return false;
        }

        public static bool IsThemeRequestSupported(this FrameworkElement frameworkElement)
        {
            var supportedType = frameworkElement.GetType();
            if (typeof(Panel).IsAssignableFrom(supportedType))
            {
                return true;
            }
            else if (typeof(Decorator).IsAssignableFrom(supportedType))
            {
                return true;
            }
            else if (typeof(Control).IsAssignableFrom(supportedType))
            {
                return true;
            }
            return false;
        }
    }
}
