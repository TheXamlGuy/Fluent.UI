using Fluent.UI.Core.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;
        private DependencyPropertyChangedManager _dependencyPropertyChangedManager = new DependencyPropertyChangedManager();

        private bool _isRequestedTheme;
        private bool _isRequestedThemePropagated;
        private ElementTheme _requestedTheme;
        private ElementTheme _requestedThemePropagated;

        protected bool IsEnabled => AttachedFrameworkElement.IsEnabled;
        protected bool IsFocused => AttachedFrameworkElement.IsFocused;
        protected bool IsMouseOver => AttachedFrameworkElement.IsMouseOver;
        protected TFrameworkElement AttachedFrameworkElement { get; private set; }
        protected bool IsLoaded { get; private set; }

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            AttachedFrameworkElement = frameworkElement as TFrameworkElement;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);

            OnAttached();
        }

        public void SetRequestedTheme(ElementTheme requestedTheme)
        {
            _isRequestedThemePropagated = false;
            _isRequestedTheme = true;
            _requestedTheme = requestedTheme;

            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            PrepareRequestedTheme();
        }

        public void SetRequestedThemePropagated(ElementTheme requestedTheme)
        {
            _isRequestedThemePropagated = true;
            _requestedThemePropagated = requestedTheme;

            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            PrepareRequestedTheme();
        }

        internal void PropagateRequestedTheme(ElementTheme requestedTheme)
        {
            PrepareRequestedTheme(requestedTheme);
        }

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        protected virtual void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {

        }

        protected TTemplateChild GetTemplateChild<TTemplateChild>(string name) where TTemplateChild : FrameworkElement => AttachedFrameworkElement.FindDescendantByName(name) as TTemplateChild;

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, stateName, useTransitions);

        protected virtual void OnAttached()
        {

        }

        protected virtual void OnDetached()
        {

        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            if (IsLoaded)
            {
                return;
            }

            OnApplyTemplate();
            ChangeVisualState(false);
            IsLoaded = true;
        }

        protected virtual void OnApplyTemplate()
        {

        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
            OnDetached();
        }

        protected virtual void PrepareRequestedTheme(ElementTheme requestedTheme)
        {

        }

        private void PrepareRequestedTheme()
        {
            ElementTheme requestedTheme;
            if (_isRequestedThemePropagated)
            {
                requestedTheme = _requestedThemePropagated;
            }
            else
            {
                if (!_isRequestedTheme)
                {
                    return;
                }

                var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
                if (!_isRequestedTheme && ((int)_requestedTheme == (int)requestedApplicationTheme))
                {
                    return;
                }
                requestedTheme = _requestedTheme;
            }

            PrepareRequestedTheme(requestedTheme);
        }

        private void RegisterEvents()
        {
            AddEvent<RoutedEventArgs>("Loaded", OnLoaded);
            AddEvent<RoutedEventArgs>("Unloaded", OnLoaded);
        }

        protected void AddEvent<TEventArgs>(string eventName, EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
        {
            WeakEventManager<TFrameworkElement, TEventArgs>.AddHandler(AttachedFrameworkElement, eventName, handler);
        }

        protected void AddValueChanged(DependencyProperty property, EventHandler handler)
        {
            _dependencyPropertyChangedManager.AddEventHandler(AttachedFrameworkElement, property);
        }

        private void RemoveAttachedControl()
        {
            UnregisterEvents();
            OnDetached();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            AttachedFrameworkElement = null;
        }


        private void UnregisterEvents()
        {
 
        }
    }
}
