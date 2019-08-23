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

        //protected override IItemsControlExtensionHandler GetItemsControlHandler(ItemsControl itemsControl) { get; } /*=> FrameworkElementExtension<ListBox>.GetAttachedHandler(itemsControl) as IItemsControlExtensionHandler*/;

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
            WeakEventManager<FrameworkElement, MouseButtonEventArgs>.AddHandler(AttachedFrameworkElement, "PreviewMouseDown", OnPreviewMouseDown);
            WeakEventManager<FrameworkElement, MouseButtonEventArgs>.AddHandler(AttachedFrameworkElement, "MouseUp", OnMouseUp);
            WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(AttachedFrameworkElement, "MouseLeave", OnMouseLeave);
        }

        private void OnMouseLeave(object sender, RoutedEventArgs args)
        {
            _isPressed = false;
            ChangeVisualState(true);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, focusable);

            if (_isPressed && args.ButtonState == MouseButtonState.Released)
            {
                _isPressed = false;
                ChangeVisualState(true);

                AttachedFrameworkElement.IsSelected = true;
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs args)
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
    }
}
