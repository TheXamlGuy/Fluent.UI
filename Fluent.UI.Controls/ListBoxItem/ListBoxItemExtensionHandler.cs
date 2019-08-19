using Fluent.UI.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    public class ListBoxItemExtensionHandler : FrameworkElementExtensionHandler<ListBoxItem>
    {
        private bool _isPressed;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState = "";
            if (!AttachedFrameworkElement.IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (!AttachedFrameworkElement.IsSelected)
            {
                if (!_isPressed && AttachedFrameworkElement.IsMouseOver)
                {
                    visualState = CommonVisualState.PointerOver;
                }
                else if (_isPressed)
                {
                    visualState = CommonVisualState.Pressed;
                }
            }
            else if(AttachedFrameworkElement.IsSelected)
            {
                if (!_isPressed && AttachedFrameworkElement.IsMouseOver)
                {
                    visualState = CommonVisualState.SelectedPointerOver;
                }
                else if(_isPressed)
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
                visualState = CommonVisualState.Normal;
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsEnabledProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, ListBoxItem.IsSelectedProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsFocusedProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            AttachedFrameworkElement.AddHandler(UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown, true);
            AttachedFrameworkElement.AddHandler(UIElement.MouseUpEvent, (MouseButtonEventHandler)OnMouseUp, true);

            base.OnLoaded(sender, args);
        }

        protected override void OnUnloaded()
        {
            AttachedFrameworkElement.RemoveHandler(UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown);
            AttachedFrameworkElement.RemoveHandler(UIElement.MouseUpEvent, (MouseButtonEventHandler)OnMouseUp);

            base.OnUnloaded();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (args.ChangedButton == MouseButton.Left)
            {
                _isPressed = false;
                ChangeVisualState(true);

                AttachedFrameworkElement.IsSelected = true;
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            args.Handled = true;
            _isPressed = true;

            ChangeVisualState(true);
        }
    }
}
