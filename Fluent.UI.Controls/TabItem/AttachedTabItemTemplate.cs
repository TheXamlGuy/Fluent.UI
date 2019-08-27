using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TabItem))]
    public class AttachedTabItemTemplate : AttachedItemContainerTemplate<TabItem>
    {
        private bool _isPressed;

        private bool focusable;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsSelected)
            {
                if (!_isPressed && IsPointerOver)
                {
                    visualState = CommonVisualState.SelectedPointerOver;
                }
                else if (_isPressed)
                {
                    visualState = CommonVisualState.SelectedPressed;
                }
                else
                {
                    visualState = CommonVisualState.Selected;
                }
            }
            else if (_isPressed)
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

        protected override void OnAttached()
        {
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
        }

        protected override void OnPointerLeave(object sender, RoutedEventArgs args)
        {
            _isPressed = false;
            ChangeVisualState(true);
        }

        protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, focusable);

            if (_isPressed && args.ButtonState == MouseButtonState.Released)
            {
                _isPressed = false;
                ChangeVisualState(true);

                AttachedFrameworkElement.IsSelected = true;
            }
        }

        protected override void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {
            if (AttachedFrameworkElement.Focusable)
            {
                focusable = true;
                AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            }

            if (args.ButtonState == MouseButtonState.Pressed)
            {
                _isPressed = true;
                ChangeVisualState(true);
            }
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ChangeVisualState(true);
        }
    }
}