using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    internal struct TextControlSelection
    {
        public int Start;
        public int End;
    }

    [DefaultStyleTarget(typeof(PasswordBox))]
    internal class AttachedPasswordBoxTemplate : AttachedControlTemplate<PasswordBox>
    {
        private object _header;
        private DataTemplate _headerTemplate;
        private ToggleButton _revealButton;
        private TextBox _revealTextBox;

        public void SetHeader(object header = null)
        {
            _header = header;
            ChangeHeaderVisualState();
        }

        public void SetHeaderTemplate(DataTemplate headertemplate = null)
        {
            _headerTemplate = headertemplate;
            ChangeHeaderVisualState();
        }

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (IsFocused)
            {
                visualState = CommonVisualState.Focused;
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

        protected override void OnApplyTemplate()
        {
            _revealButton = GetTemplateChild<ToggleButton>("RevealButton");
            _revealTextBox = GetTemplateChild<TextBox>("RevealTextBox");

            if (_revealButton != null)
            {
                RegisterRevealButtonEvent();
            }

            var scrollViewer = GetTemplateChild<ScrollViewer>("PART_ContentHost");
            if (scrollViewer != null)
            {
                var binding = new Binding
                {
                    Source = scrollViewer,
                    Path = new PropertyPath("Foreground"),
                    Mode = BindingMode.OneWay
                };

                BindingOperations.SetBinding(AttachedFrameworkElement, Control.ForegroundProperty, binding);
            }

            ChangeHeaderVisualState(false);
            ChangePlaceholderVisualState(false);
        }

        protected override void OnIsFocusedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ChangeDeleteButtonVisualState();
            ChangeVisualState();
        }

        protected override void RegisterEvents()
        {
            AddPropertyChangedHandler(TextBox.TextProperty, OnTextPropertyChanged);
        }

        protected override void UnregisterEvents()
        {
            UnregisterRevealButtonEvent();
        }

        private void ChangeDeleteButtonVisualState(bool useTransitions = true) => GoToVisualState(AttachedFrameworkElement.IsFocused && AttachedFrameworkElement.Password.Length > 0 ? CommonVisualState.ButtonVisible : CommonVisualState.ButtonCollapsed, useTransitions);

        private void ChangeHeaderVisualState(bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, _headerTemplate == null && _header == null ? CommonVisualState.HeaderCollapsed : CommonVisualState.HeaderVisible, useTransitions);

        private void ChangePlaceholderVisualState(bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Password.Length > 0 ? CommonVisualState.PlaceholderCollapsed : CommonVisualState.PlaceholderVisible, useTransitions);

        private void OnRevealButtonPointerDown(object sender, RoutedEventArgs args)
        {
            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                RevealPassword();
            }
        }

        private void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ChangePlaceholderVisualState();
            ChangeDeleteButtonVisualState();
        }

        private TextControlSelection GetSelection()
        {
            var selection = AttachedFrameworkElement.GetValue<TextSelection>("Selection", BindingFlags.NonPublic | BindingFlags.Instance);
            var textRangeType = selection.GetType().GetInterfaces().FirstOrDefault(x => x.Name == "ITextRange");

            var startTextPointer = textRangeType.GetValue<object>(selection, "Start");
            var endEndTextPointer = textRangeType.GetValue<object>(selection, "End");

            var start = startTextPointer.GetValue<int>("Offset", BindingFlags.Instance | BindingFlags.NonPublic);
            var end = endEndTextPointer.GetValue<int>("Offset", BindingFlags.Instance | BindingFlags.NonPublic);

            return new TextControlSelection
            { 
                Start = start, 
                End = end 
            };
        }

        private void RegisterRevealButtonEvent()
        {
            _revealButton.RemoveHandler(UIElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)OnRevealButtonPointerDown);
            _revealButton.AddHandler(UIElement.MouseLeftButtonDownEvent, (MouseButtonEventHandler)OnRevealButtonPointerDown, true);
        }

        private void RevealPassword()
        {
            if (_revealTextBox == null)
            {
                return;
            }

            var selection = GetSelection();
            _revealTextBox.Visibility = Visibility.Visible;
            _revealTextBox.Focus();

            _revealTextBox.Text = AttachedFrameworkElement.Password;
            _revealTextBox.SelectionStart = selection.Start;
            _revealTextBox.SelectionLength = selection.End - selection.Start;
        }

        private void UnregisterRevealButtonEvent()
        {
            if (_revealButton != null)
            {
                _revealButton.Click -= OnRevealButtonPointerDown;
            }
        }
    }
}