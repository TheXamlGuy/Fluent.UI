using System;

namespace Fluent.UI.Controls
{
    public sealed class ItemDoubleClickEventArgs : EventArgs
    {
        public object ClickedItem { get; internal set; }
    }
}