using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(MenuItem))]
    public class AttachedMenuItemTemplate : AttachedItemContainerTemplate<MenuItem>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;

            if (AttachedFrameworkElement.Role == MenuItemRole.TopLevelHeader && AttachedFrameworkElement.IsSubmenuOpen)
            {
                visualState = CommonVisualState.Selected;
            }
            else if (AttachedFrameworkElement.IsSubmenuOpen)
            {
                visualState = "SubMenuOpened";
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
            ChangeCheckAndIconPlaceholderVisualState(false);
        }

        protected override void OnAttached()
        {
            AddPropertyChangedHandler(MenuItem.IconProperty, OnIsCheckablePropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsCheckableProperty, OnIconPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsSubmenuOpenProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.RoleProperty, OnRolePropertyChanged);
        }

        private void ChangeCheckAndIconPlaceholderVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.Icon != null && AttachedFrameworkElement.IsCheckable)
            {
                visualState = "CheckAndIconPlaceholder";
            }
            else if (AttachedFrameworkElement.IsCheckable)
            {
                visualState = "CheckPlaceholder";
            }
            else if (AttachedFrameworkElement.Icon != null)
            {
                visualState = "IconPlaceholder";
            }
            else
            {
                visualState = "NoPlaceholder";

            }

            GoToVisualState(visualState, useTransitions);
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

        private void OnIconPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckAndIconPlaceholderVisualState(true);

        private void OnIsCheckablePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckAndIconPlaceholderVisualState(true);
        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        private void OnRolePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangePlacementVisualState(true);
    }
}

