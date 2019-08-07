using System.Windows;

namespace Fluent.UI.Controls
{
    public class StateTrigger : StateTriggerBase
    { 
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive",
                typeof(bool), typeof(StateTrigger),
                new PropertyMetadata(false, OnIsActivePropertyChanged));

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        private static void OnIsActivePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var stateTrigger = dependencyObject as StateTrigger;
            stateTrigger?.OnIsActivePropertyChanged();
        }

        private void OnIsActivePropertyChanged()
        {
            SetActive(IsActive);
        }
    }
}