using Fluent.UI.Core;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ComboBox))]
    public class AttachedComboBoxTemplate : AttachedControlTemplate<ComboBox>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
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
