using System;
using System.Windows;

namespace Fluent.UI.Controls
{
    public sealed class PropertyChangeObserver : DependencyObject, IDisposable
    {
        private readonly WeakReference _propertyReference;
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
