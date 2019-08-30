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

            GoToVisualState(visualState, useTransitions);
        }

        //var pos = Mouse.PrimaryDevice.GetPosition(_contentPresenterBorder);
        //    if (pos.X >= 0 && pos.X <= _contentPresenterBorder.ActualWidth && pos.Y >= 0 && pos.Y <= _contentPresenterBorder.ActualHeight)
        //{
        //    base.OnPointerPressed(sender, args);
        //}

    protected override void OnApplyTemplate()
        {
            _templateSettings = new TreeViewItemTemplateSettings();
            TreeViewItemExtension.SetTemplateSettings(AttachedFrameworkElement, _templateSettings);

            _contentPresenterBorder = GetTemplateChild<Border>("ContentPresenterBorder");
            SetLeftIndentLengthSettings();
        }

        protected override void OnClick() => AttachedFrameworkElement.IsSelected = true;

        //protected override void OnPointerMove(object sender, MouseEventArgs args)
        //{
        //    if (Mouse.Captured == null)
        //    {
        //        var pos = Mouse.PrimaryDevice.GetPosition(_contentPresenterBorder);
        //        IsPointerOver = !(pos.Y >= _contentPresenterBorder.ActualHeight);
        //    }

        //    base.OnPointerMove(sender, args);
        //}

        //protected override void OnPointerOver(object sender, MouseEventArgs args)
        //{
        //    var pos = Mouse.PrimaryDevice.GetPosition(_contentPresenterBorder);
        //    if (pos.X >= 0 && pos.X <= _contentPresenterBorder.ActualWidth && pos.Y >= 0 && pos.Y <= _contentPresenterBorder.ActualHeight)
        //    {
        //        base.OnPointerOver(sender, args);
        //    }
        //}

        private void SetIsPressed(bool pressed)
        {
            if (pressed)
            {
                SetValue(IsPressedPropertyKey, true);
            }
            else
            {
                ClearValue(IsPressedPropertyKey);
            }
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
                            SetIsPressed(true);
                        }
                    }
                    else
                    {
                        _contentPresenterBorder.ReleaseMouseCapture();
                    }
                }
            }
        }

        protected override void RegisterEvents()
        {
            AttachedFrameworkElement.SetCurrentValue(UIElement.FocusableProperty, false);
            AddPropertyChangedHandler(TreeViewItem.IsSelectedProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState();
    }
}