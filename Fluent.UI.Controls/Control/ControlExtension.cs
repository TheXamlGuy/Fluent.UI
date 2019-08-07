using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
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

        internal static ControlExtension<TControl, TControlExtension> GetAttachedControl(UIElement control)
        {
            return (ControlExtension<TControl, TControlExtension>)control.GetValue(AttachedControlProperty);
        }

        internal static void SetAttachedControl(TControl control, ControlExtension<TControl, TControlExtension> extensionBase)
        {
            control.SetValue(AttachedControlProperty, extensionBase);
            extensionBase.PepareToAttach(control);
        }

        internal virtual void PepareToAttach(TControl control)
        {
            AttachedControl = control;

            AttachedControl.Loaded -= OnLoaded;
            AttachedControl.Loaded += OnLoaded;

            AttachedControl.MouseLeftButtonDown -= OnPointerPressed;
            AttachedControl.MouseLeftButtonDown += OnPointerPressed;

            AttachedControl.MouseEnter -= OnPointerOver;
            AttachedControl.MouseEnter += OnPointerOver;

            OnAttached(control);
        }

        internal virtual void OnPointerOver(object sender, MouseEventArgs args)
        {

        }

        internal virtual void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {

        }

        internal void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedControl, stateName, useTransitions);

        internal virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            OnApplyTemplate();
        }

        internal static void ClearAttachedControl(TControl element)
        {
            element.ClearValue(AttachedControlProperty);
        }

        protected TControl AttachedControl;

        internal virtual void OnApplyTemplate()
        {

        }

        internal virtual void OnAttached(TControl element)
        {
           
        }

        public static bool GetIsAttached(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsAttachedProperty);
        }

        public static void SetIsAttached(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsAttachedProperty, value);
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
