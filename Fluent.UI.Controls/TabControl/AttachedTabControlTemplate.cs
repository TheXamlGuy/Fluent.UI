using Fluent.UI.Core;
using System;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(TabControl))]
    public class AttachedTabControlTemplate : AttachedItemsControlTemplate<TabControl>
    {
        protected override Type GetContainerTypeForItem() => typeof(TabItem);
    }
}