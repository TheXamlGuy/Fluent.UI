using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class AttachedControlTemplate<TControl> : AttachedFrameworkElementTemplate<TControl> where TControl : Control
    {
        protected void ApplyRequestedTheme(ElementTheme requestedTheme)
        {
            var elementType = AttachedFrameworkElement.GetType();
            var style = RequestedThemeFactory.Current.Create(elementType, requestedTheme);

            AttachedFrameworkElement.SetCurrentValue(FrameworkElement.StyleProperty, style);
            AttachedFrameworkElement.UpdateLayout();
            AttachedFrameworkElement.UpdateDefaultStyle();

            OnAttached();
            ChangeVisualState(true);
        }

        protected override void PrepareRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Control))
                {
                    ApplyRequestedTheme(requestedTheme);
                }
            }
        }
    }
}
