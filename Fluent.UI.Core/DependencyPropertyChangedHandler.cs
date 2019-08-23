using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Core
{
    public struct Subscription
    {
        public Subscription(WeakReference subscriber, DependencyProperty handler, Action action)
        {
            Subscriber = subscriber;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            Action = action
        }

        public readonly WeakReference Subscriber;
        public readonly DependencyProperty Handler;
        public readonly Action Action;
    }

    public class DependencyPropertyChangedHandler
    {

        readonly Dictionary<DependencyObject, List<Subscription>> _eventHandlers = new Dictionary<DependencyObject, List<Subscription>>();

        public void Add(DependencyObject propertySource, DependencyProperty property, Action actionDelegate)
        {
            void handler(object sender, EventArgs args) => actionDelegate.Invoke();
            TypeDescriptor.GetProperties(propertySource)[property.Name].AddValueChanged(propertySource, handler);

            if (!_eventHandlers.TryGetValue(propertySource, out List<Subscription> targets))
            {
                targets = new List<Subscription>();
                _eventHandlers.Add(propertySource, targets);
            }

            targets.Add(new Subscription(new WeakReference(propertySource), property));
        }

        void AddEventHandler(string eventName, object handlerTarget, MethodInfo methodInfo)
        {
            if (!_eventHandlers.TryGetValue(eventName, out List<Subscription> targets))
            {
                targets = new List<Subscription>();
                _eventHandlers.Add(eventName, targets);
            }

            if (handlerTarget == null)
            {
                // This event handler is a static method
                targets.Add(new Subscription(null, methodInfo));
                return;
            }

            targets.Add(new Subscription(new WeakReference(handlerTarget), methodInfo));
        }

        public void Remove(DependencyObject propertySource, DependencyProperty property, Action actionDelegate)
        {
            var key = new Tuple<DependencyObject, DependencyProperty, Action>(propertySource, property, actionDelegate);
            if (_subscriptions.ContainsKey(key))
            {
                var handler = _subscriptions[key];
                TypeDescriptor.GetProperties(propertySource)[property.Name].RemoveValueChanged(propertySource, handler);

                _subscriptions.Remove(key);
            }
        }

        public void Clear()
        {
            foreach(var subscription in _subscriptions.ToList())
            {
                var key = subscription.Key;
                var handler = subscription.Value;

                TypeDescriptor.GetProperties(key.Item1)[key.Item2.Name].RemoveValueChanged(key.Item1, handler);
                _subscriptions.Remove(key);
            }
        }
    }
}
