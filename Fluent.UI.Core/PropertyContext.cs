using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Fluent.UI.Core
{
    public class DependencyPropertyChangedHandler
    {
        private readonly IDictionary<Tuple<DependencyObject, DependencyProperty, EventHandler>, DependencyPropertyDescriptor> _subscriptions = new Dictionary<Tuple<DependencyObject, DependencyProperty, EventHandler>, DependencyPropertyDescriptor>();

        public void Add<TType>(DependencyObject propertySource, DependencyProperty property, EventHandler handler)
        {
            var dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(property, typeof(TType));
            if (dependencyPropertyDescriptor != null)
            {
                dependencyPropertyDescriptor.AddValueChanged(propertySource, handler);
                _subscriptions.Add(new Tuple<DependencyObject, DependencyProperty, EventHandler>(propertySource, property, handler), dependencyPropertyDescriptor);
            }
        }

        public void Remove(DependencyObject propertySource, DependencyProperty property, EventHandler handler)
        {
            var key = new Tuple<DependencyObject, DependencyProperty, EventHandler>(propertySource, property, handler);
            if (_subscriptions.ContainsKey(key))
            {
                _subscriptions[key].RemoveValueChanged(propertySource, handler);
                _subscriptions.Remove(key);
            }
        }

        public void Clear()
        {
            foreach(var subscription in _subscriptions)
            {
                var key = subscription.Key;

                subscription.Value.RemoveValueChanged(key.Item1, key.Item3);
                _subscriptions.Remove(key);
            }
        }
    }
}
