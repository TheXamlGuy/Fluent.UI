using Fluent.UI.Core;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class ButtonExtension : ControlExtension<Button, ButtonExtension>
    {
        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedControl, ButtonBase.IsEnabledProperty, () => ChangeVisualState(true));
            handler.Add(AttachedControl, ButtonBase.IsPressedProperty, () => ChangeVisualState(true));
            handler.Add(AttachedControl, ButtonBase.IsMouseOverProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!AttachedControl.IsEnabled)
            {
                visualState = VisualStates.StateDisabled;
            }
            else if (AttachedControl.IsPressed)
            {
                visualState = VisualStates.StatePressed;
            }
            else if (AttachedControl.IsMouseOver)
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
