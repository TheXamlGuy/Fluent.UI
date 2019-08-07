using System.Windows;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    public class MenuFlyoutItem : MenuFlyoutItemBase
    {
        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter),
                typeof(object), typeof(MenuFlyoutItem),
                new PropertyMetadata(null));

        public static DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command),
                typeof(ICommand), typeof(MenuFlyoutItem),
                new PropertyMetadata(null));

        public static DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon),
                typeof(IconElement), typeof(MenuFlyoutItem),
                new PropertyMetadata(null));

        public static DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text),
                typeof(string), typeof(MenuFlyoutItem),
                new PropertyMetadata(null));

        public MenuFlyoutItem()
        {
            DefaultStyleKey = typeof(MenuFlyoutItem);
        }

        public event RoutedEventHandler Click;

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void ItemClick()
        {
            Click?.Invoke(this, new RoutedEventArgs());
            Command?.Execute(CommandParameter);

            Owner.ItemClick();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            OnVisualStatesChanged();
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            OnVisualStatesChanged();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            OnVisualStatesChanged();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            OnVisualStatesChanged();
            ItemClick();
            base.OnMouseUp(e);
        }

        private void OnVisualStatesChanged()
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
    }
}