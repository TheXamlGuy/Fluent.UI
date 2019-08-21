using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class ControlExtensionHandler<TControl> : FrameworkElementExtensionHandler<TControl> where TControl : Control
    {
        protected void ApplyRequestedTheme(ElementTheme requestedTheme)
        {
            var elementType = AttachedFrameworkElement.GetType();
            var style = RequestedThemeFactory.Current.Create(elementType, requestedTheme);

            AttachedFrameworkElement.SetCurrentValue(FrameworkElement.StyleProperty, style);
            AttachedFrameworkElement.UpdateLayout();
            AttachedFrameworkElement.UpdateDefaultStyle();

            OnAttached();
            ChangeVisualState(true);
        }

        protected override void PrepareRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Control))
                {
                    ApplyRequestedTheme(requestedTheme);
                }
            }
        }
    }

    public class ItemContainerExtensionHandler<TItemContainer> : ControlExtensionHandler<TItemContainer> where TItemContainer : Control
    {
        private IItemsControlExtensionHandler _handler;

        protected ItemsControl Parent { get; private set; }

        protected virtual IItemsControlExtensionHandler GetItemsControlHandler(ItemsControl itemsControl) => null;
        
        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            Parent = AttachedFrameworkElement.FindParent<ItemsControl>();
            _handler = GetItemsControlHandler(Parent);

            base.OnLoaded(sender, args);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs args)
        {
            if (_handler.IsItemContainerUpdating)
            {
                _handler.IsItemContainerUpdating = true;

                args.Handled = true;
                return;
            }

            base.OnUnloaded(sender, args);
        }
    }
}
