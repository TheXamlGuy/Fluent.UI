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
        private bool _isPressed;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsSelected)
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
            else
            {
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = CommonVisualState.Disabled;
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
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
            AddEventHandler<MouseButtonEventArgs>("PreviewMouseDown", OnPreviewMouseDown);
            AddEventHandler<MouseButtonEventArgs>("MouseUp", OnMouseUp);
            
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected override void OnPointerLeave(object sender, RoutedEventArgs args)
        {
            OverrideFocusable(true);
            _isPressed = false;
            ChangeVisualState(true);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (_isPressed && args.ButtonState == MouseButtonState.Released)
            {
                OverrideFocusable(true);

                if (AttachedFrameworkElement.IsEnabled)
                {
                    _isPressed = false;
                    ChangeVisualState(true);

                    AttachedFrameworkElement.IsSelected = true;
                }
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (!IsEnabled)
            {
                args.Handled = true;
            }

            if (args.ButtonState == MouseButtonState.Pressed)
            {
                OverrideFocusable();
                _isPressed = true;
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