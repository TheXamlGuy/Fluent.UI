using System.Collections.Generic;

namespace Fluent.UI.Controls
{
    public static class TwelveHourDataSource
    {
        private static readonly IList<TimePickerItem> Items = new List<TimePickerItem>
        {
            new TimePickerItem
            {
                Value = 0,
                PrimaryText = "0"
            },
            new TimePickerItem
            {
                Value = 1,
                PrimaryText = "1"
            },
            new TimePickerItem
            {
                Value = 2,
                PrimaryText = "2"
            },
            new TimePickerItem
            {
                Value = 3,
                PrimaryText = "3"
            },
            new TimePickerItem
            {
                Value = 4,
                PrimaryText = "4"
            },
            new TimePickerItem
            {
                Value = 5,
                PrimaryText = "5"
            },
            new TimePickerItem
            {
                Value = 6,
                PrimaryText = "6"
            },
            new TimePickerItem
            {
                Value = 7,
                PrimaryText = "7"
            },
            new TimePickerItem
            {
                Value = 8,
                PrimaryText = "8"
            },
            new TimePickerItem
            {
                Value = 9,
                PrimaryText = "9"
            },
            new TimePickerItem
            {
                Value = 10,
                PrimaryText = "10"
            },
            new TimePickerItem
            {
                Value = 11,
                PrimaryText = "11"
            },
            new TimePickerItem
            {
                Value = 12,
                PrimaryText = "12"
            }
        };

        public static IList<TimePickerItem> GetItems()
        {
            return Items;
        }

        public static TimePickerItem GetItemFromIndex(int index)
        {
            return Items[index];
        }
    }
}