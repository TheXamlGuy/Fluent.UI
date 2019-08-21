using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TextBox))]
    internal class AttachedTextBoxTemplate : AttachedControlTemplate<TextBox>
    {
        private Button _deleteButton;

        private object _header;

        private DataTemplate _headerTemplate;

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
            else if (IsMouseOver)
            {
                visualState = CommonVisualState.PointerOver;
            }
            else
            {
                visualState = CommonVisualState.Normal;
            }

            GoToVisualState(visualState, useTransitions);
        }

        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsFocusedProperty, OnFocusedChanged);
            handler.Add(AttachedFrameworkElement, TextBox.TextProperty, OnTextChanged);

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void OnApplyTemplate()
        {
            _deleteButton = GetTemplateChild<Button>("DeleteButton");
            if (_deleteButton != null)
            {
                _deleteButton.Click -= OnDeleteButtonClick;
                _deleteButton.Click += OnDeleteButtonClick;
            }

            var scrollViewer = GetTemplateChild<ScrollViewer>("PART_ContentHost");
            var binding = new Binding
            {
                Source = scrollViewer,
                Path = new PropertyPath("Foreground"),
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(AttachedFrameworkElement, Control.ForegroundProperty, binding);

            ChangeHeaderVisualState(false);
            ChangePlaceholderVisualState(false);
        }

        protected override void OnDetached()
        {
            if (_deleteButton != null)
            {
                _deleteButton.Click -= OnDeleteButtonClick;
            }
        }

        private void ChangeDeleteButtonVisualState(bool useTransitions = true) => GoToVisualState(AttachedFrameworkElement.IsFocused && AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.ButtonVisible : CommonVisualState.ButtonCollapsed, useTransitions);

        private void ChangeHeaderVisualState(bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, _headerTemplate == null && _header == null ? CommonVisualState.HeaderCollapsed : CommonVisualState.HeaderVisible, useTransitions);

        private void ChangePlaceholderVisualState(bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.PlaceholderCollapsed : CommonVisualState.PlaceholderVisible, useTransitions);

        private void OnDeleteButtonClick(object sender, RoutedEventArgs args) => AttachedFrameworkElement.Text = "";

        private void OnFocusedChanged() => ChangeDeleteButtonVisualState();

        private void OnTextChanged()
        {
            ChangePlaceholderVisualState();
            ChangeDeleteButtonVisualState();
        }
    }
}