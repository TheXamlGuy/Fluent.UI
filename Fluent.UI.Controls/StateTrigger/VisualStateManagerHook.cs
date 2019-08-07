using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Controls
{
    internal class VisualStateManagerHook : IDisposable
    {
        private static readonly DependencyProperty VisualStateManagerHookProperty =
            DependencyProperty.RegisterAttached("VisualStateManagerHookInternal",
                typeof(VisualStateManagerHook), typeof(VisualStateManagerHook));

        private VisualStateManagerHook(FrameworkElement element)
        {
            Element = element;
            VisualStateGroups = (ObservableCollection<VisualStateGroup>)element.GetValue(VisualStateManager.VisualStateGroupsProperty);
            VisualStateGroups.CollectionChanged += CollectionChanged;
            ApplyElement(VisualStateGroups);
        }

        private FrameworkElement Element { get; set; }

        private ObservableCollection<VisualStateGroup> VisualStateGroups { get; set; }

        public void Dispose()
        {
            RemoveElement(VisualStateGroups);

            VisualStateGroups.CollectionChanged -= CollectionChanged;

            Element = null;
            VisualStateGroups = null;
        }

        internal static void Set(FrameworkElement element)
        {
            var hook = new VisualStateManagerHook(element);
            element.SetValue(VisualStateManagerHookProperty, hook);
        }

        internal static void UnSet(FrameworkElement element)
        {
            if (element.GetValue(VisualStateManagerHookProperty) is VisualStateManagerHook hook)
            {
                element.SetValue(VisualStateManagerHookProperty, null);
                hook.Dispose();
            }
        }

        private void ApplyElement(IEnumerable<VisualStateGroup> visualStateGroups)
        {
            foreach (var group in visualStateGroups)
            {
                foreach (var state in group.States.OfType<VisualStateTrigger>())
                {
                    state.Element = Element;
                }
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                ApplyElement(e.NewItems.OfType<VisualStateGroup>());
            }

            if (e.OldItems != null)
            {
                RemoveElement(e.OldItems.OfType<VisualStateGroup>());
            }
        }

        private void RemoveElement(IEnumerable<VisualStateGroup> visualStateGroups)
        {
            foreach (var group in visualStateGroups)
            {
                foreach (var state in group.States.OfType<VisualStateTrigger>())
                {
                    state.Element = null;
                }
            }
        }
    }
}