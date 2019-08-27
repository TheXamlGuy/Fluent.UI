using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TabItem))]
    public class AttachedTabItemTemplate : AttachedItemContainerTemplate<TabItem>
    {
        private Border _tabContainer;
        protected override void OnApplyTemplate()
        {
            _tabContainer = GetTemplateChild<Border>("TabContainer");
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsSelected)
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

            GoToVisualState(visualState, useTransitions);
        }

        protected override void RegisterEvents()
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            AddPropertyChangedHandler(TabItem.IsSelectedProperty, OnPropertyChanged);
        }

        protected override void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {
            if (args.Source is TabItem)
            {
                base.OnPointerPressed(sender, args);
            }
        }

        protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {
            if (args.Source is TabItem)
            {
                AttachedFrameworkElement.IsSelected = true;
                base.OnPointerReleased(sender, args);
            }
        }
        
        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ChangeVisualState(true);
        }
    }
}