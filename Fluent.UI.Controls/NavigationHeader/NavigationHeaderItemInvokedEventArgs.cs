using System;

namespace Fluent.UI.Controls
{
    public class NavigationHeaderItemInvokedEventArgs : EventArgs
    {
        internal NavigationHeaderItemInvokedEventArgs(object invokedItem)
        {
            InvokedItem = invokedItem;
        }

        public object InvokedItem { get; internal set; }
    }
}
