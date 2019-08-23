using Fluent.UI.Core;
using System;
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

        //protected override IItemsControlExtensionHandler GetItemsControlHandler(ItemsControl itemsControl) { get; } /*=> FrameworkElementExtension<ListBox>.GetAttachedHandler(itemsControl) as IItemsControlExtensionHandler*/;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsSelected)
            {
                if (!_isPressed && IsMouseOver)
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
                else if (IsMouseOver)
                {
                    visualState = CommonVisualState.PointerOver;
                }
                else
                {
                    visualState = CommonVisualState.Normal;
                }
            }


            Debug.WriteLine(visualState);
            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
            AddEventHandler<MouseButtonEventArgs>("PreviewMouseDown", OnPreviewMouseDown);
            AddEventHandler<MouseButtonEventArgs>("MouseUp", OnMouseUp);
            AddEventHandler<RoutedEventArgs>("MouseLeave", OnMouseLeave);

            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsFocusedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ChangeVisualState(true);
        }

        private void OnMouseLeave(object sender, RoutedEventArgs args)
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