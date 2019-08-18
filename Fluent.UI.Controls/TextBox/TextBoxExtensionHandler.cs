using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    internal class TextBoxExtensionHandler : FrameworkElementExtensionHandler<TextBox>
    {
        private Button _deleteButton;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!AttachedFrameworkElement.IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsFocused)
            {
                visualState = CommonVisualState.Focused;
            }
            else if (AttachedFrameworkElement.IsMouseOver)
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
        }

        protected override void OnUnloaded()
        {
            if (_deleteButton != null)
            {
                _deleteButton.Click -= OnDeleteButtonClick;
            }
        }

        private void ChangeDeleteButtonVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.ButtonVisible : CommonVisualState.ButtonCollapsed, true);

        private void ChangeHeaderVisualState()
        {
            //var header = GetHeader(AttachedFrameworkElement);
            //var headerTemplate = GetHeaderTemplate(AttachedFrameworkElement);

            //VisualStateManager.GoToState(AttachedFrameworkElement, headerTemplate == null && header == null ? CommonVisualState.HeaderCollapsed : CommonVisualState.HeaderVisible, true);
        }

        private void ChangePlaceholderVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.PlaceholderCollapsed : CommonVisualState.PlaceholderVisible, true);

        private void OnDeleteButtonClick(object sender, RoutedEventArgs args) => AttachedFrameworkElement.Text = "";
        
        private void OnTextChanged()
        {
            ChangePlaceholderVisualState();
            ChangeDeleteButtonVisualState();
        }
    }
}
