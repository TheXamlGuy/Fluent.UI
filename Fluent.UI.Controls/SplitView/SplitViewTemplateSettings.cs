using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using Fluent.UI.Controls.Animation;

namespace Fluent.UI.Controls
{
    public class SplitViewTemplateSettings : DependencyObject
    {
        internal static readonly DependencyProperty CompactPaneGridLengthProperty =
            DependencyProperty.Register(nameof(CompactPaneGridLength),
                typeof(GridLength), typeof(SplitViewTemplateSettings),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty NegativeOpenPaneLengthMinusCompactLengthProperty =
            DependencyProperty.Register(nameof(NegativeOpenPaneLengthMinusCompactLength),
                typeof(double), typeof(SplitViewTemplateSettings),
                new PropertyMetadata(0d));

        internal static readonly DependencyProperty NegativeOpenPaneLengthProperty =
            DependencyProperty.Register(nameof(NegativeOpenPaneLength), 
                typeof(double), typeof(SplitViewTemplateSettings),
                new PropertyMetadata(0d));

        internal static readonly DependencyProperty OpenPaneGridLengthProperty =
            DependencyProperty.Register(nameof(OpenPaneGridLength), 
                typeof(GridLength), typeof(SplitViewTemplateSettings),
                new PropertyMetadata(null));

        internal static readonly DependencyProperty OpenPaneLengthMinusCompactLengthProperty =
            DependencyProperty.Register(nameof(OpenPaneLengthMinusCompactLength),
                typeof(double),typeof(SplitViewTemplateSettings), 
                new PropertyMetadata(0d));

        internal static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register(nameof(OpenPaneLength), 
                typeof(double), typeof(SplitViewTemplateSettings), new PropertyMetadata(0d));

        internal SplitViewTemplateSettings(SplitView splitView)
        {
            Owner = splitView;
            Update();
        }

        public GridLength CompactPaneGridLength
        {
            get => (GridLength)GetValue(CompactPaneGridLengthProperty);
            private set => SetValue(CompactPaneGridLengthProperty, value);
        }

        public double NegativeOpenPaneLength
        {
            get => (double)GetValue(NegativeOpenPaneLengthProperty);
            private set => SetValue(NegativeOpenPaneLengthProperty, value);
        }

        public double NegativeOpenPaneLengthMinusCompactLength
        {
            get => (double)GetValue(NegativeOpenPaneLengthMinusCompactLengthProperty);
            set => SetValue(NegativeOpenPaneLengthMinusCompactLengthProperty, value);
        }

        public GridLength OpenPaneGridLength
        {
            get => (GridLength)GetValue(OpenPaneGridLengthProperty);
            private set => SetValue(OpenPaneGridLengthProperty, value);
        }

        public double OpenPaneLength
        {
            get => (double)GetValue(OpenPaneLengthProperty);
            private set => SetValue(OpenPaneLengthProperty, value);
        }

        public double OpenPaneLengthMinusCompactLength
        {
            get => (double)GetValue(OpenPaneLengthMinusCompactLengthProperty);
            private set => SetValue(OpenPaneLengthMinusCompactLengthProperty, value);
        }

        internal SplitView Owner { get; }

        internal void Update()
        {
            CompactPaneGridLength = new GridLength(Owner.CompactPaneLength, GridUnitType.Pixel);
            OpenPaneGridLength = new GridLength(Owner.OpenPaneLength, GridUnitType.Pixel);

            OpenPaneLength = Owner.OpenPaneLength;
            OpenPaneLengthMinusCompactLength = Owner.OpenPaneLength - Owner.CompactPaneLength;

            NegativeOpenPaneLength = -OpenPaneLength;
            NegativeOpenPaneLengthMinusCompactLength = -OpenPaneLengthMinusCompactLength;
        }

        internal void SetBindings()
        {
            if (Owner.InternalGetTemplateChild("ClosedToOpenOverlayLeftPaneTransformXAnimation") is DiscreteDoubleKeyFrame closedToOpenOverlayLeftPaneTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(closedToOpenOverlayLeftPaneTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedToOpenOverlayLeftPaneClipRectangleTransformXAnimation") is DiscreteDoubleKeyFrame closedToOpenOverlayLeftPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(closedToOpenOverlayLeftPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedToOpenOverlayRightPaneTransformXAnimation") is DiscreteDoubleKeyFrame closedToOpenOverlayRightPaneTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(closedToOpenOverlayRightPaneTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedToOpenOverlayRightPaneClipRectangleTransformXAnimation") is DiscreteDoubleKeyFrame closedToOpenOverlayRightPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(closedToOpenOverlayRightPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactLeftToOpenCompactOverlayLeftColumnDefinition1WidthAnimation") is GridLengthAnimation closedCompactLeftToOpenCompactOverlayLeftColumnDefinition1WidthAnimation)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactLeftToOpenCompactOverlayLeftColumnDefinition1WidthAnimation,
                    GridLengthAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactLeftToOpenCompactOverlayLeftPaneClipRectangleTransformXAnimation") is DiscreteDoubleKeyFrame closedCompactLeftToOpenCompactOverlayLeftPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactLeftToOpenCompactOverlayLeftPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactRightToOpenCompactOverlayRightColumnDefinition2WidthAnimation") is GridLengthAnimation closedCompactRightToOpenCompactOverlayRightColumnDefinition2WidthAnimation)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(
                    closedCompactRightToOpenCompactOverlayRightColumnDefinition2WidthAnimation,
                    GridLengthAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactRightToOpenCompactOverlayRightPaneClipRectangleTransformXAnimation") is DiscreteDoubleKeyFrame closedCompactRightToOpenCompactOverlayRightPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(
                    closedCompactRightToOpenCompactOverlayRightPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenOverlayLeftToClosedPanePaneTransformXAnimation") is SplineDoubleKeyFrame openOverlayLeftToClosedPanePaneTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(openOverlayLeftToClosedPanePaneTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenOverlayLeftToClosedPaneClipRectangleTransformXAnimation") is SplineDoubleKeyFrame openOverlayLeftToClosedPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(openOverlayLeftToClosedPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenOverlayRightToClosedPanePaneTransformXAnimation") is SplineDoubleKeyFrame openOverlayRightToClosedPanePaneTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(openOverlayRightToClosedPanePaneTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenOverlayRightToClosedPaneClipRectangleTransformXAnimation") is SplineDoubleKeyFrame openOverlayRightToClosedPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLength") { Source = Owner };
                BindingOperations.SetBinding(openOverlayRightToClosedPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayRightToClosedCompactRightColumnDefinition2WidthAnimation") is GridLengthAnimation openCompactOverlayRightToClosedCompactRightColumnDefinition2WidthAnimation)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(
                    openCompactOverlayRightToClosedCompactRightColumnDefinition2WidthAnimation,
                    GridLengthAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayLeftToClosedCompactLeftColumnDefinition1Width") is GridLengthAnimation openCompactOverlayLeftToClosedCompactLeftColumnDefinition1Width)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(openCompactOverlayLeftToClosedCompactLeftColumnDefinition1Width,
                    GridLengthAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayLeftToClosedCompactLeftPaneClipRectangleTransformXAnimation") is SplineDoubleKeyFrame openCompactOverlayLeftToClosedCompactLeftPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(openCompactOverlayLeftToClosedCompactLeftPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayRightToClosedCompactRightPaneClipRectangleTransformXAnimation") is SplineDoubleKeyFrame openCompactOverlayRightToClosedCompactRightPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(openCompactOverlayRightToClosedCompactRightPaneClipRectangleTransformXAnimation,
                    DoubleKeyFrame.ValueProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactLeftColumnDefinition1Width") is GridLengthAnimation closedCompactLeftColumnDefinition1Width)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactLeftColumnDefinition1Width, GridLengthAnimation.ToProperty,
                    binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactLeftPaneClipRectangleTransformXAnimation") is DoubleAnimation closedCompactLeftPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.NegativeOpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactLeftPaneClipRectangleTransformXAnimation,
                    DoubleAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactRightColumnDefinition2Width") is GridLengthAnimation closedCompactRightColumnDefinition2Width)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactRightColumnDefinition2Width, GridLengthAnimation.ToProperty,
                    binding);
            }

