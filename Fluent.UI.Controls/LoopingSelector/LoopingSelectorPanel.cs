using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Controls
{
    public class LoopingSelectorPanel : Panel
    {
        private readonly TimeSpan _animationDuration = TimeSpan.FromMilliseconds(200);
        private readonly Slider _sliderVertical;
        private double _itemHeight = 1d;
        private double _offsetSeparator;
        private bool _templateApplied;

        public LoopingSelectorPanel()
        {
         //   PreviewMouseUp += OnMouseUp;

            _sliderVertical = new Slider
            {
                SmallChange = 0.0000000001,
                Minimum = double.MinValue,
                Maximum = double.MaxValue,
                TickFrequency = 0.0000000001
            };

            _sliderVertical.ValueChanged += OnVerticalOffsetChanged;
        }

        internal void ScrollToSelectedIndex(LoopingSelectorItem selectedItem, bool useTransitions)
        {
            if (!_templateApplied)
            {
                return;
            }

            var centerTopOffset = ActualHeight / 2d - _itemHeight / 2d;
            var deltaOffset = centerTopOffset - selectedItem.GetVerticalPosition();

            if (double.IsInfinity(deltaOffset) || double.IsNaN(deltaOffset))
            {
                return;
            }

            var from = selectedItem.GetTranslateTransform().Y;
            //if (useTransitions)
            //{
                UpdatePositionsWithAnimation(from, from + deltaOffset);
           // }
            //else
            //{
            //    _sliderVertical.Value = deltaOffset;
            //}
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _templateApplied = false;

            Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, finalSize.Width, finalSize.Height)
            };

            var itemsHeight = Children.Cast<LoopingSelectorItem>()
                .Where(item => item != null)
                .Select(item => item.DesiredSize)
                .Select(desiredSize => desiredSize.Height).Sum();

            var positionTop = itemsHeight > finalSize.Height ? 0d : finalSize.Height / 2d;
            foreach (LoopingSelectorItem item in Children)
            {
                if (item == null)
                {
                    continue;
                }

                var desiredSize = item.DesiredSize;
                if (double.IsNaN(desiredSize.Width) || double.IsNaN(desiredSize.Height))
                {
                    continue;
                }

                item.RectPosition = new Rect(0, positionTop, finalSize.Width, desiredSize.Height);
                item.Arrange(item.RectPosition);

                var compositeTransform = new TranslateTransform();
                item.RenderTransform = compositeTransform;
                positionTop += desiredSize.Height;
            }

            _templateApplied = true;
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);
            Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, size.Width, size.Height)
            };

            foreach (UIElement container in Children)
            {
                container.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                if (!_itemHeight.Equals(container.DesiredSize.Height))
                {
                    _itemHeight = container.DesiredSize.Height;
                }

            }
            return size;
        }

        private int GetItemsCount()
        {
            return Children.Count;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs args)
        {
            if (!_templateApplied)
            {
                return;
            }

            if (Children.Count == 0)
            {
                return;
            }

            var positionY = args.GetPosition(this).Y;
            foreach (UIElement child in Children)
            {
                var rect = child.TransformToVisual(this).TransformBounds(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                if (!(positionY >= rect.Y) || !(positionY <= rect.Y + rect.Height))
                {
                    continue;
                }

                ScrollToSelectedIndex(child, rect);
                break;
            }
        }

        private void OnVerticalOffsetChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePositions(e.NewValue - e.OldValue);
        }

        private void ScrollToSelectedIndex(UIElement selectedItem, Rect rect)
        {
            if (!_templateApplied)
            {
                return;
            }

            var compositeTransform = (TranslateTransform) selectedItem.RenderTransform;
            if (compositeTransform == null)
            {
                return;
            }

            var centerTopOffset = ActualHeight / 2d - _itemHeight / 2d;
            var deltaOffset = centerTopOffset - rect.Y;

            UpdatePositionsWithAnimation(compositeTransform.Y, compositeTransform.Y + deltaOffset);
        }

        private void UpdatePosition(int startIndex, int endIndex, double offset)
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                var loopListItem = Children[i] as LoopingSelectorItem;
                var compositeTransform = loopListItem?.GetTranslateTransform();
                if (compositeTransform == null)
                {
                    continue;
                }

                compositeTransform.Y = offset;
            }
        }

        private void UpdatePositions(double offsetDelta)
        {
            var maxLogicalHeight = GetItemsCount() * _itemHeight;

            _offsetSeparator = (_offsetSeparator + offsetDelta) % maxLogicalHeight;

            var itemNumberSeparator = (int) (Math.Abs(_offsetSeparator) / _itemHeight);

            int itemIndexChanging;

            double offsetAfter;
            double offsetBefore;

            if (_offsetSeparator > 0)
            {
                itemIndexChanging = GetItemsCount() - itemNumberSeparator - 1;

                offsetAfter = _offsetSeparator;
                if ((_offsetSeparator % maxLogicalHeight).Equals(0))
                {
                    itemIndexChanging++;
                }

                offsetBefore = offsetAfter - maxLogicalHeight;
            }
            else
            {
                itemIndexChanging = itemNumberSeparator;
                offsetBefore = _offsetSeparator;
                offsetAfter = maxLogicalHeight + offsetBefore;
            }

            UpdatePosition(itemIndexChanging, GetItemsCount(), offsetBefore);
            UpdatePosition(0, itemIndexChanging, offsetAfter);
        }

        private void UpdatePositionsWithAnimation(double fromOffset, double toOffset)
        {
            var storyboard = new Storyboard();
            var animationSnap = new DoubleAnimation
            {
                From = fromOffset,
                To = toOffset,
                Duration = _animationDuration,
                EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut}
            };

            storyboard.Children.Add(animationSnap);

            Storyboard.SetTarget(animationSnap, _sliderVertical);
            Storyboard.SetTargetProperty(animationSnap, new PropertyPath("Value"));

            _sliderVertical.ValueChanged -= OnVerticalOffsetChanged;
            _sliderVertical.Value = fromOffset;
            _sliderVertical.ValueChanged += OnVerticalOffsetChanged;

            storyboard.Begin();
        }
    }
}
