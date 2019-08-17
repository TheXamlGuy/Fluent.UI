using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public partial class TextBoxExtension : FrameworkElementExtension<TextBox, TextBoxExtension>
    {
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
            handler.Add(AttachedFrameworkElement, TextBox.TextProperty, () => ChangePlaceholderVisualState());

            base.DependencyPropertyChangedHandler(handler);
        }

        protected override void OnApplyTemplate()
        {
            ChangeHeaderVisualState();
        }

        private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => OnHeaderPropertyChanged(dependencyObject as TextBox);

        private static void OnHeaderPropertyChanged(TextBox text)
        {
            if (!text.IsLoaded)
            {
                return;
            }

            var extension = GetAttachedFrameworkElement(text) as TextBoxExtension;
            extension.ChangeHeaderVisualState();
        }

        private static void OnHeaderTemplatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => OnHeaderPropertyChanged(dependencyObject as TextBox);

        private void ChangeHeaderVisualState()
        {
            var header = GetHeader(AttachedFrameworkElement);
            var headerTemplate = GetHeaderTemplate(AttachedFrameworkElement);

            VisualStateManager.GoToState(AttachedFrameworkElement, headerTemplate == null && header == null ? CommonVisualState.HeaderCollapsed : CommonVisualState.HeaderVisible, true);
        }

        private void ChangePlaceholderVisualState() => VisualStateManager.GoToState(AttachedFrameworkElement, AttachedFrameworkElement.Text.Length > 0 ? CommonVisualState.PlaceholderCollapsed : CommonVisualState.PlaceholderVisible, true);
    }
}
