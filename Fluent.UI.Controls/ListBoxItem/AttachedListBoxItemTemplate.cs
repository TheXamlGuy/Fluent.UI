using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ListBoxItem))]
    public class AttachedListBoxItemTemplate : AttachedItemContainerTemplate<ListBoxItem>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsSelected)
            {
                if (!IsPressed && IsPointerOver)
                {
                    visualState = CommonVisualState.SelectedPointerOver;
                }
                else if (IsPressed)
                {
                    visualState = CommonVisualState.SelectedPressed;
                }
                else
                {
                    visualState = CommonVisualState.Selected;
                }
            }
            else
            {
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = CommonVisualState.Disabled;
                }
                else if (IsPressed)
                {
                    visualState = CommonVisualState.Pressed;
                }
                else if (IsPointerOver)
                {
                    visualState = CommonVisualState.PointerOver;
                }
                else
                {
                    visualState = CommonVisualState.Normal;
                }
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void RegisterEvents()
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            AddPropertyChangedHandler(ListBoxItem.IsSelectedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected override void OnClick()
        {
            AttachedFrameworkElement.IsSelected = true;
        }
    }
}