using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Core
{
    public class AttachedControlTemplate<TControl> : AttachedFrameworkElementTemplate<TControl> where TControl : Control
    {
        public override void OnApplyRequestedTheme(ElementTheme requestedTheme)
        {
            var elementType = AttachedFrameworkElement.GetType();
            var style = RequestedThemeFactory.Current.Create(elementType, requestedTheme);

            AttachedFrameworkElement.SetValue(FrameworkElement.StyleProperty, style);
            AttachedFrameworkElement.UpdateLayout();
            AttachedFrameworkElement.UpdateDefaultStyle();

            OnApplyTemplate();
            ChangeVisualState(true);
        }
    }
}