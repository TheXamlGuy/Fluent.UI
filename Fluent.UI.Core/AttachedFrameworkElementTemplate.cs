using Fluent.UI.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Input;

namespace Fluent.UI.Core
{
    public class AttachedFrameworkElementTemplate<TFrameworkElement> : DependencyObject, IAttachedFrameworkElementTemplate<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        protected static readonly DependencyPropertyKey IsPointerOverPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsPointerOver),
                typeof(bool), typeof(AttachedFrameworkElementTemplate<TFrameworkElement>),
                new PropertyMetadata(false, OnInternalIsPointOverPropertyChanged));

        protected static readonly DependencyPropertyKey IsPressedPropertyKey = 
            DependencyProperty.RegisterReadOnly(nameof(IsPressed),
                typeof(bool), typeof(AttachedFrameworkElementTemplate<TFrameworkElement>),
                new PropertyMetadata(false, OnInternalIsPressedPropertyChanged));

        protected readonly DependencyProperty IsPointerOverProperty = IsPointerOverPropertyKey.DependencyProperty;
        protected readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        private DependencyPropertyChangedManager _dependencyPropertyChangedManager;

        private ElementTheme _requestedTheme;

        private WeakReference<TFrameworkElement> _weakFrameworkElement;

        protected bool IsEnabled => AttachedFrameworkElement.IsEnabled;

        protected bool IsFocused => AttachedFrameworkElement.IsFocused;

        protected TFrameworkElement AttachedFrameworkElement => _weakFrameworkElement.TryGetTarget(out TFrameworkElement frameworkElement) ? frameworkElement : null;

        protected bool IsPointerOver
        {
            get => (bool)GetValue(IsPointerOverProperty);
            set => SetValue(IsPointerOverPropertyKey, value);
        }

        protected bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedPropertyKey, value);
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

        protected virtual void OnClick()
        {

        }

        protected virtual void OnDetached()
        {
        }

        protected virtual void OnIsEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsFocusedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsPointerOverPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnIsPressedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        protected virtual void OnPointerLeave(object sender, MouseEventArgs args)
        {
            SetIsPressed(false);
            SetIsPointerOver(false);
        }

        protected virtual void OnPointerLostCapture(object sender, MouseEventArgs args)
        {
            if (args.OriginalSource.Equals(AttachedFrameworkElement))
            {
                SetIsPressed(false);
            }
        }

        protected virtual void OnPointerMove(object sender, MouseEventArgs args)
        {
            if (AttachedFrameworkElement.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                UpdateIsPressed();
            }
        }

        protected virtual void OnPointerOver(object sender, MouseEventArgs args)
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
            if (Mouse.Captured != null && AttachedFrameworkElement.IsMouseCaptured)
            {
                if (IsPressed && args.ButtonState == MouseButtonState.Released)
                {
                    OnClick();
                }

                AttachedFrameworkElement.ReleaseMouseCapture();
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

        private bool IsPointerInPosition()
        {
            var pos = Mouse.PrimaryDevice.GetPosition(AttachedFrameworkElement);
            if (pos.X >= 0 && pos.X <= AttachedFrameworkElement.ActualWidth && pos.Y >= 0 && pos.Y <= AttachedFrameworkElement.ActualHeight)
            {
                return true;
            }

            return false;
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
            AddEventHandler<MouseEventHandler>(UIElement.MouseMoveEvent, OnPointerMove);
            AddEventHandler<MouseEventHandler>(UIElement.MouseLeaveEvent, OnPointerLeave);
            AddEventHandler<MouseEventHandler>(UIElement.MouseEnterEvent, OnPointerOver);
            AddEventHandler<MouseEventHandler>(UIElement.LostMouseCaptureEvent, OnPointerLostCapture);
        }

        private void SetIsPointerOver(bool pointerOver)
        {
            if (pointerOver)
            {
                SetValue(IsPointerOverPropertyKey, true);
            }
            else
            {
                ClearValue(IsPointerOverPropertyKey);
            }
        }

        private void SetIsPressed(bool pressed)
        {
            if (pressed)
            {
                SetValue(IsPressedPropertyKey, true);
            }
            else
            {
                ClearValue(IsPressedPropertyKey);
            }
        }

        private void UnregisterRequiredEvents()
        {
            //  EventAggregator.Current.<RequestedThemeEventArgs>(OnRequestedTheme);

            RemoveEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonDownEvent, OnPointerPressed);
            RemoveEventHandler<MouseButtonEventHandler>(UIElement.MouseLeftButtonUpEvent, OnPointerReleased);
            RemoveEventHandler<MouseEventHandler>(UIElement.MouseMoveEvent, OnPointerMove);
            RemoveEventHandler<MouseEventHandler>(UIElement.MouseLeaveEvent, OnPointerLeave);
            RemoveEventHandler<MouseEventHandler>(UIElement.MouseEnterEvent, OnPointerOver);
            AddEventHandler<MouseEventHandler>(UIElement.LostMouseCaptureEvent, OnPointerLostCapture);
        }

        private void UpdateIsPressed()
        {
            if (IsPointerInPosition())          
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