using Fluent.UI.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    public class AttachedButtonBaseTemplate<TButtonBase> : AttachedControlTemplate<TButtonBase>
        where TButtonBase : ButtonBase
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (IsPressed)
            {
                visualState = CommonVisualState.Pressed;
            }
            else if (IsPointerOver)
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
