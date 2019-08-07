using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class MenuFlyoutPresenter : ItemsControl
    {
        public MenuFlyoutPresenter()
        {
            DefaultStyleKey = typeof(MenuFlyoutPresenter);
        }

        internal MenuFlyout Owner { get; set; }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element is MenuFlyoutItemBase menuFlyoutItemBase)
            {
                menuFlyoutItemBase.Owner = this;
            }
        }

        internal void ItemClick()
        {
            Owner?.Hide();
        }
    }
}
