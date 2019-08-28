using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Core
{
    public class AttachedItemsControlTemplate<TItemsControl> : AttachedControlTemplate<TItemsControl>, IAttachedItemsControlTemplate where TItemsControl : ItemsControl
    {
        public bool IsItemContainerUpdating { get; set; }

        protected virtual Type GetContainerTypeForItem()
        {
            return null;
        }

        protected override void OnAttached()
        {
            AttachedFrameworkElement.LayoutUpdated -= OnItemsControlLayoutUpdated;
            AttachedFrameworkElement.LayoutUpdated += OnItemsControlLayoutUpdated;

            base.OnAttached();
        }

        private void PrepareItemsContainerRequestedTheme(ElementTheme requestedTheme)
        {
            var itemsControlType = AttachedFrameworkElement.GetType();
            var itemsControlStyle = RequestedThemeFactory.Current.Create(itemsControlType, requestedTheme);

            var itemContainerType = GetContainerTypeForItem();
            var itemContainerStyle = RequestedThemeFactory.Current.Create(itemContainerType, requestedTheme);

            AttachedFrameworkElement.SetValue(ItemsControl.ItemContainerStyleProperty, itemContainerStyle);
            AttachedFrameworkElement.SetValue(FrameworkElement.StyleProperty, itemsControlStyle);

            AttachedFrameworkElement.UpdateLayout();
            AttachedFrameworkElement.UpdateDefaultStyle();

            OnAttached();
            ChangeVisualState(true);
        }

        private void OnItemsControlLayoutUpdated(object sender, EventArgs args)
        {
            IsItemContainerUpdating = true;
        }

        protected override void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {

        }

        protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {

        }

        protected override void OnPointerLeave(object sender, RoutedEventArgs args)
        {

        }
    }
}