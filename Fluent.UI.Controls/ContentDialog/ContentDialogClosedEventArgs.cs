using System;

namespace Fluent.UI.Controls
{
    public sealed class ContentDialogClosedEventArgs : EventArgs
    {
        public ContentDialogResult Result { get; internal set; }
    }
}