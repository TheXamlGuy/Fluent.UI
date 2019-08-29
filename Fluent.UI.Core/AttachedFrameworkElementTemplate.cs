using Fluent.UI.Core.Extensions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : DependencyObject, IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        protected static readonly DependencyPropertyKey IsPressedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsPressed),
                typeof(bool), typeof(AttachedFrameworkElementTemplate<TFrameworkElement>),
                new PropertyMetadata(false, OnInternalIsPressedPropertyChanged));

        protected static readonly DependencyPropertyKey IsPointerOverPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsPointerOver),
                typeof(bool), typeof(AttachedFrameworkElementTemplate<TFrameworkElement>),
                new PropertyMetadata(false, OnInternalIsPointOverPropertyChanged));

        protected readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;
        protected readonly DependencyProperty IsPointerOverProperty = IsPointerOverPropertyKey.DependencyProperty;

        private DependencyPropertyChangedManager _dependencyPropertyChangedManager;

        private ElementTheme _requestedTheme;

        private WeakReference<TFrameworkElement> _weakFrameworkElement;

        protected bool IsEnabled => AttachedFrameworkElement.IsEnabled;

        protected bool IsFocused => AttachedFrameworkElement.IsFocused;

        protected bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedPropertyKey, value);
        }

        protected TFrameworkElement AttachedFrameworkElement => _weakFrameworkElement.TryGetTarget(out TFrameworkElement frameworkElement) ? frameworkElement : null;

        protected bool IsPointerOver
        {
            get => (bool)GetValue(IsPointerOverProperty);
            set => SetValue(IsPointerOverPropertyKey, value);
        }

        public void ApplyRequestedTheme()
        {
            OnApplyRequestedTheme(_requestedTheme);
        }

        public virtual void OnApplyRequestedTheme(ElementTheme requestedTheme)
        {
        }

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            _weakFrameworkElement = new WeakReference<TFrameworkElement>(frameworkElement as TFrameworkElement);
            _dependencyPropertyChangedManager = new DependencyPropertyChangedManager();

            WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(AttachedFrameworkElement, "Loaded", OnLoaded);
            WeakEventManager<FrameworkElement, RoutedEventArgs>.AddHandler(AttachedFrameworkElement, "Unloaded", OnUnloaded);

            OnAttached();
        }

        protected void AddEventHandler<THandler>(RoutedEvent routedEvent, THandler handler) where THandler : Delegate
        {
            AttachedFrameworkElement.AddHandler(routedEvent, handler, true);
        }

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

        protected virtual void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsFocusedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsPointerOverPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsPressedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnPointerLeave(object sender, RoutedEventArgs args)
        {
            SetIsPressed(false);
            SetIsPointerOver(false);
        }

        protected virtual void OnPointerEnter(object sender, RoutedEventArgs args)
        {
            SetIsPointerOver(true);
        }

        protected virtual void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {
            if (Mouse.Captured == null)
            {
                AttachedFrameworkElement.Focus();
                AttachedFrameworkElement.CaptureMouse();

                if (AttachedFrameworkElement.IsMouseCaptured)
                {
                    if (args.ButtonState == MouseButtonState.Pressed)
                    {
                        if (!IsPressed)
                        {
                            SetIsPressed(true);
                        }
                    }
                    else
                    {
                        AttachedFrameworkElement.ReleaseMouseCapture();
                    }
                }
            }
        }

        protected virtual void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {
            if (args.ButtonState == MouseButtonState.Released)
            {
                if (Mouse.Captured != null)
                {
                    AttachedFrameworkElement.ReleaseMouseCapture();
                    SetIsPressed(false);
                }
            }
        }

        protected virtual void RegisterEvents()
        {

        }

        protected void RemoveEventHandler<THandler>(RoutedEvent routedEvent, THandler handler) where THandler : Delegate
        {
            AttachedFrameworkElement.RemoveHandler(routedEvent, handler);
        }

        protected virtual void UnregisterEvents()
        {

        }

        private static void OnInternalIsPointOverPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as AttachedFrameworkElementTemplate<TFrameworkElement>;
            sender?.OnIsPointerOverPropertyChanged(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        private static void OnInternalIsPressedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as AttachedFrameworkElementTemplate<TFrameworkElement>;
            sender?.OnIsPressedPropertyChanged(dependencyObject, dependencyPropertyChangedEventArgs);
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            RegisterRequiredEvents();
            RegisterEvents();

            OnApplyTemplate();
            ChangeVisualState(false);
        }

        private void OnRequestedTheme(RequestedThemeEventArgs args)
        {
            if (args.Source.TryGetTarget(out var source))
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
            UnregisterEvents();
            UnregisterRequiredEvents();
        }

        private void RegisterRequiredEvents()
        {
            //    EventAggregator.Current.Subscribe<RequestedThemeEventArgs>(OnRequestedTheme);

            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnIsEnabledPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsFocusedProperty, OnIsFocusedPropertyChanged);

            AddEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonDownEvent, OnPointerPressed);
            AddEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonUpEvent, OnPointerReleased);
            AddEventHandler<RoutedEventHandler>(UIElement.MouseLeaveEvent, OnPointerLeave);
            AddEventHandler<RoutedEventHandler>(UIElement.MouseEnterEvent, OnPointerEnter);

        }

        private void SetIsPressed(bool pressed)
        {
            if (pressed)
            {
                SetValue(IsPressedPropertyKey, pressed);
            }
            else
            {
                ClearValue(IsPressedPropertyKey);
            }
        }

        private void SetIsPointerOver(bool pointerOver)
        {
            if (pointerOver)
            {
                SetValue(IsPointerOverPropertyKey, pointerOver);
            }
            else
            {
                ClearValue(IsPointerOverPropertyKey);
            }
        }

        private void UnregisterRequiredEvents()
        {
            //  EventAggregator.Current.<RequestedThemeEventArgs>(OnRequestedTheme);

            RemoveEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonDownEvent, OnPointerPressed);
            RemoveEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonUpEvent, OnPointerReleased);
            RemoveEventHandler<RoutedEventHandler>(UIElement.MouseLeaveEvent, OnPointerLeave);
        }

        private void UpdateIsPressed()
        {
            var pos = Mouse.PrimaryDevice.GetPosition(AttachedFrameworkElement);
            if (pos.X >= 0 && pos.X <= AttachedFrameworkElement.ActualWidth && pos.Y >= 0 && pos.Y <= AttachedFrameworkElement.ActualHeight)
            {
                if (!IsPressed)
                {
                    SetIsPressed(true);
                }
            }
            else if (IsPressed)
            {
                SetIsPressed(false);
            }
        }
    }
}