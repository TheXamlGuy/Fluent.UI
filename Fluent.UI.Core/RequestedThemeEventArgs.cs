using System;
using System.Windows;

namespace Fluent.UI.Core
{
    public class RequestedThemeEventArgs : IEvent
    {
        public RequestedThemeEventArgs(FrameworkElement source, ElementTheme requestedTheme)
        {
            Source = new WeakReference<FrameworkElement>(source);
            RequestedTheme = requestedTheme;
        }

        public WeakReference<FrameworkElement> Source { get; set; }

        public ElementTheme RequestedTheme { get; }
    }
}