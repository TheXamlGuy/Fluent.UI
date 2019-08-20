using Fluent.UI.Core.Extensions;
using System;
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

            var returnValue = itemsControl.GetType().GetMethods();
            // var style = RequestedThemeFactory.Current.Create(elementType, requestedTheme);

        }

    }
}
