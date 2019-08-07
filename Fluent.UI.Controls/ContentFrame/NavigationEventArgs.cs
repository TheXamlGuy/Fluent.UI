using System;
using System.Windows.Navigation;

namespace Fluent.UI.Controls.ContentFrame
{
    public sealed class NavigationEventArgs : EventArgs
    {
        public object Content { get; internal set; }
        public NavigationMode NavigationMode { get; internal set; }
        public object Parameter { get; internal set; }
        public Type SourcePageType { get; internal set; }       
    }
}