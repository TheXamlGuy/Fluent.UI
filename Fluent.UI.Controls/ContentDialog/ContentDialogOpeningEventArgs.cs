using System;

namespace Fluent.UI.Controls
{
    public class ContentDialogOpeningEventArgs : EventArgs
    {
        internal ContentDialogOpeningEventArgs(object parameter)
        {
            Parameter = parameter;
        }

        public bool Cancel { get; set; }

        public object Parameter { get; internal set; }
    }
}
