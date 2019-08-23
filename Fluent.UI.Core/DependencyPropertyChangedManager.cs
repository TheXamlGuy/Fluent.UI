using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Core
{
    public class DependencyPropertyChangedManager
    {
        private readonly IList<DependencyPropertyChangedSubscription> _subscriptions = new List<DependencyPropertyChangedSubscription>();

        public void AddEventHandler(DependencyObject source, DependencyProperty property)
        {
            _subscriptions.Add(new DependencyPropertyChangedSubscription(source, property));
        }
    }
}
