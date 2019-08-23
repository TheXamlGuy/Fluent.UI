using Fluent.UI.Core;
using System;
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
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsSelected)
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

            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
            AddEventHandler<MouseButtonEventArgs>("PreviewMouseDown", OnPreviewMouseDown);
            AddEventHandler<MouseButtonEventArgs>("MouseUp", OnMouseUp);
            AddEventHandler<MouseButtonEventArgs>("MouseLeave", OnMouseLeave);

            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsFocusedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, EventArgs args)
        {
            ChangeVisualState(true);
        }

        private void OnMouseLeave(object sender, RoutedEventArgs args)
        {
            _isPressed = false;
            ChangeVisualState(true);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (_isPressed && args.ButtonState == MouseButtonState.Released)
            {
                _isPressed = false;
                ChangeVisualState(true);

                AttachedFrameworkElement.IsSelected = true;
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.ButtonState == MouseButtonState.Pressed)
            {
                args.Handled = true;
                _isPressed = true;

                ChangeVisualState(true);
            }
        }
    }
}