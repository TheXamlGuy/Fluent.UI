using System.Collections.Generic;
using System.Windows;

namespace Fluent.UI.Core
{
    public class DependencyPropertyChangedManager
    {
        private readonly IList<DependencyPropertyChangedSubscription> _subscriptions = new List<DependencyPropertyChangedSubscription>();

        public void AddEventHandler(DependencyObject source, DependencyProperty property, PropertyChangedCallback propertyChangedCallback)
        {
            _subscriptions.Add(new DependencyPropertyChangedSubscription(source, property, propertyChangedCallback));
        }
    }
}