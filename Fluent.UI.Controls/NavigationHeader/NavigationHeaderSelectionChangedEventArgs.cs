using System;

namespace Fluent.UI.Controls
{
    public class NavigationHeaderSelectionChangedEventArgs : EventArgs
    {
        internal NavigationHeaderSelectionChangedEventArgs(object selectedItem)
        {
            SelectedItem = selectedItem;
        }

        public object SelectedItem { get; internal set; }
    }
}
