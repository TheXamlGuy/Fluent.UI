using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class RelativePanel : Panel
    {
        public static readonly DependencyProperty AboveProperty =
            DependencyProperty.RegisterAttached("Above", 
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignBottomWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignBottomWithPanel", 
                typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignBottomWithProperty =
            DependencyProperty.RegisterAttached("AlignBottomWith", 
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignHorizontalCenterWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignHorizontalCenterWithPanel",
                typeof(bool), typeof(RelativePanel), 
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignHorizontalCenterWithProperty =
            DependencyProperty.RegisterAttached("AlignHorizontalCenterWith", 
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignLeftWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignLeftWithPanel",
                typeof(bool), typeof(RelativePanel), 
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignLeftWithProperty =
            DependencyProperty.RegisterAttached("AlignLeftWith",
                typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignRightWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignRightWithPanel",
                typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignRightWithProperty =
            DependencyProperty.RegisterAttached("AlignRightWith",
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignTopWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignTopWithPanel", 
                typeof(bool), typeof(RelativePanel),
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignTopWithProperty =
            DependencyProperty.RegisterAttached("AlignTopWith",
                typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignVerticalCenterWithPanelProperty =
            DependencyProperty.RegisterAttached("AlignVerticalCenterWithPanel",
                typeof(bool), typeof(RelativePanel), 
                new PropertyMetadata(false, OnAlignPropertiesChanged));

        public static readonly DependencyProperty AlignVerticalCenterWithProperty =
            DependencyProperty.RegisterAttached("AlignVerticalCenterWith",
                typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty BelowProperty =
            DependencyProperty.RegisterAttached("Below",
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty LeftOfProperty =
            DependencyProperty.RegisterAttached("LeftOf", 
                typeof(object), typeof(RelativePanel),
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        public static readonly DependencyProperty RightOfProperty =
            DependencyProperty.RegisterAttached("RightOf",
                typeof(object), typeof(RelativePanel), 
                new PropertyMetadata(null, OnAlignPropertiesChanged));

        private static readonly DependencyProperty ArrangeStateProperty =
            DependencyProperty.Register("ArrangeState", 
                typeof(double[]), typeof(StateTrigger), new PropertyMetadata(null));

        public static object GetAbove(DependencyObject obj)
        {
            return obj.GetValue(AboveProperty);
        }

        public static object GetAlignBottomWith(DependencyObject obj)
        {
            return obj.GetValue(AlignBottomWithProperty);
        }

        public static bool GetAlignBottomWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignBottomWithPanelProperty);
        }

        public static object GetAlignHorizontalCenterWith(DependencyObject obj)
        {
            return obj.GetValue(AlignHorizontalCenterWithProperty);
        }

        public static bool GetAlignHorizontalCenterWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignHorizontalCenterWithPanelProperty);
        }

        public static object GetAlignLeftWith(DependencyObject obj)
        {
            return obj.GetValue(AlignLeftWithProperty);
        }

        public static bool GetAlignLeftWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignLeftWithPanelProperty);
        }

        public static object GetAlignRightWith(DependencyObject obj)
        {
            return obj.GetValue(AlignRightWithProperty);
        }

        public static bool GetAlignRightWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignRightWithPanelProperty);
        }

        public static object GetAlignTopWith(DependencyObject obj)
        {
            return obj.GetValue(AlignTopWithProperty);
        }

        public static bool GetAlignTopWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignTopWithPanelProperty);
        }

        public static object GetAlignVerticalCenterWith(DependencyObject obj)
        {
            return obj.GetValue(AlignVerticalCenterWithProperty);
        }

        public static bool GetAlignVerticalCenterWithPanel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AlignVerticalCenterWithPanelProperty);
        }

        public static object GetBelow(DependencyObject obj)
        {
            return obj.GetValue(BelowProperty);
        }

        public static object GetLeftOf(DependencyObject obj)
        {
            return obj.GetValue(LeftOfProperty);
        }

        public static object GetRightOf(DependencyObject obj)
        {
            return obj.GetValue(RightOfProperty);
        }

        public static void SetAbove(DependencyObject obj, object value)
        {
            obj.SetValue(AboveProperty, value);
        }

        public static void SetAlignBottomWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignBottomWithProperty, value);
        }

        public static void SetAlignBottomWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignBottomWithPanelProperty, value);
        }

        public static void SetAlignHorizontalCenterWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignHorizontalCenterWithProperty, value);
        }

        public static void SetAlignHorizontalCenterWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignHorizontalCenterWithPanelProperty, value);
        }

        public static void SetAlignLeftWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignLeftWithProperty, value);
        }

        public static void SetAlignLeftWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignLeftWithPanelProperty, value);
        }

        public static void SetAlignRightWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignRightWithProperty, value);
        }

        public static void SetAlignRightWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignRightWithPanelProperty, value);
        }

        public static void SetAlignTopWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignTopWithProperty, value);
        }

        public static void SetAlignTopWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignTopWithPanelProperty, value);
        }

        public static void SetAlignVerticalCenterWith(DependencyObject obj, object value)
        {
            obj.SetValue(AlignVerticalCenterWithProperty, value);
        }

        public static void SetAlignVerticalCenterWithPanel(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignVerticalCenterWithPanelProperty, value);
        }

        public static void SetBelow(DependencyObject obj, object value)
        {
            obj.SetValue(BelowProperty, value);
        }

        public static void SetLeftOf(DependencyObject obj, object value)
        {
            obj.SetValue(LeftOfProperty, value);
        }

        public static void SetRightOf(DependencyObject obj, object value)
        {
            obj.SetValue(RightOfProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var elements = new Dictionary<string, UIElement>();
            foreach (var child in Children.OfType<FrameworkElement>().Where(c => c.Name != null))
            {
                elements[child.Name] = child;
            }

            var arranges = new List<double[]>(Children.Count);

            var arrangedCount = 0;
            foreach (var child in Children.OfType<UIElement>())
            {
                var rect = new[] { double.NaN, double.NaN, double.NaN, double.NaN };
                arranges.Add(rect);
                child.SetValue(ArrangeStateProperty, rect);

                if (GetAlignLeftWithPanel(child))
                {
                    rect[0] = 0;
                }
                else if (child.GetValue(AlignLeftWithProperty) == null &&
                    child.GetValue(RightOfProperty) == null &&
                    child.GetValue(AlignHorizontalCenterWithProperty) == null &&
                    !GetAlignHorizontalCenterWithPanel(child))
                {
                    if (GetAlignRightWithPanel(child))
                    {
                        rect[0] = finalSize.Width - child.DesiredSize.Width;
                    }
                    else if (child.GetValue(AlignRightWithProperty) == null &&
                             child.GetValue(AlignHorizontalCenterWithProperty) == null)
                    {
                        rect[0] = 0;
                    }
                }

                if (GetAlignTopWithPanel(child))
                {
                    rect[1] = 0;
                }
                else if (child.GetValue(AlignTopWithProperty) == null &&
                    child.GetValue(BelowProperty) == null &&
                    child.GetValue(AlignVerticalCenterWithProperty) == null &&
                    !GetAlignVerticalCenterWithPanel(child))
                {
                    if (GetAlignBottomWithPanel(child))
                    {
                        rect[1] = finalSize.Height - child.DesiredSize.Height;
                    }
                    else if (child.GetValue(AlignBottomWithProperty) == null &&
                             child.GetValue(AlignVerticalCenterWithProperty) == null)
                    {
                        rect[1] = 0;
                    }
                }

                if (GetAlignRightWithPanel(child))
                {
                    rect[2] = 0;
                }
                else if (!double.IsNaN(rect[0]) &&
                 child.GetValue(AlignRightWithProperty) == null &&
                 child.GetValue(LeftOfProperty) == null &&
                 child.GetValue(AlignHorizontalCenterWithProperty) == null &&
                 !GetAlignHorizontalCenterWithPanel(child))
                {
                    rect[2] = finalSize.Width - rect[0] - child.DesiredSize.Width;
                }

                if (GetAlignBottomWithPanel(child))
                {
                    rect[3] = 0;
                }
                else if (!double.IsNaN(rect[1]) &&
                    (child.GetValue(AlignBottomWithProperty) == null &&
                    child.GetValue(AboveProperty) == null) &&
                    child.GetValue(AlignVerticalCenterWithProperty) == null &&
                    !GetAlignVerticalCenterWithPanel(child))
                {
                    rect[3] = finalSize.Height - rect[1] - child.DesiredSize.Height;
                }

                if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                    !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                    arrangedCount++;
            }

            int i;
            while (arrangedCount < Children.Count)
            {
                var lastArrangeCount = arrangedCount;
                i = 0;
                foreach (var child in Children.OfType<UIElement>())
                {
                    var rect = arranges[i++];

                    if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                        !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                        continue;

                    if (double.IsNaN(rect[0]))
                    {
                        var alignLeftWith = GetDependencyElement(AlignLeftWithProperty, child, elements);
                        if (alignLeftWith != null)
                        {
                            var r = (double[])alignLeftWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[0]))
                                rect[0] = r[0];
                        }
                        else
                        {
                            var rightOf = GetDependencyElement(RightOfProperty, child, elements);
                            if (rightOf != null)
                            {
                                var r = (double[])rightOf.GetValue(ArrangeStateProperty);
                                rect[0] = finalSize.Width - r[2];
                            }
                            else if (!double.IsNaN(rect[2]))
                            {
                                rect[0] = finalSize.Width - rect[2] - child.DesiredSize.Width;
                            }
                        }
                    }

                    if (double.IsNaN(rect[1]))
                    {
                        var alignTopWith = GetDependencyElement(AlignTopWithProperty, child, elements);
                        if (alignTopWith != null)
                        {
                            var r = (double[])alignTopWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[1]))
                                rect[1] = r[1];
                        }
                        else
                        {
                            var below = GetDependencyElement(BelowProperty, child, elements);
                            if (below != null)
                            {
                                var r = (double[])below.GetValue(ArrangeStateProperty);
                                rect[1] = finalSize.Height - r[3];
                            }
                            else if (!double.IsNaN(rect[3]))
                            {
                                rect[1] = finalSize.Height - rect[3] - child.DesiredSize.Height;
                            }
                        }
                    }

                    if (double.IsNaN(rect[2]))
                    {
                        var alignRightWith = GetDependencyElement(AlignRightWithProperty, child, elements);
                        if (alignRightWith != null)
                        {
                            var r = (double[])alignRightWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[2]))
                            {
                                rect[2] = r[2];
                                if (double.IsNaN(rect[0]))
                                {
                                    if (child.GetValue(AlignLeftWithProperty) == null)
                                    {
                                        rect[0] = rect[2] + child.DesiredSize.Width;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var leftOf = GetDependencyElement(LeftOfProperty, child, elements);
                            if (leftOf != null)
                            {
                                var r = (double[])leftOf.GetValue(ArrangeStateProperty);
                                rect[2] = finalSize.Width - r[0];
                            }
                            else if (!double.IsNaN(rect[0]))
                            {
                                rect[2] = finalSize.Width - rect[0] - child.DesiredSize.Width;
                            }
                        }
                    }

                    if (double.IsNaN(rect[3]))
                    {
                        var alignBottomWith = GetDependencyElement(AlignBottomWithProperty, child, elements);
                        if (alignBottomWith != null)
                        {
                            var r = (double[])alignBottomWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[3]))
                            {
                                rect[3] = r[3];
                                if (double.IsNaN(rect[1]))
                                {
                                    if (child.GetValue(AlignTopWithProperty) == null)
                                    {
                                        rect[1] = rect[3] + child.DesiredSize.Height;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var above = GetDependencyElement(AboveProperty, child, elements);
                            if (above != null)
                            {
                                var r = (double[])above.GetValue(ArrangeStateProperty);
                                rect[3] = finalSize.Height - r[1];
                            }
                            else if (!double.IsNaN(rect[1]))
                            {
                                rect[3] = finalSize.Height - rect[1] - child.DesiredSize.Height;
                            }
                        }
                    }

                    if (double.IsNaN(rect[0]) && double.IsNaN(rect[2]))
                    {
                        var alignHorizontalCenterWith = GetDependencyElement(AlignHorizontalCenterWithProperty, child, elements);
                        if (alignHorizontalCenterWith != null)
                        {
                            var r = (double[])alignHorizontalCenterWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[0]) && !double.IsNaN(r[2]))
                            {
                                rect[0] = r[0] + (finalSize.Width - r[2] - r[0]) * .5 - child.DesiredSize.Width * .5;
                                rect[2] = finalSize.Width - rect[0] - child.DesiredSize.Width;
                            }
                        }
                        else
                        {
                            if (GetAlignHorizontalCenterWithPanel(child))
                            {
                                var roomToSpare = finalSize.Width - child.DesiredSize.Width;
                                rect[0] = roomToSpare * .5;
                                rect[2] = roomToSpare * .5;
                            }
                        }
                    }

                    if (double.IsNaN(rect[1]) && double.IsNaN(rect[3]))
                    {
                        var alignVerticalCenterWith = GetDependencyElement(AlignVerticalCenterWithProperty, child, elements);
                        if (alignVerticalCenterWith != null)
                        {
                            double[] r = (double[])alignVerticalCenterWith.GetValue(ArrangeStateProperty);
                            if (!double.IsNaN(r[1]) && !double.IsNaN(r[3]))
                            {
                                rect[1] = r[1] + (finalSize.Height - r[3] - r[1]) * .5 - child.DesiredSize.Height * .5;
                                rect[3] = finalSize.Height - rect[1] - child.DesiredSize.Height;
                            }
                        }
                        else
                        {
                            if (GetAlignVerticalCenterWithPanel(child))
                            {
                                var roomToSpare = finalSize.Height - child.DesiredSize.Height;
                                rect[1] = roomToSpare * .5;
                                rect[3] = roomToSpare * .5;
                            }
                        }
                    }


                    if (!double.IsNaN(rect[0]) && !double.IsNaN(rect[1]) &&
                        !double.IsNaN(rect[2]) && !double.IsNaN(rect[3]))
                        arrangedCount++;
                }
                if (lastArrangeCount == arrangedCount)
                {
                    throw new ArgumentException("RelativePanel error: Circular dependency detected. Layout could not complete");
                }
            }

            i = 0;

            foreach (var child in Children.OfType<UIElement>())
            {
                var rect = arranges[i++];
                child.Arrange(new Rect(rect[0], rect[1], Math.Max(0, finalSize.Width - rect[2] - rect[0]), Math.Max(0, finalSize.Height - rect[3] - rect[1])));
            }
            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in Children.OfType<FrameworkElement>())
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        private static void OnAlignPropertiesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is FrameworkElement elm && elm.Parent is FrameworkElement element)
                element.InvalidateArrange();
        }

        private UIElement GetDependencyElement(DependencyProperty property, DependencyObject child, Dictionary<string, UIElement> elements)
        {
            var dependency = child.GetValue(property);
            if (dependency == null)
            {
                return null;
            }

            if (dependency is string name)
            {
                if (!elements.ContainsKey(name))
                {
                    throw new ArgumentException($"RelativePanel error: The name '{name}' does not exist in the current context");
                }

                return elements[name];
            }

            if (dependency is UIElement element)
            {
                if (Children.Contains(element))
                {
                    return element;
                }

                throw new ArgumentException("RelativePanel error: Element does not exist in the current context");
            }

            throw new ArgumentException("RelativePanel error: Value must be of type UIElement");
        }
    }
}