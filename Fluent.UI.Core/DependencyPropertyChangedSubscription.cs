using System;
using System.Windows;
using System.Windows.Data;

namespace Fluent.UI.Core
{
    public class DependencyPropertyChangedSubscription : DependencyObject, IDisposable
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object),
                typeof(DependencyPropertyChangedSubscription),
                new PropertyMetadata(null, OnValueChanged));

        private readonly WeakReference _weakPropertySource;

        public DependencyPropertyChangedSubscription(DependencyObject propertySource, string path, PropertyChangedCallback propertyChangedCallback) : this(propertySource, new PropertyPath(path), propertyChangedCallback)
        {

        }

        public DependencyPropertyChangedSubscription(DependencyObject propertySource, DependencyProperty property, PropertyChangedCallback propertyChangedCallback) : this(propertySource, new PropertyPath(property), propertyChangedCallback)
        {
        }

        public DependencyPropertyChangedSubscription(DependencyObject propertySource, PropertyPath property, PropertyChangedCallback propertyChangedCallback)
        {
            if (propertySource == null)
            {
                throw new ArgumentNullException(nameof(propertySource));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            _weakPropertySource = new WeakReference(propertySource);
            var binding = new Binding
            {
                Path = property,
                Mode = BindingMode.OneWay,
                Source = propertySource
            };

            BindingOperations.SetBinding(this, ValueProperty, binding);

            _propertyChangedCallback = propertyChangedCallback;
        }

        private PropertyChangedCallback _propertyChangedCallback;

        public DependencyObject PropertySource
        {
            get
            {
                try
                {
                    return _weakPropertySource.IsAlive ? _weakPropertySource.Target as DependencyObject : null;
                }
                catch
                {
                    return null;
                }
            }
        }

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ValueProperty);
        }

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var sender = dependencyObject as DependencyPropertyChangedSubscription;
            sender?._propertyChangedCallback?.Invoke(dependencyObject, args);
        }
    }
}