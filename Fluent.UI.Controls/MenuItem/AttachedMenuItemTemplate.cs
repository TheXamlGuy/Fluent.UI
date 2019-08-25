using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(MenuItem))]
    public class AttachedMenuItemTemplate : AttachedItemContainerTemplate<MenuItem>
    {
        private string _groupName;
        private bool _isRadioCheckable;

        internal void SetGroupName(string groupName)
        {
            _groupName = groupName;
            _isRadioCheckable = !string.IsNullOrEmpty(groupName);

            AttachedFrameworkElement.IsCheckable = !_isRadioCheckable;
        }

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
            ChangeCheckedVisualState(false);
        }

        protected override void OnAttached()
        {
            AddEventHandler<RoutedEventArgs>("Click", OnClick);
            AddPropertyChangedHandler(MenuItem.IsCheckedProperty, OnIsCheckedPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IconProperty, OnIconPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsCheckableProperty, OnIsCheckablePropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsSubmenuOpenProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.RoleProperty, OnRolePropertyChanged);
        }

        private void ChangeCheckAndIconPlaceholderVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.Icon != null && !_isRadioCheckable && AttachedFrameworkElement.IsCheckable)
            {
                visualState = "CheckAndIconPlaceholder";
            }
            else if (AttachedFrameworkElement.Icon != null && _isRadioCheckable)
            {
                visualState = "RadioCheckAndIconPlaceholder";
            }
            else if (AttachedFrameworkElement.Icon != null)
            {
                visualState = "IconPlaceholder";
            }
            else if (_isRadioCheckable)
            {
                visualState = "RadioCheckPlaceholder";
            }
            else if (AttachedFrameworkElement.IsCheckable)
            {
                visualState = "CheckPlaceholder";
            }
            else
            {
                visualState = "NoPlaceholder";
            }

            GoToVisualState(visualState, useTransitions);
        }

        private void ChangeCheckedVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsChecked)
            {
                visualState = CommonVisualState.Checked;
            }
            else
            {
                visualState = CommonVisualState.Unchecked;
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
            else if (AttachedFrameworkElement.Role == MenuItemRole.SubmenuHeader)
            {
                visualState = "SubmenuHeader";
            }
            else if (AttachedFrameworkElement.Role == MenuItemRole.SubmenuItem)
            {
                visualState = "SubmenuItem";
            }

            GoToVisualState(visualState, useTransitions);
        }

        private void OnClick(object sender, RoutedEventArgs args)
        {
            if (_isRadioCheckable)
            {
                if (AttachedFrameworkElement.IsChecked)
                {
                    return;
                }

                AttachedFrameworkElement.IsChecked = true;
                UpdateSiblings();
            }
        }

        private void OnIconPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckAndIconPlaceholderVisualState(true);

        private void OnIsCheckablePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckAndIconPlaceholderVisualState(true);

        private void OnIsCheckedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeCheckedVisualState(true);

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        private void OnRolePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangePlacementVisualState(true);

        private void UpdateSiblings()
        {
            if (VisualTreeHelper.GetParent(AttachedFrameworkElement) is FrameworkElement parent)
            {
                foreach (var child in parent.FindChildren<MenuItem>())
                {
                    if (child != AttachedFrameworkElement && MenuItemExtension.GetGroupName(child) == _groupName)
                    {
                        child.IsChecked = false;
                    }
                }
            }
        }
    }
}

