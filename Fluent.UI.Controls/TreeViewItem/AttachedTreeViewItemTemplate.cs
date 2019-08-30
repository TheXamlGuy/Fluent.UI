using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TreeViewItem))]
    public class AttachedTreeViewItemTemplate : AttachedControlTemplate<TreeViewItem>
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

        private TreeViewItemTemplateSettings _templateSettings;

        protected override void OnApplyTemplate()
        {
            _templateSettings = new TreeViewItemTemplateSettings();
            TreeViewItemExtension.SetTemplateSettings(AttachedFrameworkElement, _templateSettings);

            SetLeftIndentLengthSettings();
        }

        public void SetLeftIndentLengthSettings()
        {
            var indentLength = TreeViewItemExtension.GetItemIndentLength(AttachedFrameworkElement);

            var count = 1;
            var leftIndentLengthDelta = count > 0 ? indentLength * count : 0;
            var foo = GetDepth(AttachedFrameworkElement);

            _templateSettings.SetValue(TreeViewItemTemplateSettings.ItemIndentThicknessDeltaProperty, new Thickness(leftIndentLengthDelta, 0, 0, 0));
        }

        private TreeViewItem GetParent(TreeViewItem item)
        {
            TreeViewItem parent;

            while (!(parent is TreeViewItem || parent is TreeView) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        protected override void RegisterEvents()
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            AddPropertyChangedHandler(TreeViewItem.IsSelectedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);

        protected override void OnClick()
        {
            AttachedFrameworkElement.IsSelected = true;
        }
    }
}

