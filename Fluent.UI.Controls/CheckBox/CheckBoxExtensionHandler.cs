using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class CheckBoxExtensionHandler : FrameworkElementExtensionHandler<CheckBox>
    {
        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsEnabledProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, ButtonBase.IsPressedProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, ToggleButton.IsCheckedProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState = "";
            if (AttachedFrameworkElement.IsChecked == true)
            {
                if (IsEnabled)
                {
                    visualState = "CheckedDisabled";
                }
                else if (AttachedFrameworkElement.IsPressed)
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

            if (AttachedFrameworkElement.IsChecked == null)
            {
                if (!IsEnabled)
                {
                    visualState = "IndeterminateDisabled";
                }
                else if (AttachedFrameworkElement.IsPressed)
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

            if (AttachedFrameworkElement.IsChecked == false)
            {
                if (!IsEnabled)
                {
                    visualState = "UncheckedDisabled";
                }
                else if (AttachedFrameworkElement.IsPressed)
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
    }
}
