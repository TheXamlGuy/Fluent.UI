using Fluent.UI.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(CheckBox))]
    public class AttachedCheckBoxTemplate : AttachedControlTemplate<CheckBox>
    {
        protected override void OnAttached()
        {
            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(ButtonBase.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(ToggleButton.IsCheckedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, EventArgs args)
        {
            ChangeVisualState(true);
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState = "";
            if (AttachedFrameworkElement.IsChecked == true)
            {
                if (!IsEnabled)
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