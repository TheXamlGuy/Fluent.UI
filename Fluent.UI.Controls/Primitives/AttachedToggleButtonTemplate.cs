using System.Windows;
using System.Windows.Controls.Primitives;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class AttachedToggleButtonTemplate<TToggleButton> : AttachedButtonBaseTemplate<TToggleButton>
        where TToggleButton : ToggleButton
    {
        internal bool? IsChecked => AttachedFrameworkElement.IsChecked;

        protected virtual void OnIsCheckedPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            ChangeCheckedVisualState();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            AddPropertyChangedHandler(ToggleButton.IsCheckedProperty, OnIsCheckedPropertyChanged);
        }

        private void ChangeCheckedVisualState(bool useTransitions = true)
        {
            GoToVisualState(IsChecked == true ? CommonVisualState.Checked : CommonVisualState.Unchecked,
                useTransitions);
        }
    }
}