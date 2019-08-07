using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Fluent.UI.Controls
{
    internal class MonthDataSource : DependencyObject
    {
        internal static DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items),
                typeof(IList<TimePickerItem>), typeof(MonthDataSource),
                new PropertyMetadata(null));

        internal static DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                typeof(TimePickerItem), typeof(MonthDataSource),
                new PropertyMetadata(null));

        public MonthDataSource()
        {
            Items = new List<TimePickerItem>();

            var months = DateTimeFormatInfo.InvariantInfo.MonthNames;
            for (var i = 0; i < 12; i++)
            {
                Items.Add(new TimePickerItem
                {
                    Value = i + 1,
                    PrimaryText = months[i]
                });
            }
        }

        internal IList<TimePickerItem> Items
        {
            get => (IList<TimePickerItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        internal TimePickerItem SelectedItem
        {
            get => (TimePickerItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        internal void SetDefaultItem(DateTime dateTime)
        {
            var item = Items.FirstOrDefault(x => x.Value.Equals(dateTime.Month));
            if (item != null)
            {
                SetValue(SelectedItemProperty, item);
            }
        }
    }
}