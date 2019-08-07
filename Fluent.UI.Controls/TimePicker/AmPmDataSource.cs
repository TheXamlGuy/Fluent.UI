using System.Collections.Generic;

namespace Fluent.UI.Controls
{
    internal static class AmPmDataSource
    {
        private static readonly IList<TimePickerItem> Items = new List<TimePickerItem>
        {
            new TimePickerItem
            {
                Value = "AM",
                PrimaryText = "AM"
            },
            new TimePickerItem
            {
                Value = "PM",
                PrimaryText = "PM"
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