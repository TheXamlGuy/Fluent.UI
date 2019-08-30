using System.Windows;
using System.Windows.Controls.Primitives;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class AttachedButtonBaseTemplate<TButtonBase> : AttachedControlTemplate<TButtonBase>
        where TButtonBase : ButtonBase
    {
        public new bool IsPressed => AttachedFrameworkElement.IsPressed;

        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
                visualState = CommonVisualState.Disabled;
            else if (IsPressed)
                visualState = CommonVisualState.Pressed;
            else if (IsPointerOver)
                visualState = CommonVisualState.PointerOver;
            else
                visualState = CommonVisualState.Normal;

            GoToVisualState(visualState, useTransitions);
        }

        protected override void RegisterEvents()
        {
            AddPropertyChangedHandler(ButtonBase.IsPressedProperty, OnIsPressedPropertyChanged);
        }

        protected override void OnIsPressedPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            ChangeVisualState();
        }
    }
}