using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class ProgressRing : Control
    {
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive",
                typeof(bool), typeof(ProgressRing), 
                new PropertyMetadata(false, OnIsActivePropertyChanged));

        protected Border PartBorder;

        public ProgressRing()
        {
            DefaultStyleKey = typeof(ProgressRing);
        }

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        public override void OnApplyTemplate()
        {
            OnChangeVisualStates();
        }

        private static void OnIsActivePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as ProgressRing;
            sender?.OnIsActivePropertyChanged();
        }

        private void OnChangeVisualStates()
        {
            VisualStateManager.GoToState(this, IsActive ? "Active" : "Inactive", true);
        }

        private void OnIsActivePropertyChanged()
        {
            OnChangeVisualStates();
        }
    }
}