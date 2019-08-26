using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    public class AttachedButtonBaseTemplate<TButtonBase> : AttachedControlTemplate<TButtonBase> where TButtonBase : ButtonBase
    {
        protected bool IsPressed => AttachedFrameworkElement.IsPressed;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
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

            GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached() => AddPropertyChangedHandler(ButtonBase.IsPressedProperty, OnIsPressedPropertyChanged);

        protected virtual void OnIsPressedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => ChangeVisualState(true);
    }
}
