using Fluent.UI.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    public class AttachedButtonBaseTemplate<TButtonBase> : AttachedControlTemplate<TButtonBase> where TButtonBase : ButtonBase
    {
    //    protected new bool IsPressed => AttachedFrameworkElement.IsPressed;

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

            Debug.WriteLine(visualState);
            GoToVisualState(visualState, useTransitions);
        }

        protected override void RegisterEvents()
        {
           // AddPropertyChangedHandler(ButtonBase.IsPressedProperty, OnIsPressedPropertyChanged1);
        }

        //protected override void OnPointerLeave(object sender, RoutedEventArgs args)
        //{
        //}

        //protected override void OnPointerReleased(object sender, MouseButtonEventArgs args)
        //{
        //}

        //protected override void OnLostMouseCapture(object sender, MouseEventArgs args)
        //{
        //}

        //protected virtual void OnIsPressedPropertyChanged1(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        //{
        //    ChangeVisualState(true);
        //}
    }
}
