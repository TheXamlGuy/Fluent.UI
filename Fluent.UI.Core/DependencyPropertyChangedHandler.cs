using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Fluent.UI.Core
{
    public static class Observe
    {
        public static PropertyChangeNotifier PropertyChanged(this DependencyObject source, DependencyProperty property)
        {
            return new PropertyChangeNotifier(source, property);
        }
    }

   

    public class DependencyPropertyChangedHandler
    {
        private readonly IDictionary<Tuple<DependencyObject, DependencyProperty, Action>, EventHandler> _subscriptions = new Dictionary<Tuple<DependencyObject, DependencyProperty, Action>, EventHandler>();

        public void Add(DependencyObject propertySource, DependencyProperty property, Action actionDelegate)
        {
            void handler(object sender, EventArgs args) => actionDelegate.Invoke();
            TypeDescriptor.GetProperties(propertySource)[property.Name].AddValueChanged(propertySource, handler);

            _subscriptions.Add(new Tuple<DependencyObject, DependencyProperty, Action>(propertySource, property, actionDelegate), handler);
        }

        public void Clear()
        {
            foreach (var subscription in _subscriptions.ToList())
            {
                var key = subscription.Key;
                var handler = subscription.Value;

                TypeDescriptor.GetProperties(key.Item1)[key.Item2.Name].RemoveValueChanged(key.Item1, handler);
                _subscriptions.Remove(key);
            }
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
    }

    public sealed class PropertyChangeNotifier : DependencyObject, IDisposable
    {
        private readonly WeakReference propertySource;

        public PropertyChangeNotifier(DependencyObject propertySource, string path)
            : this(propertySource, new PropertyPath(path))
        {
        }

        public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
            : this(propertySource, new PropertyPath(property))
        {
        }

        public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
        {
            if (null == propertySource)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }

            if (null == property)
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.propertySource = new WeakReference(propertySource);
            var binding = new Binding { Path = property, Mode = BindingMode.OneWay, Source = propertySource };
            BindingOperations.SetBinding(this, ValueProperty, binding);
        }

        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    // note, it is possible that accessing the target property
                    // will result in an exception so i’ve wrapped this check
                    // in a try catch
                    return this.propertySource.IsAlive ? this.propertySource.Target as DependencyObject : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register(nameof(Value),
                                          typeof(object),
                                          typeof(PropertyChangeNotifier),
                                          new FrameworkPropertyMetadata(null, OnValueChanged));

        /// <summary>
        /// Gets or sets the value of the watched property.
        /// </summary>
        /// <seealso cref="ValueProperty"/>
        [Description("Gets or sets the value of the watched property.")]
        [Category("Behavior")]
        [Bindable(true)]
        public object Value
        {
            get { return (object)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var notifier = (PropertyChangeNotifier)d;
            if (notifier.RaiseValueChanged)
            {
                notifier.ValueChanged?.Invoke(notifier.PropertySource, EventArgs.Empty);
                Debug.WriteLine("sfdsaf");
            }
        }

        public event EventHandler ValueChanged;

        public bool RaiseValueChanged { get; set; } = true;

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ValueProperty);
        }
    }
}
