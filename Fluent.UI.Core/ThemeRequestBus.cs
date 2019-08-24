using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Fluent.UI.Core
{
    public class RequestedThemeEventArgs : EventArgs
    {
        public RequestedThemeEventArgs(ElementTheme requestedTheme)
        {
            RequestedTheme = requestedTheme;
        }

        public ElementTheme RequestedTheme { get; }
    }

    public class RequestedThemeMessageBus
    {
        private static readonly Lazy<RequestedThemeMessageBus> _lazyBus = new Lazy<RequestedThemeMessageBus>(() => new RequestedThemeMessageBus());
        private ConditionalWeakTable<FrameworkElement, EventHandler<RequestedThemeEventArgs>> _subscriptions = new ConditionalWeakTable<FrameworkElement, EventHandler<RequestedThemeEventArgs>>();

        public void Subscribe(FrameworkElement key, EventHandler<RequestedThemeEventArgs> handler)
        {
            _subscriptions.AddOrUpdate(key, handler);
        }

        public void Publish(FrameworkElement sender, RequestedThemeEventArgs args)
        {
            foreach (var i in _subscriptions)
            {
                i.Value.Invoke(sender, args);
            }
        }

        public static RequestedThemeMessageBus Current => _lazyBus.Value;
    }
}