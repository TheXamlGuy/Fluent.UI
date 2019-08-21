using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class AttachedDecoratorTemplate<TDecorator> : AttachedFrameworkElementTemplate<TDecorator> where TDecorator : Decorator
































































    {
        protected override void PrepareRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Decorator) && AttachedFrameworkElement is Decorator decorator)
                {
                    PrepareChildThemeRequest(decorator.Child, requestedTheme);
                }
            }
        }

        private void PrepareChildThemeRequest(UIElement frameworkElement, ElementTheme requestedTheme)
        {
            if (frameworkElement == null)
            {
                return;
            }

            FrameworkElementExtension.SetRequestedThemePropagated(frameworkElement, requestedTheme);
        }
    }
}
