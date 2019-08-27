using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ComboBox))]
    public class AttachedComboBoxTemplate : AttachedControlTemplate<ComboBox>
    {
        protected override void ChangeVisualState(bool useTransitions = true)
        {
            //string visualState;
            //if (!IsEnabled)
            //{
            //    visualState = CommonVisualState.Disabled;
            //}
            //else if (IsPressed)
            //{
            //    visualState = CommonVisualState.Pressed;
            //}
            //else if (IsPointerOver)
            //{
            //    visualState = CommonVisualState.PointerOver;
            //}
            //else
            //{
            //    visualState = CommonVisualState.Normal;
            //}

        //    GoToVisualState(visualState, useTransitions);
        }

        protected override void OnAttached()
        {
          // AddEventHandler<MouseButtonEventArgs>("MouseLeftButtonDown", grid_ButtonMOuseDown2);
            //AttachedFrameworkElement.MouseLeftButtonDown += AttachedFrameworkElement_MouseLeftButtonDown;
           // AttachedFrameworkElement.AddHandler(ComboBox.MouseLeftButtonDownEvent, new RoutedEventHandler(grid_ButtonMOuseDown), true);
            base.OnAttached();
        }

    }
}
