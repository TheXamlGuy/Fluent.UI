using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class ButtonExtension : FrameworkElementExtension<Button, ButtonExtension>
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
            if (!AttachedFrameworkElement.IsEnabled)
            {
                visualState = VisualStates.StateDisabled;
            }
            else if (AttachedFrameworkElement.IsPressed)
            {
                visualState = VisualStates.StatePressed;
            }
            else if (AttachedFrameworkElement.IsMouseOver)
            {
                visualState = VisualStates.StatePointerOver;
            }
            else
            {
                visualState = VisualStates.StateNormal;
            }

            GoToVisualState(visualState, useTransitions);
        }
    }
}
