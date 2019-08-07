using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Controls
{
    internal class YearDataSource : DependencyObject
    {
        internal static DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items),
                typeof(IList<TimePickerItem>), typeof(YearDataSource),
                new PropertyMetadata(null));

        internal static DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                typeof(TimePickerItem), typeof(YearDataSource),
                new PropertyMetadata(null));

        internal TimePickerItem SelectedItem
        {
            get => (TimePickerItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        internal IList<TimePickerItem> Items
        {
            get => (IList<TimePickerItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public YearDataSource()
        {
            Items = Enumerable.Range(DateTime.Now.Year - 60, 100).Select(item => new TimePickerItem
            {
                Value = item,
                PrimaryText = item.ToString()
            }).ToList();
        }

        internal void SetDefaultItem(DateTime dateTime)
        {
            var item = Items.FirstOrDefault(x => x.Value.Equals(dateTime.Year));
            if (item != null)
            {
                SetValue(SelectedItemProperty, item);
            }
        }
    }
}