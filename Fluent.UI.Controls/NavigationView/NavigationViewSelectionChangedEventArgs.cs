using System;

namespace Fluent.UI.Controls
{
    public class NavigationViewSelectionChangedEventArgs : EventArgs
    {
        internal NavigationViewSelectionChangedEventArgs(object selectedItem, bool isSettingsSelected)
        {
            SelectedItem = selectedItem;
            IsSettingsSelected = isSettingsSelected;
        }

        public bool IsSettingsSelected { get; internal set; }

        public object SelectedItem { get; internal set; }
    }
}
