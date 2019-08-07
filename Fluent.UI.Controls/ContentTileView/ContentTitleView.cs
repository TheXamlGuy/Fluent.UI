using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class ContentTitleView : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentTile();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }
    }
}
