using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    internal class ButtonExtension : ControlExtension<Button, ButtonExtension>
    {
        public ButtonExtension()
        {

        }

        internal override void ChangeVisualState(bool useTransitions = true)
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

        internal override void OnPointerPressed(object sender, MouseButtonEventArgs args) => ChangeVisualState();

        internal override void OnPointerOver(object sender, MouseEventArgs args) => ChangeVisualState();
    }
}
