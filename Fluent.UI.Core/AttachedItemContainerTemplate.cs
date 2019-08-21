using Fluent.UI.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class AttachedItemContainerTemplate<TItemContainer> : AttachedControlTemplate<TItemContainer> where TItemContainer : Control
    {
       // private IItemsControlExtensionHandler _handler;

        protected ItemsControl Parent { get; private set; }

       // protected virtual IItemsControlExtensionHandler GetItemsControlHandler(ItemsControl itemsControl) => null;
        
        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
            Parent = AttachedFrameworkElement.FindParent<ItemsControl>();
          //  _handler = GetItemsControlHandler(Parent);

            base.OnLoaded(sender, args);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs args)
        {
            //if (_handler.IsItemContainerUpdating)
            //{
            //    _handler.IsItemContainerUpdating = true;

            //    args.Handled = true;
            //    return;
            //}

            base.OnUnloaded(sender, args);
        }
    }
}
