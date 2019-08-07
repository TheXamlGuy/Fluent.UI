using System;

namespace Fluent.UI.Controls
{
    public sealed class FlyoutBaseClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}