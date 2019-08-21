using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Core
{
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From),
                typeof(GridLength), typeof(GridLengthAnimation));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(nameof(To),
                typeof(GridLength), typeof(GridLengthAnimation));

        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public override Type TargetPropertyType => typeof(GridLength);

        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            var from = (GridLength)GetValue(FromProperty);
            var to = (GridLength)GetValue(ToProperty);

            if (from.GridUnitType != to.GridUnitType)
            {
                return to;
            }

            var fromValue = from.Value;
            var toValue = to.Value;

            return fromValue > toValue ? new GridLength((1 - animationClock.CurrentProgress.Value) * (fromValue - toValue) + toValue, GridUnitType.Star) : new GridLength(animationClock.CurrentProgress.Value * (toValue - fromValue) + fromValue, GridUnitType.Star);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }
    }
}