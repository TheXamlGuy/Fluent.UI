using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TabItem))]
    public class AttachedTabItemTemplate : AttachedItemContainerTemplate<TabItem>
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

        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsEnabledProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, TabItem.IsSelectedProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsFocusedProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void OnAttached()
        {
            AttachedFrameworkElement.AddHandler(UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown, true);
            AttachedFrameworkElement.AddHandler(UIElement.MouseUpEvent, (MouseButtonEventHandler)OnMouseUp, true);
            AttachedFrameworkElement.AddHandler(UIElement.MouseLeaveEvent, (RoutedEventHandler)OnMouseLeave, true);
        }  

        protected override void OnDetached()
        {
            AttachedFrameworkElement.RemoveHandler(UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown);
            AttachedFrameworkElement.RemoveHandler(UIElement.MouseUpEvent, (MouseButtonEventHandler)OnMouseUp);
            AttachedFrameworkElement.RemoveHandler(UIElement.MouseLeaveEvent, (RoutedEventHandler)OnMouseLeave);
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

                _isPressed = false;
                ChangeVisualState(true);

                AttachedFrameworkElement.IsSelected = true;
            }
        }

        private void OverrideFocusable(bool value)
        {
            if (AttachedFrameworkElement.Focusable)
            {
                AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, value);
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
        {
            OverrideFocusable(false);

            _isPressed = true;
            ChangeVisualState(true);
        }
    }
}
