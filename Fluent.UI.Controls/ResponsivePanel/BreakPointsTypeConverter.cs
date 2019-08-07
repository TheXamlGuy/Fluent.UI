using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Fluent.UI.Controls
{
    public class BreakPointsTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = (string)value;
            var list = text?.Split(',')
                .Select(o => o.Trim())
                .Select(int.Parse)
                .ToList();

            if (list.Count != 3)
            {
                throw new ArgumentException($"'{value}' Invalid value. BreakPoints must contains 3 items.");
            }

            return new BreakPoints
            {
                ExtraSmallToSmallScreen = list[0],
                SmallToMediumScreen = list[1],
                MediumToLargeScreen = list[2] 
            };
        }
    }
}
