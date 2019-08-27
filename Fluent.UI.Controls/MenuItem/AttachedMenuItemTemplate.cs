using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(MenuItem))]
    public class AttachedMenuItemTemplate : AttachedItemContainerTemplate<MenuItem>
    {
        internal bool IsRadioCheckable;
        private string _groupName;

        internal void SetGroupName(string groupName)
        {
            _groupName = groupName;
            IsRadioCheckable = !string.IsNullOrEmpty(groupName);

            AttachedFrameworkElement.IsCheckable = !IsRadioCheckable;
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
                if (!IsEnabled)
                {
                    visualState = CommonVisualState.Disabled;
                }
                else if (AttachedFrameworkElement.IsPressed)
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

        protected override void OnApplyTemplate()
        {
            ChangePlacementVisualState(false);
            ChangeCheckAndIconPlaceholderVisualState(false, true);
            ChangeCheckedVisualState(false);
        }

        protected override void RegisterEvents()
        {
            AddEventHandler<RoutedEventHandler>(MenuItem.ClickEvent, OnClick);
            AddPropertyChangedHandler(MenuItem.IsCheckedProperty, OnIsCheckedPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IconProperty, OnIconPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsCheckableProperty, OnIsCheckablePropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsSubmenuOpenProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(MenuItem.RoleProperty, OnRolePropertyChanged);
        }

        protected override void UnregisterEvents()
        {
            RemoveEventHandler<RoutedEventHandler>(MenuItem.ClickEvent, OnClick);
        }

        private void ChangeCheckAndIconPlaceholderVisualState(bool useTransitions = true, bool once = false)
        {
            if (VisualTreeHelper.GetParent(AttachedFrameworkElement) is Panel parent)
            {
                if (!once || (once && parent.Children.IndexOf(AttachedFrameworkElement) == 0))
                {
                    var visualStates = new List<string>();
                    foreach (var child in parent.FindChildren<MenuItem>())
                    {
                        if (MenuItemExtension.GetAttachedTemplate(child) is AttachedMenuItemTemplate attachedTemplate)
                        {
                            var menuItem = attachedTemplate.AttachedFrameworkElement;
                            if (menuItem.Icon != null && !attachedTemplate.IsRadioCheckable && menuItem.IsCheckable)
                            {
                                visualStates.Add("CheckAndIconPlaceholder");
                            }
                            else if (menuItem.Icon != null && attachedTemplate.IsRadioCheckable)
                            {
                                visualStates.Add("RadioCheckAndIconPlaceholder");
                            }
                            else if (menuItem.Icon != null)
                            {
                                visualStates.Add("IconPlaceholder");
                            }
                            else if (attachedTemplate.IsRadioCheckable)
                            {
                                visualStates.Add("RadioCheckPlaceholder");
                            }
                            else if (menuItem.IsCheckable)
                            {
                                visualStates.Add("CheckPlaceholder");
                            }
                            else
                            {
                                visualStates.Add("NoPlaceholder");
                            }
                        }
                    }

                    if (visualStates.Any())
                    {
                        var mostCommonVisualState = visualStates.GroupBy(x => x).OrderBy(g => g.Key).FirstOrDefault().Key;
                        foreach (var child in GetSiblings())
                        {
                            VisualStateManager.GoToState(child, mostCommonVisualState, useTransitions);
                            Debug.WriteLine(mostCommonVisualState);
                        }
                    }
                }
            }
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

        private IEnumerable<MenuItem> GetSiblings()
        {
            if (VisualTreeHelper.GetParent(AttachedFrameworkElement) is FrameworkElement parent)
            {
                foreach (var child in parent.FindChildren<MenuItem>())
                {
                    yield return child;
                }
            }
        }

        private void OnClick(object sender, RoutedEventArgs args)
        {
            if (IsRadioCheckable)
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
            foreach (var child in GetSiblings())
            {
                if (child != AttachedFrameworkElement && MenuItemExtension.GetGroupName(child) == _groupName)
                {
                    child.IsChecked = false;
                }
            }
        }
    }
}

