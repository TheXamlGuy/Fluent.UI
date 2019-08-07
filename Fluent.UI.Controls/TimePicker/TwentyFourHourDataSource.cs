using System.Collections.Generic;

namespace Fluent.UI.Controls
{
    internal static class TwentyFourHourDataSource
    {
        private static readonly IList<TimePickerItem> Items = new List<TimePickerItem>
        {
            new TimePickerItem
            {
                Value = 0,
                PrimaryText = "00"
            },
            new TimePickerItem
            {
                Value = 1,
                PrimaryText = "01"
            },
            new TimePickerItem
            {
                Value = 2,
                PrimaryText = "02"
            },
            new TimePickerItem
            {
                Value = 3,
                PrimaryText = "03"
            },
            new TimePickerItem
            {
                Value = 4,
                PrimaryText = "04"
            },
            new TimePickerItem
            {
                Value = 5,
                PrimaryText = "05"
            },
            new TimePickerItem
            {
                Value = 6,
                PrimaryText = "06"
            },
            new TimePickerItem
            {
                Value = 7,
                PrimaryText = "07"
            },
            new TimePickerItem
            {
                Value = 8,
                PrimaryText = "08"
            },
            new TimePickerItem
            {
                Value = 9,
                PrimaryText = "09"
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
            },
            new TimePickerItem
            {
                Value = 13,
                PrimaryText = "13"
            },
            new TimePickerItem
            {
                Value = 14,
                PrimaryText = "14"
            },
            new TimePickerItem
            {
                Value = 15,
                PrimaryText = "15"
            },
            new TimePickerItem
            {
                Value = 16,
                PrimaryText = "16"
            },
            new TimePickerItem
            {
                Value = 17,
                PrimaryText = "17"
            },
            new TimePickerItem
            {
                Value = 18,
                PrimaryText = "18"
            },
            new TimePickerItem
            {
                Value = 19,
                PrimaryText = "19"
            },
            new TimePickerItem
            {
                Value = 20,
                PrimaryText = "20"
            },
            new TimePickerItem
            {
                Value = 21,
                PrimaryText = "21"
            },
            new TimePickerItem
            {
                Value = 22,
                PrimaryText = "22"
            },
            new TimePickerItem
            {
                Value = 23,
                PrimaryText = "23"
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