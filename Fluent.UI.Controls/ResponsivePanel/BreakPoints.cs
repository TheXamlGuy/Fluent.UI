using System.ComponentModel;
using System.Windows;

namespace Fluent.UI.Controls
{
    [TypeConverter(typeof(BreakPointsTypeConverter))]
    public class BreakPoints : DependencyObject
    {
        public static readonly DependencyProperty MediumToLargeScreenProperty =
            DependencyProperty.Register(nameof(MediumToLargeScreen),
                typeof(double), typeof(BreakPoints),
                new PropertyMetadata(1200.0));

        public static readonly DependencyProperty SmallToMediumScreenProperty =
            DependencyProperty.Register(nameof(SmallToMediumScreen), 
                typeof(double), typeof(BreakPoints),
                new PropertyMetadata(992.0));

        public static readonly DependencyProperty ExtraSmallToSmallScreenProperty =
            DependencyProperty.Register(nameof(ExtraSmallToSmallScreen), 
                typeof(double), typeof(BreakPoints),
                new PropertyMetadata(768.0));

        public double MediumToLargeScreen
        {
            get => (double)GetValue(MediumToLargeScreenProperty);
            set => SetValue(MediumToLargeScreenProperty, value);
        }

        public double SmallToMediumScreen
        {
            get => (double)GetValue(SmallToMediumScreenProperty);
            set => SetValue(SmallToMediumScreenProperty, value);
        }

        public double ExtraSmallToSmallScreen
        {
            get => (double)GetValue(ExtraSmallToSmallScreenProperty);
            set => SetValue(ExtraSmallToSmallScreenProperty, value);
        }
    }
}