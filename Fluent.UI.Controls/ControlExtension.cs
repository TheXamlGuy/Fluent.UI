using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public abstract class ControlExtension<TControl, TControlExtension> : DependencyObject where TControl : Control where TControlExtension : ControlExtension<TControl, TControlExtension>, new()
    {
        public static readonly DependencyProperty IsAttachedProperty =
           DependencyProperty.RegisterAttached("IsAttached",
               typeof(bool), typeof(ControlExtension<TControl, TControlExtension>),
               new PropertyMetadata(false, OnIsExtendedPropertyChanged));

        internal static DependencyProperty AttachedControlProperty =
          DependencyProperty.RegisterAttached("AttachedControl",
              typeof(ControlExtension<TControl, TControlExtension>), typeof(ControlExtension<TControl, TControlExtension>));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(ControlExtension<TControl, TControlExtension>),
                new PropertyMetadata(ElementTheme.Dark, OnRequestedThemePropertyChanged));

        protected TControl AttachedControl;

        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        public static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        public static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        protected virtual void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {

        }

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedControl, stateName, useTransitions);

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            PrepareRequestedTheme();
        }

        private void PrepareRequestedTheme()
        {

        }

        private static void ClearAttachedControl(TControl control)
        {
            var extensionBase = control.GetValue(AttachedControlProperty) as ControlExtension<TControl, TControlExtension>;
            extensionBase?.RemoveAttachedControl();

            control.ClearValue(AttachedControlProperty);
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                SetRequestedTheme((TControl)dependencyObject, (ElementTheme)args.NewValue);
            }
        }

        public static void SetRequestedTheme(TControl control, ElementTheme value)
        {
            var attachedControl = GetAttachedControl(control);
            if (attachedControl != null)
            {

            }

            control.SetValue(RequestedThemeProperty, value);
        }

        private static void OnIsExtendedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue != (bool)args.OldValue)
            {
                if ((bool)args.NewValue)
                {
                    SetAttachedControl(dependencyObject as TControl, new TControlExtension());
                }
                else
                {
                    ClearAttachedControl(dependencyObject as TControl);
                }
            }
        }

        internal static ControlExtension<TControl, TControlExtension> GetAttachedControl(TControl control) => (ControlExtension<TControl, TControlExtension>)control.GetValue(AttachedControlProperty);

        private static void SetAttachedControl(TControl control, ControlExtension<TControl, TControlExtension> extensionBase)
        {
            control.SetValue(AttachedControlProperty, extensionBase);
            extensionBase.SetAttachedControl(control);
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            AttachedControl.Unloaded += OnUnloaded;
            AttachedControl.Loaded += OnLoaded;
        }

        private void RemoveAttachedControl()
        {
            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            AttachedControl = null;
        }

        private void SetAttachedControl(TControl control)
        {
            AttachedControl = control;
            
            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        private void UnregisterEvents()
        {
            AttachedControl.Unloaded -= OnUnloaded;
            AttachedControl.Loaded -= OnLoaded;
        }
    }
}
