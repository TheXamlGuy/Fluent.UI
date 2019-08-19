using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class ButtonExtensionHandler : FrameworkElementExtensionHandler<Button>
    {
        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsEnabledProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, ButtonBase.IsPressedProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsPressed)
            {
                visualState = CommonVisualState.Pressed;
            }
            else if (IsMouseOver)
            {
                visualState = CommonVisualState.PointerOver;
            }
            else
            {
                visualState = CommonVisualState.Normal;
            }

            GoToVisualState(visualState, useTransitions);
        }
    }
}