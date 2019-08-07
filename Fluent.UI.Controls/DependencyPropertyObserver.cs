using System;
using System.Windows;
using System.Windows.Data;

namespace Fluent.UI.Controls
{
    public class DependencyPropertyObserver : DependencyObject, IDisposable
    {
        private readonly WeakReference _propertyReference;

        private DependencyPropertyObserver(DependencyObject propertySource, PropertyPath propertyPath)
        {
            _propertyReference = new WeakReference(propertySource);

            var propertyBinding = new Binding
            {
                Path = propertyPath,
                Mode = BindingMode.OneWay,
                Source = propertySource
            };

            BindingOperations.SetBinding(this, ValueProperty, propertyBinding);
        }

        public static DependencyPropertyObserver Register(DependencyObject dependencyObject, PropertyPath propertyPath)
        {
            return new DependencyPropertyObserver(dependencyObject, propertyPath);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
