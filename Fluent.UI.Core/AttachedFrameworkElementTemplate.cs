using Fluent.UI.Core.Extensions;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Windows;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        private bool _isRequestedTheme;
        private bool _isRequestedThemePropagated;
        private ElementTheme _requestedTheme;
        private ElementTheme _requestedThemePropagated;
        private WeakReference<TFrameworkElement> _weakFrameworkElement;
        private WeakEventListener<AttachedFrameworkElementTemplate<TFrameworkElement>, object, RoutedEventArgs> _weakLoadedEventListener;
        private WeakEventListener<AttachedFrameworkElementTemplate<TFrameworkElement>, object, RoutedEventArgs> _weakUnloadedEventListener;
        protected bool IsEnabled => AttachedFrameworkElement.IsEnabled;
        protected bool IsFocused => AttachedFrameworkElement.IsFocused;
        protected bool IsMouseOver => AttachedFrameworkElement.IsMouseOver;

        protected TFrameworkElement AttachedFrameworkElement
        {
            get
            {
                if (_weakFrameworkElement.TryGetTarget(out TFrameworkElement target))
                {
                    return target;
                }

                return null;
            }
        }

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            _weakFrameworkElement = new WeakReference<TFrameworkElement>(frameworkElement as TFrameworkElement);

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

        protected virtual void OnApplyTemplate()
        {

        }

        protected virtual void OnAttached()
        {

        }

        protected virtual void OnDetached()
        {

        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            OnApplyTemplate();
            ChangeVisualState(false);
        }

        protected virtual void PrepareRequestedTheme(ElementTheme requestedTheme)
        {

        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            OnDetached();
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
            WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(AttachedFrameworkElement, "Loaded", OnUnloaded);
            //_weakLoadedEventListener = new WeakEventListener<AttachedFrameworkElementTemplate<TFrameworkElement>, object, RoutedEventArgs>(this)
            //{.
            //    OnEventAction = (instance, source, eventArgs) => instance.OnLoaded(source, eventArgs),
            //    OnDetachAction = (weakEventListener) => AttachedFrameworkElement.Loaded -= weakEventListener.OnEvent
            //};

            //AttachedFrameworkElement.Loaded += _weakLoadedEventListener.OnEvent;

            //_weakUnloadedEventListener = new WeakEventListener<AttachedFrameworkElementTemplate<TFrameworkElement>, object, RoutedEventArgs>(this)
            //{
            //    OnEventAction = (instance, source, eventArgs) => instance.OnUnloaded(source, eventArgs),
            //    OnDetachAction = (weakEventListener) => AttachedFrameworkElement.Unloaded -= weakEventListener.OnEvent
            //};

            //AttachedFrameworkElement.Unloaded += _weakUnloadedEventListener.OnEvent;
        }

        private void RemoveAttachedControl()
        {
            UnregisterEvents();
            OnDetached();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            _weakFrameworkElement = null;
        }

        private void UnregisterEvents()
        {
            if ( _weakLoadedEventListener != null)
            {
                _weakLoadedEventListener.Detach();
                _weakLoadedEventListener = null;
            }

            if (_weakUnloadedEventListener != null)
            {
                _weakUnloadedEventListener.Detach();
                _weakUnloadedEventListener = null;
            }
        }
    }
}
