using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class FlyoutBase : DependencyObject
    {
        public static DependencyProperty AttachedFlyoutProperty =
            DependencyProperty.RegisterAttached("AttachedFlyout",
                typeof(FlyoutBase), typeof(FlyoutBase),
                new PropertyMetadata(null));

        public static DependencyProperty PlacementProperty =
            DependencyProperty.Register(nameof(Placement),
                typeof(FlyoutPlacementMode), typeof(FlyoutBase),
                new PropertyMetadata(FlyoutPlacementMode.Top));

        private Grid _dismissLayer;
        private Control _flyoutPresenter;
        private bool _isFlyoutOpen;
        private bool _isFlyoutPrepared;
        private FlyoutPlacementMode _perferredPlacement;
        private Point _offSet;
        public event EventHandler<object> Closed;
        public event TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs> Closing;
        public event EventHandler<object> Opened;
        public event EventHandler<object> Opening;

        public FlyoutPlacementMode Placement
        {
            get => (FlyoutPlacementMode)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public FrameworkElement Target { get; set; }

        public static FlyoutBase GetAttachedFlyout(FrameworkElement element)
        {
            return (FlyoutBase)element.GetValue(AttachedFlyoutProperty);
        }

        public static void SetAttachedFlyout(FrameworkElement element, FlyoutBase value)
        {
            element.SetValue(AttachedFlyoutProperty, value);
        }

        public static void ShowAttachedFlyout(FrameworkElement flyoutOwner, Point offset)
        {
            var flyoutBase = GetAttachedFlyout(flyoutOwner);
            flyoutBase?.ShowAt(flyoutOwner, offset);
        }

        public void Hide()
        {
            CloseFlyout();
        }

        public void ShowAt(FrameworkElement placementTarget, Point offset)
        {
            Target = placementTarget;
            _offSet = offset;
            OpenFlyout();
        }

        internal void CloseFlyout()
        {
            if (!_isFlyoutOpen)
            {
                return;
            }

            if (Closing != null)
            {
                var args = new FlyoutBaseClosingEventArgs();
                foreach (var eventDelegate in Closing.GetInvocationList())
                {
                    var handler = (TypedEventHandler<FlyoutBase, FlyoutBaseClosingEventArgs>)eventDelegate;
                    handler(this, args);
                    if (args.Cancel)
                    {
                        return;
                    }
                }
            }

            RemoveFlyout();
        }

        protected virtual Control CreatePresenter()
        {
            return null;
        }

        private void ClearPopupRoot()
        {
            VisualStateManager.GoToState(_flyoutPresenter, "Closed", true);

            ApplicationView.Current.PopupRoot.Children.Remove(_flyoutPresenter);
            ApplicationView.Current.PopupRoot.Children.Remove(_dismissLayer);

            _dismissLayer.MouseDown -= OnDismissLayerMouseDown;
            _dismissLayer = null;
        }

        private void OnApplicationViewDeactivated(object sender, EventArgs eventArgs)
        {
            CloseFlyout();
        }

        private void OnDismissLayerMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            CloseFlyout();
        }

        private void OpenFlyout()
        {
            Opening?.Invoke(this, Target);
            SetFlyout();
        }

        private void PrepareApplicationView()
        {
            ApplicationView.Current.Deactivated -= OnApplicationViewDeactivated;
            ApplicationView.Current.Deactivated += OnApplicationViewDeactivated;
        }

        private void PrepareFlyout()
        {
            if (_isFlyoutPrepared)
            {
                return;
            }

            PrepareApplicationView();
            PrepareFlyoutPresenter();

            _isFlyoutPrepared = true;
        }

        private void PrepareFlyoutPresenter()
        {
            _flyoutPresenter = CreatePresenter();
        }

        private void PreparePopupRoot()
        {
            _dismissLayer = new Grid
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };

            _dismissLayer.MouseDown -= OnDismissLayerMouseDown;
            _dismissLayer.MouseDown += OnDismissLayerMouseDown;

            ApplicationView.Current.PopupRoot.Children.Add(_dismissLayer);
            ApplicationView.Current.PopupRoot.Children.Add(_flyoutPresenter);

            UpdateFlyoutPosition();
        }

        private void RemoveFlyout()
        {
            ClearPopupRoot();
            Closed?.Invoke(this, Target);

            _isFlyoutOpen = false;
        }

        private void SetBottomPlacement(Point point)
        {
            double perferredTop;
            double perferredLeft;

            if (point.Y + _flyoutPresenter.DesiredSize.Height > ApplicationView.Current.ActualHeight)
            {
                perferredTop = point.Y - _flyoutPresenter.DesiredSize.Height;
            }
            else
            {
                perferredTop = point.Y + Target.DesiredSize.Height;
            }

            if (point.X + _flyoutPresenter.DesiredSize.Width > ApplicationView.Current.ActualWidth)
            {
                perferredLeft = point.X;
            }
            else
            {
                perferredLeft = point.X;
            }

            SetPreferredPlacement(perferredTop, perferredLeft, _perferredPlacement);
        }

        private void SetFlyout()
        {
            PrepareFlyout();
            PreparePopupRoot();

            Opened?.Invoke(this, Target);

            _isFlyoutOpen = true;
        }

        private void SetFullPlacement()
        {
            PopupRoot.SetLeft(_flyoutPresenter, double.NaN);
            PopupRoot.SetTop(_flyoutPresenter, double.NaN);
        }

        private void SetTopPlacement(Point point)
        {
            double perferredTop;
            var perferredLeft = 0d;
            FlyoutPlacementMode perferredPlacement;

            var targetWidth = Target.ActualWidth;
            var flyoutWidth = _flyoutPresenter.DesiredSize.Width;
            var applicationWidth = ApplicationView.Current.PopupRoot.ActualWidth;

            var left = point.X;
            double centerLeft;

            if (targetWidth > flyoutWidth)
            {
                centerLeft = left + targetWidth / 2 - flyoutWidth / 2;
            }
            else
            {
                centerLeft = left - flyoutWidth / 2 + targetWidth / 2;
            }

            if (_flyoutPresenter.DesiredSize.Height > point.Y)
            {
                perferredTop = point.Y + Target.DesiredSize.Height;
                perferredPlacement = FlyoutPlacementMode.Bottom;
            }
            else
            {
                perferredTop = point.Y - _flyoutPresenter.DesiredSize.Height;
                perferredPlacement = FlyoutPlacementMode.Top;
            }

            if (centerLeft > 0 && centerLeft + flyoutWidth < applicationWidth)
            {
                perferredLeft = centerLeft;
            }
            else if (left > 0 && left + flyoutWidth < applicationWidth)
            {
                perferredLeft = left;
            }
            else if (left - flyoutWidth + targetWidth > 0)
            {
                perferredLeft = left - flyoutWidth + targetWidth;
            }

            SetPreferredPlacement(perferredTop, perferredLeft, perferredPlacement);
        }

        private void SetPreferredPlacement(double top, double left, FlyoutPlacementMode placement)
        {
            PopupRoot.SetTop(_flyoutPresenter, top);
            PopupRoot.SetLeft(_flyoutPresenter, left);

            var placementState = string.Empty;
            switch (placement)
            {
                case FlyoutPlacementMode.Top:
                    placementState = "OpeningTopPlacement";
                    break;
                case FlyoutPlacementMode.Bottom:
                    placementState = "OpeningBottomPlacement";
                    break;
                case FlyoutPlacementMode.Left:
                    break;
                case FlyoutPlacementMode.Right:
                    break;
                case FlyoutPlacementMode.Full:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }

            VisualStateManager.GoToState(_flyoutPresenter, placementState, true);
        }

        private void UpdateFlyoutPosition()
        {
            _flyoutPresenter.UpdateLayout();

            var relativePoint = Target.TransformToAncestor(ApplicationView.Current).Transform(new Point(0, 0));
            switch (Placement)
            {
                case FlyoutPlacementMode.Top:
                    SetTopPlacement(relativePoint);
                    break;
                case FlyoutPlacementMode.Bottom:
                    SetBottomPlacement(relativePoint);
                    break;
                case FlyoutPlacementMode.Left:
                    break;
                case FlyoutPlacementMode.Right:
                    break;
                case FlyoutPlacementMode.Full:
                    SetFullPlacement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