            if (Owner.InternalGetTemplateChild("ClosedCompactRightPaneClipRectangleTransformXAnimation") is DoubleAnimation closedCompactRightPaneClipRectangleTransformXAnimation)
            {
                var binding = new Binding("TemplateSettings.OpenPaneLengthMinusCompactLength") { Source = Owner };
                BindingOperations.SetBinding(closedCompactRightPaneClipRectangleTransformXAnimation,
                    DoubleAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenInlineRightColumnDefinition2Width") is GridLengthAnimation openInlineRightColumnDefinition2Width)
            {
                var binding = new Binding("TemplateSettings.OpenPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(openInlineRightColumnDefinition2Width, GridLengthAnimation.ToProperty,
                    binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayLeftColumnDefinition1Width") is GridLengthAnimation openCompactOverlayLeftColumnDefinition1Width)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(openCompactOverlayLeftColumnDefinition1Width,
                    GridLengthAnimation.ToProperty, binding);
            }

            if (Owner.InternalGetTemplateChild("OpenCompactOverlayRightColumnDefinition2Width") is GridLengthAnimation openCompactOverlayRightColumnDefinition2Width)
            {
                var binding = new Binding("TemplateSettings.CompactPaneGridLength") { Source = Owner };
                BindingOperations.SetBinding(openCompactOverlayRightColumnDefinition2Width,
                    GridLengthAnimation.ToProperty, binding);
            }
        }

    }
}