using Fluent.UI.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ListBoxItem))]
    public class AttachedListBoxItemTemplate : AttachedItemContainerTemplate<ListBoxItem>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsSelected)
            {
                if (!IsPressed && IsPointerOver)
                {
                    visualState = CommonVisualState.SelectedPointerOver;
                }
                else if (IsPressed)
                {
                    visualState = CommonVisualState.SelectedPressed;
                }
                else
                {
                    visualState = CommonVisualState.Selected;
                }
            }
            else
            {
                if (!AttachedFrameworkElement.IsEnabled)
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
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected override void OnPointerLeave(object sender, RoutedEventArgs args)
        {
            OverrideFocusable(true);
            base.OnPointerLeave(sender, args);
            ChangeVisualState(true);
        }

        protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {
            if (args.ButtonState == MouseButtonState.Released)
            {
                OverrideFocusable(true);

                if (AttachedFrameworkElement.IsEnabled)
                {
                    base.OnPointerReleased(sender, args);
                    ChangeVisualState(true);

                    AttachedFrameworkElement.IsSelected = true;
                }
            }
        }

        protected override void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {
            if (!IsEnabled)
            {
                args.Handled = true;
            }

            if (args.ButtonState == MouseButtonState.Pressed)
            {
                OverrideFocusable();
                base.OnPointerPressed(sender, args);
                ChangeVisualState(true);
            }
        }

        private void OverrideFocusable(bool isFocusable = false)
        {
            if (AttachedFrameworkElement.Focusable)
            {
                AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, isFocusable);
            }
        }
    }
}