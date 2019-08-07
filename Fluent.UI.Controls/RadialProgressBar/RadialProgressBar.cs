using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public sealed class RadialProgressBar : ProgressBar
    {
        public static readonly DependencyProperty OutlineProperty =
            DependencyProperty.Register(nameof(Outline),
                typeof(Brush), typeof(RadialProgressBar),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(nameof(Thickness),
                typeof(double), typeof(RadialProgressBar),
                new PropertyMetadata(0.0, ThicknessChangedHandler));

        private bool _allTemplatePartsDefined;
        private ArcSegment _barArc;
        private PathFigure _barFigure;
        private ArcSegment _outlineArc;
        private PathFigure _outlineFigure;

        public RadialProgressBar()
        {
            DefaultStyleKey = typeof(RadialProgressBar);
            SizeChanged += SizeChangedHandler;
        }

        public Brush Outline
        {
            get => (Brush)GetValue(OutlineProperty);
            set => SetValue(OutlineProperty, value);
        }

        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _outlineFigure = GetTemplateChild("OutlineFigure") as PathFigure;
            _outlineArc = GetTemplateChild("OutlineArc") as ArcSegment;
            _barFigure = GetTemplateChild("BarFigure") as PathFigure;
            _barArc = GetTemplateChild("BarArc") as ArcSegment;

            _allTemplatePartsDefined = _outlineFigure != null && _outlineArc != null && _barFigure != null && _barArc != null;

            RenderAll();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            RenderSegment();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            RenderSegment();
        }
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            RenderSegment();
        }
        private static void ThicknessChangedHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as RadialProgressBar;
            sender?.RenderAll();
        }

        private Size ComputeEllipseSize()
        {
            var safeThickness = Math.Max(Thickness, 0.0);
            var width = Math.Max((ActualWidth - safeThickness) / 2.0, 0.0);
            var height = Math.Max((ActualHeight - safeThickness) / 2.0, 0.0);
            return new Size(width, height);
        }

        private double ComputeNormalizedRange()
        {
            var range = Maximum - Minimum;
            var delta = Value - Minimum;
            var output = range.Equals(0.0) ? 0.0 : delta / range;
            output = Math.Min(Math.Max(0.0, output), 0.9999);
            return output;
        }

        private void RenderAll()
        {
            if (!_allTemplatePartsDefined)
            {
                return;
            }

            var size = ComputeEllipseSize();
            var segmentWidth = size.Width;
            var translationFactor = Math.Max(Thickness / 2.0, 0.0);

            _outlineFigure.StartPoint = _barFigure.StartPoint = new Point(segmentWidth + translationFactor, translationFactor);
            _outlineArc.Size = _barArc.Size = new Size(segmentWidth, size.Height);
            _outlineArc.Point = new Point(segmentWidth + translationFactor - 0.05, translationFactor);

            RenderSegment();
        }

        private void RenderSegment()
        {
            if (!_allTemplatePartsDefined)
            {
                return;
            }

            var normalizedRange = ComputeNormalizedRange();

            var angle = 2 * Math.PI * normalizedRange;
            var size = ComputeEllipseSize();
            var translationFactor = Math.Max(Thickness / 2.0, 0.0);

            var x = (Math.Sin(angle) * size.Width) + size.Width + translationFactor;
            var y = (((Math.Cos(angle) * size.Height) - size.Height) * -1) + translationFactor;

            _barArc.IsLargeArc = angle >= Math.PI;
            _barArc.Point = new Point(x, y);
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            var self = sender as RadialProgressBar;
            self?.RenderAll();
        }
    }
}