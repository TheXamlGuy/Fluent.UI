using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TreeViewItem))]
    public class AttachedTreeViewItemTemplate : AttachedControlTemplate<TreeViewItem>
    {
        private Border _contentPresenterBorder;
        private TreeViewItemTemplateSettings _templateSettings;

        public void SetLeftIndentLengthSettings()
        {
            var indentLength = TreeViewItemExtension.GetItemIndentLength(AttachedFrameworkElement);

            var count = AttachedFrameworkElement.FindAscendantCount<TreeViewItem, TreeView>();
            var leftIndentLengthDelta = count > 0 ? indentLength * count : 0;

            _templateSettings.SetValue(TreeViewItemTemplateSettings.ItemIndentThicknessDeltaProperty, new Thickness(leftIndentLengthDelta, 0, 0, 0));
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (AttachedFrameworkElement.IsSelected)
            {
                if (IsPointerOver)
                    visualState = CommonVisualState.SelectedPointerOver;
                else if (IsPressed)
                    visualState = CommonVisualState.SelectedPressed;
                else
                    visualState = CommonVisualState.Selected;
            }
            else
            {
                if (!IsEnabled)
                    visualState = CommonVisualState.Disabled;
                else if (IsPressed)
                    visualState = CommonVisualState.Pressed;
                else if (IsPointerOver)
                    visualState = CommonVisualState.PointerOver;
                else
                    visualState = CommonVisualState.Normal;
            }

            Debug.WriteLine(visualState);
            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnApplyTemplate()
        {
            _templateSettings = new TreeViewItemTemplateSettings();
            TreeViewItemExtension.SetTemplateSettings(AttachedFrameworkElement, _templateSettings);

            _contentPresenterBorder = GetTemplateChild<Border>("ContentPresenterBorder");
            SetLeftIndentLengthSettings();
        }

        protected override void OnClick() => AttachedFrameworkElement.IsSelected = true;

        protected override void OnPointerMove(object sender, MouseEventArgs args)
        {
            if (Mouse.Captured == null)
            {
                if (!IsPointerInPosition())
                {
                    SetIsPointerOver(false);
                }
                //var pos = Mouse.PrimaryDevice.GetPosition(_contentPresenterBorder);
                //IsPointerOver = !(pos.Y >= _contentPresenterBorder.ActualHeight);
            }

            base.OnPointerMove(sender, args);
        }

        protected override void OnPointerOver(object sender, MouseEventArgs args)
        {
            UpdateIsPointerOver();
        }

        protected override void OnPointerPressed(object sender, MouseButtonEventArgs args)
        {
            if (Mouse.Captured == null)
            {
                _contentPresenterBorder.Focus();
                _contentPresenterBorder.CaptureMouse();

                if (_contentPresenterBorder.IsMouseCaptured)
                {
                    if (args.ButtonState == MouseButtonState.Pressed)
                    {
                        if (!IsPressed)
                        {
                            SetIsPressed();
                        }
                    }
                    else
                    {
                        _contentPresenterBorder.ReleaseMouseCapture();
                    }
                }
            }
        }

        protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        {
            if (Mouse.Captured != null && _contentPresenterBorder.IsMouseCaptured)
            {
                if (IsPressed && args.ButtonState == MouseButtonState.Released)
                {
                    OnClick();
                }

                _contentPresenterBorder.ReleaseMouseCapture();
            }

            if (_contentPresenterBorder.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                UpdateIsPressed();
            }
        }

        protected override void RegisterEvents()
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            AddPropertyChangedHandler(TreeViewItem.IsSelectedProperty, OnPropertyChanged);
        }

        protected void UpdateIsPointerOver()
        {
            if (IsPointerInPosition())
            {
                if (!IsPointerOver)
                {
                    SetIsPointerOver();
                }
            }
            else if (IsPressed)
            {
                SetIsPointerOver(false);
            }
        }

        protected void UpdateIsPressed()
        {
            if (IsPointerInPosition())
            {
                if (!IsPressed)
                {
                    SetIsPressed();
                }
            }
            else if (IsPressed)
            {
                SetIsPressed(false);
            }
        }

        private bool IsPointerInPosition()
        {
            var pos = Mouse.PrimaryDevice.GetPosition(_contentPresenterBorder);
            if (pos.X >= 0 && pos.X <= _contentPresenterBorder.ActualWidth && pos.Y >= 0 && pos.Y <= _contentPresenterBorder.ActualHeight)
            {
                return true;
            }

            return false;
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();

        private void SetIsPointerOver(bool isPointerOver = true)
        {
            if (isPointerOver)
            {
                SetValue(IsPointerOverPropertyKey, true);
            }
            else
            {
                ClearValue(IsPointerOverPropertyKey);
            }
        }

        private void SetIsPressed(bool isPressed = true)
        {
            if (isPressed)
            {
                SetValue(IsPressedPropertyKey, true);
            }
            else
            {
                ClearValue(IsPressedPropertyKey);
            }
        }
    }
}