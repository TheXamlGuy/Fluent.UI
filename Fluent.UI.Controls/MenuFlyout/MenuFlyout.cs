using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Fluent.UI.Controls
{
    [ContentProperty("Items")]
    public class MenuFlyout : FlyoutBase
    {
        public static DependencyProperty MenuFlyoutPresenterStyleProperty =
            DependencyProperty.Register(nameof(MenuFlyoutPresenterStyle),
                typeof(Style), typeof(MenuFlyout),
                new PropertyMetadata(null));

        public MenuFlyout()
        {
            Items = new ObservableCollection<MenuFlyoutItemBase>();
        }

        public ObservableCollection<MenuFlyoutItemBase> Items { get; set; }

        public Style MenuFlyoutPresenterStyle
        {
            get => (Style)GetValue(MenuFlyoutPresenterStyleProperty);
            set => SetValue(MenuFlyoutPresenterStyleProperty, value);
        }

        protected override Control CreatePresenter()
        {
            var menuFlyoutPresenter = new MenuFlyoutPresenter
            {
                Owner = this
            };

            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Items")
            };

            BindingOperations.SetBinding(menuFlyoutPresenter, ItemsControl.ItemsSourceProperty, binding);

            return menuFlyoutPresenter;
        }
    }
}