using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class ItemsControlExtensionHandler<TItemsControl> : FrameworkElementExtensionHandler<TItemsControl> where TItemsControl : ItemsControl
    {
        protected virtual Type GetContainerTypeForItem()
        {
            return null;
        }

        protected override void PrepareRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(ItemsControl) && AttachedFrameworkElement is ItemsControl itemsControl)
                {
                    PrepareItemsContainerRequestedTheme(itemsControl, requestedTheme);
                }
            }
        }

        private void PrepareItemsContainerRequestedTheme(ItemsControl itemsControl, ElementTheme requestedTheme)
        {
            var itemContainerType = GetContainerTypeForItem();
            var itemContainerStylw = RequestedThemeFactory.Current.Create(itemContainerType, requestedTheme);

            AttachedFrameworkElement.SetCurrentValue(ItemsControl.ItemContainerStyleProperty, itemContainerStylw);

            var itemsControlType = AttachedFrameworkElement.GetType();
            var itemsControlStyle = RequestedThemeFactory.Current.Create(itemsControlType, requestedTheme);

            AttachedFrameworkElement.SetCurrentValue(FrameworkElement.StyleProperty, itemsControlStyle);

            AttachedFrameworkElement.UpdateLayout();
            AttachedFrameworkElement.UpdateDefaultStyle();

            OnAttached();
            ChangeVisualState(true);
        }
    }
}
