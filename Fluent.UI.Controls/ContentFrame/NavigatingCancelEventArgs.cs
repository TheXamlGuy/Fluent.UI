using System;
using System.Windows.Navigation;

namespace Fluent.UI.Controls.ContentFrame
{
    public sealed class NavigatingCancelEventArgs : EventArgs
    {
        public bool Cancel { get; set; }

        public NavigationMode NavigationMode { get; internal set; }

        public Type SourcePageType { get; internal set; }

        public object Parameter { get; internal set; }
    }
}