using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class AttachedToggleButtonTemplate<TToggleButton> : AttachedButtonBaseTemplate<TToggleButton> where TToggleButton : ToggleButton
    {
        internal bool? IsChecked => AttachedFrameworkElement.IsChecked;

        protected virtual void OnIsCheckedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckedVisualState(true);

        protected override void RegisterEvents()
        {
            AddPropertyChangedHandler(ToggleButton.IsCheckedProperty, OnIsCheckedPropertyChanged);
        }

        private void ChangeCheckedVisualState(bool useTransitions = true)
        {
            string visualState;
            if (IsChecked == true)
            {
                visualState = CommonVisualState.Checked;
            }
            else
            {
                visualState = CommonVisualState.Unchecked;
            }

            GoToVisualState(visualState, useTransitions);
        }
    }
}
