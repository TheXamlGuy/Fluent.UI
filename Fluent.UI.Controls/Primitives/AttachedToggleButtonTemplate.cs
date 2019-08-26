using System.Windows;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class AttachedToggleButtonTemplate<TToggleButton> : AttachedButtonBaseTemplate<TToggleButton> where TToggleButton : ToggleButton
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AddPropertyChangedHandler(ToggleButton.IsCheckedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
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
