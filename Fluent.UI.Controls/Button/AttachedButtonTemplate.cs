using Fluent.UI.Core;
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

        private DependencyPropertyChangedSubscription borderThicknessChangeNotifier;

        protected override void OnLoaded(object sender, RoutedEventArgs args)
        {
                d.AddEventHandler(AttachedFrameworkElement, UIElement.IsMouseOverProperty);

            //AttachedFrameworkElement.PropertyChanged(UIElement.IsMouseOverProperty, (_) => ChangeVisualState(true));
            //AttachedFrameworkElement.PropertyChanged(ButtonBase.IsPressedProperty, (_) => ChangeVisualState(true));
            //AttachedFrameworkElement.PropertyChanged(UIElement.IsEnabledProperty, (_) => ChangeVisualState(true));

        }

        protected override void OnAttached()
        {
            //AttachedFrameworkElement.PropertyChanged(new PropertyPath, (_) => ChangeVisualState(true));
            //AttachedFrameworkElement.PropertyChanged(ButtonBase.IsPressedProperty, (_) => { });
            //AttachedFrameworkElement.PropertyChanged(UIElement.IsEnabledProperty, (_) => ChangeVisualState(true));
        }
    }
}