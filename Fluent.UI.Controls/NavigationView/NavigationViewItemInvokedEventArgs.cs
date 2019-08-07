using System;

namespace Fluent.UI.Controls
{
    public class NavigationViewItemInvokedEventArgs : EventArgs
    {
        internal NavigationViewItemInvokedEventArgs(object invokedItem, bool isSettingsInvoked)
        {
            InvokedItem = invokedItem;
            IsSettingsInvoked = isSettingsInvoked;
        }

        public object InvokedItem { get; internal set; }

        public bool IsSettingsInvoked { get; internal set; }
    }
}
