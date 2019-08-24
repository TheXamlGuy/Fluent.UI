using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(MenuItem))]
    public class AttachedMenuItemTemplate : AttachedItemContainerTemplate<MenuItem>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;

            if (AttachedFrameworkElement.IsSubmenuOpen)
            {
                visualState = CommonVisualState.Selected;
            }
            else
            {
                if (!AttachedFrameworkElement.IsEnabled)
                {
                    visualState = CommonVisualState.Disabled;
                }
                else if (AttachedFrameworkElement.IsPressed)
                {
                    visualState = CommonVisualState.Pressed;
                }
                else if (IsMouseOver)
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

        protected override void OnApplyTemplate()
        {
            ChangePlacementVisualState(false);
        }

        private void ChangePlacementVisualState(bool useTransitions = true)
        {
            string visualState = "";
            if (AttachedFrameworkElement.Role == MenuItemRole.TopLevelHeader)
            {
                visualState = "TopLevelHeader";
            }
            else if (AttachedFrameworkElement.Role == MenuItemRole.SubmenuItem)
            {
                visualState = "SubmenuItem";
            }
            
            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
            AddPropertyChangedHandler(MenuItem.IsSubmenuOpenProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.RoleProperty, OnRolePropertyChanged);
        }

        private void OnRolePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangePlacementVisualState(true);

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);
    }
}

