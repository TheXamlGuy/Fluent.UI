using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class TextBoxExtension : FrameworkElementExtension<TextBox, TextBoxExtension>
    {
        protected override void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {
            handler.Add(AttachedFrameworkElement, UIElement.IsMouseOverProperty, () => ChangeVisualState(true));
            handler.Add(AttachedFrameworkElement, UIElement.IsFocusedProperty, () => ChangeVisualState(true));

            base.DependencyPropertyChangedHandler(handler);
        }

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
    }
}
