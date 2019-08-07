using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class VisualStateTrigger : VisualState
    {
        public static readonly DependencyProperty EnableStateTriggersProperty =
            DependencyProperty.RegisterAttached("EnableStateTriggers",
                typeof(bool), typeof(VisualStateTrigger),
                new PropertyMetadata(false, EnableStateTriggersChanged));

        private FrameworkElement _element;

        public VisualStateTrigger()
        {
            Name = Guid.NewGuid().ToString();

            Setters = new ObservableCollection<Setter>();
            StateTriggers = new ObservableCollection<StateTriggerBase>();
            StateTriggers.CollectionChanged += Triggers_CollectionChanged;
            Setters.CollectionChanged += Setters_CollectionChanged;
        }

        public ObservableCollection<Setter> Setters { get; }

        public ObservableCollection<StateTriggerBase> StateTriggers { get; }

        internal FrameworkElement Element
        {
            get => _element;

            set
            {
                _element = value;
                if (_element.IsLoaded)
                {
                    SetActive();
                }
                else
                {
                    _element.Loaded += Element_Loaded;
                }
            }
        }

        public static bool GetEnableStateTriggers(FrameworkElement element)
        {
            return (bool)element.GetValue(EnableStateTriggersProperty);
        }

        public static void SetEnableStateTriggers(FrameworkElement element, bool value)
        {
            element.SetValue(EnableStateTriggersProperty, value);
        }

        internal void SetActive(bool active)
        {
            if (Element == null || !active)
            {
                return;
            }

            VisualStateManager.GoToElementState(Element, Name, true);

            foreach (var setter in Setters)
            {
                var property = setter.Property;
                var value = setter.Value;         
                var targetName = setter.TargetName;

                var target = Element.FindDescendantByName(targetName) as DependencyObject;
                target?.SetCurrentValue(property, value);
            }
        }

        private static void EnableStateTriggersChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is FrameworkElement element))
            {
                throw new NotSupportedException("EnableStateTriggers can only be applied on elements of type System.Windows.FrameworkElement");
            }

            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                VisualStateManagerHook.Set(element);
            }
            else
            {
                VisualStateManagerHook.UnSet(element);
            }
        }

        private void Element_Loaded(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement)sender).Loaded -= Element_Loaded;

            SetActive();
        }

        private void SetActive()
        {
            SetActive(StateTriggers.Any(t => t.IsTriggerActive));
        }

        private void Setters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetActive();
        }

        private void Triggers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<StateTriggerBase>())
                {
                    item.Owner = this;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<StateTriggerBase>())
                {
                    if (Equals(item.Owner, this))
                    {
                        item.Owner = null;
                    }
                }
            }

            SetActive(StateTriggers.Any(t => t.IsTriggerActive));
        }
    }
}