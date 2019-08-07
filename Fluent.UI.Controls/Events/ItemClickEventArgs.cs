using System;

namespace Fluent.UI.Controls
{
    public sealed class ItemClickEventArgs : EventArgs
    {
        public object ClickedItem { get; internal set; }
    }
}
