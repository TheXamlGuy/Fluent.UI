using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    internal class TextBoxExtensionHandler : FrameworkElementExtensionHandler<TextBox>
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
            handler.Add(AttachedFrameworkElement, UIElement.IsFocusedProperty, () => ChangeVisualState(true));
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

            ChangeHeaderVisualState();
            ChangePlaceholderVisualState();
        }

        protected override void OnUnloaded()
        {
            if (_deleteButton != null)
            {
                _deleteButton.Click -= OnDeleteButtonClick;
            }
        }

        private void ChangeDeleteButtonVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.ButtonVisible : CommonVisualState.ButtonCollapsed, true);

        private void ChangeHeaderVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, _headerTemplate == null && _header == null ? CommonVisualState.HeaderCollapsed : CommonVisualState.HeaderVisible, true);

        private void ChangePlaceholderVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.PlaceholderCollapsed : CommonVisualState.PlaceholderVisible, true);

        private void OnDeleteButtonClick(object sender, RoutedEventArgs args) => AttachedFrameworkElement.Text = "";

        private void OnTextChanged()
        {
            ChangePlaceholderVisualState();
            ChangeDeleteButtonVisualState();
        }
    }
}