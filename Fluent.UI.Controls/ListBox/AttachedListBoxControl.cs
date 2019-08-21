using Fluent.UI.Core;
using System;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ListBox))]
    public class AttachedListBoxControl : AttachedItemsControlTemplate<ListBox>
    {
        protected override Type GetContainerTypeForItem() => typeof(ListBoxItem);
    }
}