using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class ResponsivePanel : Panel
    {
        public static readonly DependencyProperty ActualColumnProperty =
            DependencyProperty.RegisterAttached("ActualColumn", 
                typeof(int), typeof(ResponsivePanel), new PropertyMetadata(0));

        public static readonly DependencyProperty ActualRowProperty =
            DependencyProperty.RegisterAttached("ActualRow",
                typeof(int), typeof(ResponsivePanel), new PropertyMetadata(0));

        public static readonly DependencyProperty BreakPointsProperty =
            DependencyProperty.Register(nameof(BreakPoints),
                typeof(BreakPoints), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ExtraNarrowStateOffsetProperty =
            DependencyProperty.RegisterAttached("ExtraNarrowStateOffset",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty ExtraNarrowStateProperty =
            DependencyProperty.RegisterAttached("ExtraNarrowState",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty ExtraNarrowStatePullProperty =
            DependencyProperty.RegisterAttached("ExtraNarrowStatePull",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty ExtraNarrowStatePushProperty =
            DependencyProperty.RegisterAttached("ExtraNarrowStatePush",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MaxDivisionProperty =
            DependencyProperty.Register("MaxDivision",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(12, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty MediumStatedPullProperty =
            DependencyProperty.RegisterAttached("MediumStatedPull",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MediumStateOffsetProperty =
            DependencyProperty.RegisterAttached("MediumStateOffset",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MediumStateProperty =
            DependencyProperty.RegisterAttached("MediumState",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty MediumStatePushProperty =
            DependencyProperty.RegisterAttached("MediumStatePush",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty NarrowStatePullProperty =
            DependencyProperty.RegisterAttached("NarrowStatePull",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty NarrowStatePushProperty =
            DependencyProperty.RegisterAttached("NarrowStatePush",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty NarrowStateOffsetProperty =
            DependencyProperty.RegisterAttached("NarrowStateOffset",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty NarrowStateProperty =
            DependencyProperty.RegisterAttached("NarrowState",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty WideStateOffsetProperty =
            DependencyProperty.RegisterAttached("WideStateOffset",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty WideStateProperty =
            DependencyProperty.RegisterAttached("WideState",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty WideStatePullProperty =
            DependencyProperty.RegisterAttached("WideStatePull",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty WideStatePushProperty =
            DependencyProperty.RegisterAttached("WideStatePush",
                typeof(int), typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public ResponsivePanel()
        {
            MaxDivision = 12;
            BreakPoints = new BreakPoints();
        }

        public BreakPoints BreakPoints
        {
            get => (BreakPoints)GetValue(BreakPointsProperty);
            set => SetValue(BreakPointsProperty, value);
        }

        public int MaxDivision
        {
            get => (int)GetValue(MaxDivisionProperty);
            set => SetValue(MaxDivisionProperty, value);
        }

        public static int GetActualColumn(DependencyObject obj)
        {
            return (int)obj.GetValue(ActualColumnProperty);
        }

        public static int GetActualRow(DependencyObject obj)
        {
            return (int)obj.GetValue(ActualRowProperty);
        }

        public static int GetExtraNarrowState(DependencyObject obj)
        {
            return (int)obj.GetValue(ExtraNarrowStateProperty);
        }

        public static int GetExtraNarrowStateOffset(DependencyObject obj)
        {
            return (int)obj.GetValue(ExtraNarrowStateOffsetProperty);
        }

        public static int GetExtraNarrowStatePull(DependencyObject obj)
        {
            return (int)obj.GetValue(ExtraNarrowStatePullProperty);
        }

        public static int GetExtraNarrowStatePush(DependencyObject obj)
        {
            return (int)obj.GetValue(ExtraNarrowStatePushProperty);
        }

        public static int GetMediumState(DependencyObject obj)
        {
            return (int)obj.GetValue(MediumStateProperty);
        }

        public static int GetMediumStatePull(DependencyObject obj)
        {
            return (int)obj.GetValue(MediumStatedPullProperty);
        }

        public static int GetMediumStateOffset(DependencyObject obj)
        {
            return (int)obj.GetValue(MediumStateOffsetProperty);
        }

        public static int GetMediumStatePush(DependencyObject obj)
        {
            return (int)obj.GetValue(MediumStatePushProperty);
        }

        public static int GetNarrowStatePull(DependencyObject obj)
        {
            return (int)obj.GetValue(NarrowStatePullProperty);
        }

        public static int GetNarrowStatePush(DependencyObject obj)
        {
            return (int)obj.GetValue(NarrowStatePushProperty);
        }

        public static int GetNarrowState(DependencyObject obj)
        {
            return (int)obj.GetValue(NarrowStateProperty);
        }

        public static int GetNarrowStateOffset(DependencyObject obj)
        {
            return (int)obj.GetValue(NarrowStateOffsetProperty);
        }

        public static int GetWideState(DependencyObject obj)
        {
            return (int)obj.GetValue(WideStateProperty);
        }

        public static int GetWideStateOffset(DependencyObject obj)
        {
            return (int)obj.GetValue(WideStateOffsetProperty);
        }

        public static int GetWideStatePull(DependencyObject obj)
        {
            return (int)obj.GetValue(WideStatePullProperty);
        }

        public static int GetWideStatePush(DependencyObject obj)
        {
            return (int)obj.GetValue(WideStatePushProperty);
        }
        public static void SetExtraNarrow(DependencyObject obj, int value)
        {
            obj.SetValue(ExtraNarrowStateProperty, value);
        }

        public static void SetExtraNarrowOffset(DependencyObject obj, int value)
        {
            obj.SetValue(ExtraNarrowStateOffsetProperty, value);
        }

        public static void SetExtraNarrowPull(DependencyObject obj, int value)
        {
            obj.SetValue(ExtraNarrowStatePullProperty, value);
        }

        public static void SetExtraStateNarrowPush(DependencyObject obj, int value)
        {
            obj.SetValue(ExtraNarrowStatePushProperty, value);
        }

        public static void SetMediumState(DependencyObject obj, int value)
        {
            obj.SetValue(MediumStateProperty, value);
        }

        public static void SetMediumStatedPull(DependencyObject obj, int value)
        {
            obj.SetValue(MediumStatedPullProperty, value);
        }

        public static void SetMediumStateOffset(DependencyObject obj, int value)
        {
            obj.SetValue(MediumStateOffsetProperty, value);
        }

        public static void SetMediumStatePush(DependencyObject obj, int value)
        {
            obj.SetValue(MediumStatePushProperty, value);
        }

        public static void SetNarrowPull(DependencyObject obj, int value)
        {
            obj.SetValue(NarrowStatePullProperty, value);
        }

        public static void SetNarrowPush(DependencyObject obj, int value)
        {
            obj.SetValue(NarrowStatePushProperty, value);
        }

        public static void SetNarrowState(DependencyObject obj, int value)
        {
            obj.SetValue(NarrowStateProperty, value);
        }

        public static void SetNarrowStateOffset(DependencyObject obj, int value)
        {
            obj.SetValue(NarrowStateOffsetProperty, value);
        }

        public static void SetWideState(DependencyObject obj, int value)
        {
            obj.SetValue(WideStateProperty, value);
        }

        public static void SetWideStateOffset(DependencyObject obj, int value)
        {
            obj.SetValue(WideStateOffsetProperty, value);
        }

        public static void SetWideStatePull(DependencyObject obj, int value)
        {
            obj.SetValue(WideStatePullProperty, value);
        }

        public static void SetWideStatePush(DependencyObject obj, int value)
        {
            obj.SetValue(WideStatePushProperty, value);
        }
        protected static void SetActualColumn(DependencyObject obj, int value)
        {
            obj.SetValue(ActualColumnProperty, value);
        }
        protected static void SetActualRow(DependencyObject obj, int value)
        {
            obj.SetValue(ActualRowProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var columnWidth = finalSize.Width / MaxDivision;

            var group = Children.OfType<UIElement>().GroupBy(GetActualRow);

            double temp = 0;
            foreach (var rows in group)
            {
                double max = 0;

                var columnHeight = rows.Max(o => o.DesiredSize.Height);
                foreach (var element in rows)
                {
                    var column = GetActualColumn(element);
                    var columnSpan = GetSpan(element, finalSize.Width);

                    var rect = new Rect(columnWidth * column, temp, columnWidth * columnSpan, columnHeight);

                    element.Arrange(rect);
                    max = Math.Max(element.DesiredSize.Height, max);
                }

                temp += max;
            }
            return base.ArrangeOverride(finalSize);
        }

        protected int GetOffset(UIElement element, double width)
        {
            int span;

            var getXs = new Func<UIElement, int>(o =>
            {
                var x = GetExtraNarrowStateOffset(o); return x != 0 ? x : 0;
            });

            var getSm = new Func<UIElement, int>(o =>
            {
                var x = GetNarrowStateOffset(o); return x != 0 ? x : getXs(o);
            });

            var getMd = new Func<UIElement, int>(o =>
            {
                var x = GetMediumStateOffset(o); return x != 0 ? x : getSm(o);
            });

            var getLg = new Func<UIElement, int>(o =>
            {
                var x = GetWideStateOffset(o); return x != 0 ? x : getMd(o);
            });

            if (width < BreakPoints.ExtraSmallToSmallScreen)
            {
                span = getXs(element);
            }
            else if (width < BreakPoints.SmallToMediumScreen)
            {
                span = getSm(element);
            }
            else if (width < BreakPoints.MediumToLargeScreen)
            {
                span = getMd(element);
            }
            else
            {
                span = getLg(element);
            }

            return Math.Min(Math.Max(0, span), MaxDivision);
        }

        protected int GetPull(UIElement element, double width)
        {
            int span;

            var getXs = new Func<UIElement, int>(o =>
            {
                var x = GetExtraNarrowStatePull(o); return x != 0 ? x : 0;
            });

            var getSm = new Func<UIElement, int>(o =>
            {
                var x = GetNarrowStatePull(o); return x != 0 ? x : getXs(o);
            });

            var getMd = new Func<UIElement, int>(o =>
            {
                var x = GetMediumStatePull(o); return x != 0 ? x : getSm(o);
            });

            var getLg = new Func<UIElement, int>(o =>
            {
                var x = GetWideStatePull(o); return x != 0 ? x : getMd(o);
            });

            if (width < BreakPoints.ExtraSmallToSmallScreen)
            {
                span = getXs(element);
            }
            else if (width < BreakPoints.SmallToMediumScreen)
            {
                span = getSm(element);
            }
            else if (width < BreakPoints.MediumToLargeScreen)
            {
                span = getMd(element);
            }
            else
            {
                span = getLg(element);
            }

            return Math.Min(Math.Max(0, span), MaxDivision);
        }

        protected int GetPush(UIElement element, double width)
        {
            int span;

            var getXs = new Func<UIElement, int>(o =>
            {
                var x = GetExtraNarrowStatePush(o); return x != 0 ? x : 0;
            });

            var getSm = new Func<UIElement, int>(o =>
            {
                var x = GetNarrowStatePush(o); return x != 0 ? x : getXs(o);
            });

            var getMd = new Func<UIElement, int>(o =>
            {
                var x = GetMediumStatePush(o); return x != 0 ? x : getSm(o);
            });

            var getLg = new Func<UIElement, int>(o =>
            {
                var x = GetWideStatePush(o); return x != 0 ? x : getMd(o);
            });

            if (width < BreakPoints.ExtraSmallToSmallScreen)
            {
                span = getXs(element);
            }
            else if (width < BreakPoints.SmallToMediumScreen)
            {
                span = getSm(element);
            }
            else if (width < BreakPoints.MediumToLargeScreen)
            {
                span = getMd(element);
            }
            else
            {
                span = getLg(element);
            }

            return Math.Min(Math.Max(0, span), MaxDivision);
        }

        protected int GetSpan(UIElement element, double width)
        {
            int span;

            var getXs = new Func<UIElement, int>(o =>
            {
                var x = GetExtraNarrowState(o); return x != 0 ? x : MaxDivision;
            });

            var getSm = new Func<UIElement, int>(o =>
            {
                var x = GetNarrowState(o); return x != 0 ? x : getXs(o);
            });

            var getMd = new Func<UIElement, int>(o =>
            {
                var x = GetMediumState(o); return x != 0 ? x : getSm(o);
            });

            var getLg = new Func<UIElement, int>(o =>
            {
                var x = GetWideState(o); return x != 0 ? x : getMd(o);
            });

            if (width < BreakPoints.ExtraSmallToSmallScreen)
            {
                span = getXs(element);
            }
            else if (width < BreakPoints.SmallToMediumScreen)
            {
                span = getSm(element);
            }
            else if (width < BreakPoints.MediumToLargeScreen)
            {
                span = getMd(element);
            }
            else
            {
                span = getLg(element);
            }

            return Math.Min(Math.Max(0, span), MaxDivision);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var count = 0;
            var currentRow = 0;

            var availableWidth = double.IsPositiveInfinity(availableSize.Width) ? double.PositiveInfinity : availableSize.Width / MaxDivision;

            foreach (UIElement child in Children)
            {
                if (child != null)
                {
                    if (child.Visibility == Visibility.Collapsed)
                    {
                        continue;
                    }

                    var span = GetSpan(child, availableSize.Width);
                    var offset = GetOffset(child, availableSize.Width);
                    var push = GetPush(child, availableSize.Width);
                    var pull = GetPull(child, availableSize.Width);

                    if (count + span + offset > MaxDivision)
                    {
                        currentRow++;
                        count = 0;
                    }

                    SetActualColumn(child, count + offset + push - pull);
                    SetActualRow(child, currentRow);

                    count += span + offset;

                    var size = new Size(availableWidth * span, double.PositiveInfinity);
                    child.Measure(size);
                }
            }

            var group = Children.OfType<UIElement>().GroupBy(GetActualRow).ToList();

            var totalSize = new Size();
            if (group.Count != 0)
            {
                totalSize.Width = group.Max(rows => rows.Sum(o => o.DesiredSize.Width));
                totalSize.Height = group.Sum(rows => rows.Max(o => o.DesiredSize.Height));
            }

            return totalSize;
        }
    }
}