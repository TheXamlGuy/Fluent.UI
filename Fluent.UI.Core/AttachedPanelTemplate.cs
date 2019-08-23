using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class AttachedPanelTemplate<TPanel> : AttachedFrameworkElementTemplate<TPanel> where TPanel : Panel
    {
        protected override void PrepareRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Panel) && AttachedFrameworkElement is Panel panel)
                {
                    foreach (FrameworkElement child in panel.Children)
                    {
                        PrepareChildThemeRequest(child, requestedTheme);
                    }
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