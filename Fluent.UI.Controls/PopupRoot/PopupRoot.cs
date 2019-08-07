using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public class PopupRoot : Panel
    {
        public static readonly DependencyProperty BottomProperty =
            DependencyProperty.RegisterAttached("Bottom",
                typeof(double), typeof(PopupRoot),
                new FrameworkPropertyMetadata(double.NaN, OnPositioningChanged));

        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.RegisterAttached("Left",
                typeof(double), typeof(PopupRoot),
                new FrameworkPropertyMetadata(double.NaN, OnPositioningChanged));

        public static readonly DependencyProperty RightProperty = 
            DependencyProperty.RegisterAttached("Right",
                typeof(double), typeof(PopupRoot),
                    new FrameworkPropertyMetadata(double.NaN, OnPositioningChanged));

        public static readonly DependencyProperty TopProperty =
            DependencyProperty.RegisterAttached("Top",
                typeof(double), typeof(PopupRoot),
                new FrameworkPropertyMetadata(double.NaN, OnPositioningChanged));

        public static double GetBottom(UIElement element)
        {
            return (double)element.GetValue(BottomProperty);
        }

        public static double GetLeft(UIElement element)
        {
            return (double)element.GetValue(LeftProperty);
        }

        public static double GetRight(UIElement element)
        {
            return (double)element.GetValue(RightProperty);
        }

        public static double GetTop(UIElement element)
        {
            return (double)element.GetValue(TopProperty);
        }

        public static void SetBottom(UIElement element, double length)
        {
            element.SetValue(BottomProperty, length);
        }

        public static void SetLeft(UIElement element, double length)
        {
            element.SetValue(LeftProperty, length);
        }

        public static void SetRight(UIElement element, double length)
        {
            element.SetValue(RightProperty, length);
        }

        public static void SetTop(UIElement element, double length)
        {
            element.SetValue(TopProperty, length);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (FrameworkElement child in InternalChildren)
            {
                if (child == null)
                {
                    continue;
                }

                var x = 0d;
                var y = 0d;

                var left = GetLeft(child);
                var right = GetRight(child);

                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;

                if (!double.IsNaN(left))
                {
                    x = left;
                }
                else if (!double.IsNaN(right))
                {
                    x = arrangeSize.Width - child.DesiredSize.Width - right;
                }
                else
                {
                    if (child.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        width = ActualWidth;
                    }
                }

                var top = GetTop(child);
                var bottom = GetBottom(child);

                if (!double.IsNaN(top))
                {
                    y = top;
                }
                else if (!double.IsNaN(bottom))
                {
                    y = arrangeSize.Height - child.DesiredSize.Height - bottom;
                }
                else
                {
                    if (child.VerticalAlignment == VerticalAlignment.Stretch)
                    {
                        height = ActualHeight;
                    }
                }

                child.Arrange(new Rect(new Point(x, y), new Size(width, height)));
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var childConstraint = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            foreach (UIElement child in InternalChildren)
            {
                child?.Measure(childConstraint);
            }

            return new Size();
        }

        private static void OnPositioningChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is UIElement element)
            {
                var popupRoot = VisualTreeHelper.GetParent(element) as PopupRoot;
                popupRoot?.InvalidateArrange();
            }
        }
    }
}

