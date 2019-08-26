using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(CheckBox))]
    public class AttachedCheckBoxTemplate : AttachedToggleButtonTemplate<CheckBox>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState = "";
            if (IsChecked == true)
            {
                if (!IsEnabled)
                {
                    visualState = "CheckedDisabled";
                }
                else if (IsPressed)
                {
                    visualState = "CheckedPressed";
                }
                else if (IsMouseOver)
                {
                    visualState = "CheckedPointerOver";
                }
                else
                {
                    visualState = "CheckedNormal";
                }
            }

            if (IsChecked == null)
            {
                if (!IsEnabled)
                {
                    visualState = "IndeterminateDisabled";
                }
                else if (IsPressed)
                {
                    visualState = "IndeterminatePressed";
                }
                else if (IsMouseOver)
                {
                    visualState = "IndeterminatePointerOver";
                }
                else
                {
                    visualState = "IndeterminateNormal";
                }
            }

            if (IsChecked == false)
            {
                if (!IsEnabled)
                {
                    visualState = "UncheckedDisabled";
                }
                else if (IsPressed)
                {
                    visualState = "UncheckedPressed";
                }
                else if (IsMouseOver)
                {
                    visualState = "UncheckedPointerOver";
                }
                else
                {
                    visualState = "UncheckedNormal";
                }
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnIsCheckedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);
    }
}