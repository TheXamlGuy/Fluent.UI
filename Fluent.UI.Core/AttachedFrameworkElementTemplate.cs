using Fluent.UI.Core.Extensions;
using System;
using System.Windows;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        private DependencyPropertyChangedManager _dependencyPropertyChangedManager;
        private WeakReference<TFrameworkElement> _weakFrameworkElement;

        public TFrameworkElement AttachedFrameworkElement
        {
            get
            {
                if (_weakFrameworkElement.TryGetTarget(out TFrameworkElement frameworkElement))
                {
                    return frameworkElement;
                }

                return null;
            }
        }

        protected bool IsEnabled => AttachedFrameworkElement.IsEnabled;
        protected bool IsFocused => AttachedFrameworkElement.IsFocused;
        protected bool IsMouseOver => AttachedFrameworkElement.IsMouseOver;

        public void ApplyRequestedTheme(ElementTheme requestedTheme)
        {
            OnApplyRequestedTheme(requestedTheme);
        }

        public virtual void OnApplyRequestedTheme(ElementTheme requestedTheme)
        {
        }

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            _weakFrameworkElement = new WeakReference<TFrameworkElement>(frameworkElement as TFrameworkElement);
            _dependencyPropertyChangedManager = new DependencyPropertyChangedManager();

            RegisterEvents();
            OnAttached();
        }

        private void OnRequestedTheme(object sender, RequestedThemeEventArgs args)
        {
            if (AttachedFrameworkElement.IsChildOf(sender as FrameworkElement))
            {
                ApplyRequestedTheme(args.RequestedTheme);
            }
        }

        protected void AddEventHandler<TEventArgs>(string eventName, EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
        {
            WeakEventManager<TFrameworkElement, TEventArgs>.AddHandler(AttachedFrameworkElement, eventName, handler);
        }

        protected void AddPropertyChangedHandler(DependencyProperty property, PropertyChangedCallback propertyChangedCallback)
        {
            _dependencyPropertyChangedManager.AddEventHandler(AttachedFrameworkElement, property, propertyChangedCallback);
        }

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {
        }

        protected TTemplateChild GetTemplateChild<TTemplateChild>(string name) where TTemplateChild : FrameworkElement => AttachedFrameworkElement.FindDescendantByName(name) as TTemplateChild;

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, stateName, useTransitions);

        protected virtual void OnApplyTemplate()
        {
        }

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetached()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            OnApplyTemplate();
            ChangeVisualState(false);
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            OnDetached();
        }

        private void RegisterEvents()
        {
            RequestedThemeMessageBus.Current.Subscribe(AttachedFrameworkElement, OnRequestedTheme);

            AddEventHandler<RoutedEventArgs>("Loaded", OnLoaded);
            AddEventHandler<RoutedEventArgs>("Unloaded", OnLoaded);
        }

        private void RemoveAttachedControl()
        {
            OnDetached();

            _weakFrameworkElement = null;
        }
    }
}