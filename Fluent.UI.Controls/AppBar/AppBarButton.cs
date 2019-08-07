using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Fluent.UI.Controls
{
    [ContentProperty("Icon")]
    public class AppBarButton : Button, ICommandBarElement
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon),
                typeof(IconElement), typeof(AppBarButton),
                new PropertyMetadata(null));

        public AppBarButton()
        {
            DefaultStyleKey = typeof(AppBarButton);

            PreviewMouseDown += OnMouseDown;
            PreviewMouseUp += OnMouseUp;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            IsEnabledChanged += OnIsEnabledChanged;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            SetButtonVisualStates();
        }

        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public void SetButtonVisualStates()
        {
            if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, Mouse.LeftButton == MouseButtonState.Pressed ? "Pressed" : "PointerOver", true);
            }
            else
            {
                VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
            }
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetButtonVisualStates();
        }
    }
}
