using Fluent.UI.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(Button))]
    public class AttachedButtonTemplate : AttachedControlTemplate<Button>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            string visualState;
            if (!IsEnabled)
            {
                visualState = CommonVisualState.Disabled;
            }
            else if (AttachedFrameworkElement.IsPressed)
            {
                visualState = CommonVisualState.Pressed;
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

        protected override void OnAttached()
        {
            AddPropertyChangedHandler(UIElement.IsEnabledProperty, OnPropertyChanged);
            AddPropertyChangedHandler(ButtonBase.IsPressedProperty, OnPropertyChanged);
            AddPropertyChangedHandler(UIElement.IsMouseOverProperty, OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, EventArgs args)
        {
            ChangeVisualState(true);
        }
    }
}