using Fluent.UI.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fluent.UI.Controls
{
    [DefaultStyleTarget(typeof(ListBox))]
    public class AttachedListBoxControlTemplate : AttachedItemsControlTemplate<ListBox>
    {
        protected override Type GetContainerTypeForItem() => typeof(ListBoxItem);
    }
}