using Fluent.UI.Core.Extensions;
using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : DependencyObject, IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        private DependencyPropertyChangedManager _dependencyPropertyChangedManager;
        private ElementTheme _requestedTheme;
        private WeakReference<TFrameworkElement> _weakFrameworkElement;

        private bool IsSpaceKeyDown;

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
        protected bool IsPointerOver => AttachedFrameworkElement.IsMouseOver;

        public void ApplyRequestedTheme()
        {
            OnApplyRequestedTheme(_requestedTheme);
        }

        public virtual void OnApplyRequestedTheme(ElementTheme requestedTheme)
        {
        }

        public virtual void OnIsPressedPropertyChanged(DependencyObject dependencyObject) => ChangeVisualState(true);

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            _weakFrameworkElement = new WeakReference<TFrameworkElement>(frameworkElement as TFrameworkElement);
            _dependencyPropertyChangedManager = new DependencyPropertyChangedManager();

            RegisterEvents();
            OnAttached();
        }

        protected void AddEventHandler<TEventArgs>(string eventName, EventHandler<TEventArgs> handler) where TEventArgs : EventArgs => WeakEventManager<TFrameworkElement, TEventArgs>.AddHandler(AttachedFrameworkElement, eventName, handler);

        protected void AddPropertyChangedHandler(DependencyProperty property, PropertyChangedCallback propertyChangedCallback) => _dependencyPropertyChangedManager.AddEventHandler(AttachedFrameworkElement, property, propertyChangedCallback);

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

        protected virtual void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected virtual void OnIsFocusedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected virtual void OnIsPointerOverPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected virtual void OnPointerLeave(object sender, RoutedEventArgs args)
        {

        }

        protected virtual void OnPointerMove(object sender, RoutedEventArgs args)
        {

        }

        protected virtual void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {

        }

        protected virtual void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {

        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            OnApplyTemplate();
            ChangeVisualState(false);
        }

        private void OnRequestedTheme(RequestedThemeEventArgs args)
        {
            if (args.Source.TryGetTarget(out FrameworkElement source))
            {
                if (AttachedFrameworkElement.IsChildOf(source))
                {
                    _requestedTheme = args.RequestedTheme;
                    ApplyRequestedTheme();
                }
           }
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            OnDetached();
        }

        private void RegisterEvents()
        {
            EventAggregator.Current.Subscribe<RequestedThemeEventArgs>(OnRequestedTheme);

            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnIsEnabledPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnIsPointerOverPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsFocusedProperty, OnIsFocusedPropertyChanged);

            AddEventHandler<RoutedEventArgs>("Loaded", OnLoaded);
            AddEventHandler<RoutedEventArgs>("Unloaded", OnUnloaded);
            AddEventHandler<MouseButtonEventArgs>("MouseLeftButtonDown", OnPointerPressed);
            AddEventHandler<MouseButtonEventArgs>("MouseLeftButtonUp", OnPointerReleased);
            AddEventHandler<RoutedEventArgs>("MouseLeave", OnPointerLeave);
            AddEventHandler<RoutedEventArgs>("MouseMove", OnPointerMove);
        }
    }
}