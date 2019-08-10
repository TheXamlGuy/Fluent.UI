using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Core
{
    public class ControlExtension<TControl, TControlExtension> : DependencyObject where TControl : Control where TControlExtension : ControlExtension<TControl, TControlExtension>, new()
    {
        public static readonly DependencyProperty IsAttachedProperty =
           DependencyProperty.RegisterAttached("IsAttached",
               typeof(bool), typeof(ControlExtension<TControl, TControlExtension>),
               new PropertyMetadata(false, OnIsExtendedPropertyChanged));

        internal static DependencyProperty AttachedControlProperty =
          DependencyProperty.RegisterAttached("AttachedControl",
              typeof(ControlExtension<TControl, TControlExtension>), typeof(ControlExtension<TControl, TControlExtension>));

        protected TControl AttachedControl;

        public static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        public static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        private static void ClearAttachedControl(TControl control)
        {
            var extensionBase = control.GetValue(AttachedControlProperty) as ControlExtension<TControl, TControlExtension>;
            extensionBase?.RemoveAttachedControl();

            control.ClearValue(AttachedControlProperty);
        }

        internal static ControlExtension<TControl, TControlExtension> GetAttachedControl(UIElement control) => (ControlExtension<TControl, TControlExtension>)control.GetValue(AttachedControlProperty);

        private static void SetAttachedControl(TControl control, ControlExtension<TControl, TControlExtension> extensionBase)
        {
            control.SetValue(AttachedControlProperty, extensionBase);
            extensionBase.SetAttachedControl(control);
        }

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedControl, stateName, useTransitions);

        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        private void SetAttachedControl(TControl control)
        {
            AttachedControl = control;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        private void RemoveAttachedControl()
        {
            AttachedControl = null;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;
        }

        private void UnregisterEvents()
        {
            AttachedControl.Unloaded -= OnUnloaded;
            AttachedControl.Loaded -= OnLoaded;
        }

        private void RegisterEvents()
        {
            AttachedControl.Unloaded += OnUnloaded;
            AttachedControl.Loaded += OnLoaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
        }

        protected virtual void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {

        }

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {

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
    }
}
