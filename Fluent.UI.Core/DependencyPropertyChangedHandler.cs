using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Core
{
    public class DependencyPropertyChangedHandler
    {
        private readonly IDictionary<Tuple<DependencyObject, DependencyProperty, Action>, EventHandler> _subscriptions = new Dictionary<Tuple<DependencyObject, DependencyProperty, Action>, EventHandler>();

        public void Add(DependencyObject propertySource, DependencyProperty property, Action actionDelegate)
        {
            void handler(object sender, EventArgs args) => actionDelegate.Invoke();
            TypeDescriptor.GetProperties(propertySource)[property.Name].AddValueChanged(propertySource, handler);

            _subscriptions.Add(new Tuple<DependencyObject, DependencyProperty, Action>(propertySource, property, actionDelegate), handler);
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
