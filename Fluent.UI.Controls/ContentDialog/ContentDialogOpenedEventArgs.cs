using System;

namespace Fluent.UI.Controls
{
    public class ContentDialogOpenedEventArgs : EventArgs
    {
        public ContentDialogOpenedEventArgs(object parameter)
        {
            Parameter = parameter;
        }

        public object Parameter { get; internal set; }
    }
}
