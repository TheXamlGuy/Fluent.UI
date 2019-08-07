using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace Fluent.UI.Controls
{
    [ContentProperty("Icon")]
    public class AppBarToggleButton : ToggleButton, ICommandBarElement
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon),
                typeof(IconElement), typeof(AppBarToggleButton),
                new PropertyMetadata(null));

        public AppBarToggleButton()
        {
            DefaultStyleKey = typeof(AppBarToggleButton);

            PreviewMouseDown += OnMouseDown;
            PreviewMouseUp += OnMouseUp;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
            IsEnabledChanged += OnIsEnabledChanged;
            Checked += OnChecked;
            Unchecked += OnUnchecked;
            LostFocus += OnLostFocus;
            GotFocus += OnGotFocus;
        }

        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public void SetButtonVisualStates()
        {
            var state = "Unchecked";
            if (IsChecked == false)
            {
                if (IsEnabled)
                {
                    if (IsMouseOver)
                    {
                        state = Mouse.LeftButton == MouseButtonState.Pressed ? "UncheckedPressed" : "UncheckedPointerOver";
                    }
                }
                else
                {
                    state = "UncheckedDisabled";
                }
            }

            if (IsChecked == true)
            {
                if (IsEnabled)
                {
                    if (IsMouseOver)
                    {
                        state = Mouse.LeftButton == MouseButtonState.Pressed ? "CheckedPressed" : "CheckedPointerOver";
                    }
                    else
                    {
                        state = "CheckedNormal";
                    }
                }
                else
                {
                    state = "CheckedDisabled";
                }
            }

            VisualStateManager.GoToState(this, state, true);
            Debug.WriteLine(state);
        }

        private void OnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
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

        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            SetButtonVisualStates();
        }

        private void OnUnchecked(object sender, RoutedEventArgs routedEventArgs)
        {
            SetButtonVisualStates();
        }
    }
}
