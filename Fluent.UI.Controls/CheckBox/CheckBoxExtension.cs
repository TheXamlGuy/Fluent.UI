using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public partial class CheckBoxExtension : FrameworkElementExtension<CheckBox, CheckBoxExtension>
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
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = "CheckedDisabled";
                }
                else if (AttachedFrameworkElement.IsPressed)
                {
                    visualState = "CheckedPressed";
                }
                else if (AttachedFrameworkElement.IsMouseOver)
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
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = "IndeterminateDisabled";
                }
                else if (AttachedFrameworkElement.IsPressed)
                {
                    visualState = "IndeterminatePressed";
                }
                else if (AttachedFrameworkElement.IsMouseOver)
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
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = "UncheckedDisabled2";
                }
                else if (AttachedFrameworkElement.IsPressed)
                {
                    visualState = "UncheckedPressed";
                }
                else if (AttachedFrameworkElement.IsMouseOver)
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
