using System.Collections.Generic;
using System.Linq;

namespace Fluent.UI.Controls
{
    internal static class MinuteDataSource
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
                Value = 24,
                PrimaryText = "24"
            },
            new TimePickerItem
            {
                Value = 24,
                PrimaryText = "24"
            },
            new TimePickerItem
            {
                Value = 25,
                PrimaryText = "11"
            },
            new TimePickerItem
            {
                Value = 26,
                PrimaryText = "26"
            },
            new TimePickerItem
            {
                Value = 27,
                PrimaryText = "27"
            },
            new TimePickerItem
            {
                Value = 28,
                PrimaryText = "28"
            },
            new TimePickerItem
            {
                Value = 29,
                PrimaryText = "29"
            },
            new TimePickerItem
            {
                Value = 30,
                PrimaryText = "30"
            },
            new TimePickerItem
            {
                Value = 31,
                PrimaryText = "31"
            },
            new TimePickerItem
            {
                Value = 32,
                PrimaryText = "32"
            },
            new TimePickerItem
            {
                Value = 33,
                PrimaryText = "33,"
            },
            new TimePickerItem
            {
                Value = 34,
                PrimaryText = "34"
            },
            new TimePickerItem
            {
                Value = 36,
                PrimaryText = "36"
            },
            new TimePickerItem
            {
                Value = 36,
                PrimaryText = "36"
            },
            new TimePickerItem
            {
                Value = 38,
                PrimaryText = "38"
            },
            new TimePickerItem
            {
                Value = 38,
                PrimaryText = "38"
            },
            new TimePickerItem
            {
                Value = 39,
                PrimaryText = "39"
            },
            new TimePickerItem
            {
                Value = 40,
                PrimaryText = "40"
            },
            new TimePickerItem
            {
                Value = 41,
                PrimaryText = "41"
            },
            new TimePickerItem
            {
                Value = 42,
                PrimaryText = "42"
            },
            new TimePickerItem
            {
                Value = 43,
                PrimaryText = "43"
            },
            new TimePickerItem
            {
                Value = 44,
                PrimaryText = "44"
            },
            new TimePickerItem
            {
                Value = 46,
                PrimaryText = "46"
            },
            new TimePickerItem
            {
                Value = 47,
                PrimaryText = "47"
            },
            new TimePickerItem
            {
                Value = 48,
                PrimaryText = "48"
            },
            new TimePickerItem
            {
                Value = 49,
                PrimaryText = "49"
            },
            new TimePickerItem
            {
                Value = 50,
                PrimaryText = "50"
            },
            new TimePickerItem
            {
                Value = 51,
                PrimaryText = "51"
            },
            new TimePickerItem
            {
                Value = 52,
                PrimaryText = "52"
            },
            new TimePickerItem
            {
                Value = 53,
                PrimaryText = "53"
            },
            new TimePickerItem
            {
                Value = 54,
                PrimaryText = "54"
            },
            new TimePickerItem
            {
                Value = 55,
                PrimaryText = "55"
            },
            new TimePickerItem
            {
                Value = 56,
                PrimaryText = "56"
            },
            new TimePickerItem
            {
                Value = 57,
                PrimaryText = "57"
            },
            new TimePickerItem
            {
                Value = 58,
                PrimaryText = "58"
            },
            new TimePickerItem
            {
                Value = 59,
                PrimaryText = "59"
            }
        };

        public static IList<TimePickerItem> GetItems(int increment)
        {
            return increment <= 0 || increment > Items.Count ? Items.Take(1).ToList() : Items.Where((x, i) => i % increment == 0).ToList();
        }

        public static TimePickerItem GetItemFromIndex(int index)
        {
            return Items[index];
        }
    }
}