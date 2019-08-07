using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public class TimePickerFlyoutPresenter : Control
    {
        public static DependencyProperty ClockIdentifierProperty =
            DependencyProperty.Register(nameof(TimePickerFlyoutPresenter),
                typeof(string), typeof(TimePickerFlyoutPresenter),
                new PropertyMetadata(TwentyFourHourClock, OnClockIdentifierPropertyChanged));

        internal static DependencyProperty MinuteIncrementProperty =
            DependencyProperty.Register(nameof(MinuteIncrement),
                typeof(int), typeof(TimePickerFlyoutPresenter),
                new PropertyMetadata(1));

        internal static DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time),
                typeof(TimeSpan), typeof(TimePickerFlyoutPresenter),
                new PropertyMetadata(DateTime.Now.TimeOfDay));

        internal TimePicker Owner;

        private const string TwelveHourClock = "12HourClock";
        private const string TwentyFourHourClock = "TwentyFourHourClock";

        private Button _acceptButton;
        private Grid _acceptDismissHostGrid;
        private Button _dismissButton;
        private Grid _dismissLayer;
        private IList<TimePickerItem> _firstItems;
        private LoopingSelector _firstLoopingSelector;
        private Border _firstPickerHost;
        private bool _isInternalSelection;
        private IList<TimePickerItem> _secondItems;
        private LoopingSelector _secondLoopingSelector;
        private Border _secondPickerHost;
        private UIElement _targetElement;
        private IList<TimePickerItem> _thirdItems;
        private LoopingSelector _thirdLoopingSelector;
        private Border _thirdPickerHost;
        internal bool IsOpen;
        private TimeSpan _uncommitedTimeSpan;

        protected internal TimePickerFlyoutPresenter()
        {
            DefaultStyleKey = typeof(TimePickerFlyoutPresenter);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        internal string ClockIdentifier
        {
            get => (string)GetValue(ClockIdentifierProperty);
            set => SetValue(ClockIdentifierProperty, value);
        }

        internal int MinuteIncrement
        {
            get => (int)GetValue(MinuteIncrementProperty);
            set => SetValue(MinuteIncrementProperty, value);
        }

        internal TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public override void OnApplyTemplate()
        {
            _acceptDismissHostGrid = GetTemplateChild("AcceptDismissHostGrid") as Grid;

            PrepareFirstPickerHost();
            PrepareSecondPickerHost();
            PrepareThirdPickerHost();

            PrepareSelectionChanged();

            PrepareAcceptButton();
            PrepareDismissButton();

            PrepareClockIdentifierVisualStates();
        }

        internal void Close()
        {
            CloseFlyout();
        }

        internal void ShowAt(UIElement targetElement)
        {
            _targetElement = targetElement;
            PrepareFlyout();
        }

        private static void OnClockIdentifierPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var timePickerFlyoutPresenter = dependencyObject as TimePickerFlyoutPresenter;
            timePickerFlyoutPresenter?.OnClockIdentifierPropertyChanged();
        }

        private void ClearPopupRoot()
        {
            ApplicationView.Current.PopupRoot.Children.Remove(this);
            ApplicationView.Current.PopupRoot.Children.Remove(_dismissLayer);

            _dismissLayer.MouseDown -= OnDismissLayerMouseDown;
            _dismissLayer = null;
        }

        private void CloseFlyout()
        {
            if (!IsOpen)
            {
                return;
            }
            
            ClearPopupRoot();
            IsOpen = false;
        }

        private void CommitTimeSpan()
        {
            SetValue(TimeProperty, _uncommitedTimeSpan);
        }

        private string GetClockIdentifier()
        {
            if (ClockIdentifier == TwelveHourClock)
            {
                return TwelveHourClock;
            }

            return ClockIdentifier == TwentyFourHourClock ? TwentyFourHourClock : TwelveHourClock;
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CommitTimeSpan();
            CloseFlyout();
        }

        private void OnApplicationViewDeactivated(object sender, EventArgs eventArgs)
        {
            CloseFlyout();
        }

        private void OnClockIdentifierPropertyChanged()
        {
            PrepareClockIdentifierVisualStates();
        }

        private void OnDismissButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseFlyout();
        }

        private void OnDismissLayerMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            CloseFlyout();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetFirstPickerValue();
            SetSecondPickerValue();
            SetThirdPickerValue();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (!_isInternalSelection)
            {
                ParseTimeSpan();
            }
        }

        private void OnSizePopupRootChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateFlyoutPosition();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ApplicationView.Current.PopupRoot.SizeChanged -= OnSizePopupRootChanged;
            ApplicationView.Current.Deactivated -= OnApplicationViewDeactivated;
        }

        private void ParseTimeSpan()
        {
            var firstSelectedIndex = _firstLoopingSelector?.SelectedIndex ?? 0;
            var secondSelectedIndex = _secondLoopingSelector?.SelectedIndex ?? 0;
            var thirdSelectedIndex = _thirdLoopingSelector?.SelectedIndex ?? 0;

            var firstItem = TwentyFourHourDataSource.GetItemFromIndex(firstSelectedIndex);
            var secondItem = MinuteDataSource.GetItemFromIndex(secondSelectedIndex);
            var thirdItem = AmPmDataSource.GetItemFromIndex(thirdSelectedIndex);

            var value = $"{firstItem.Value}:{secondItem.Value} {(GetClockIdentifier() == TwelveHourClock ? thirdItem.Value : string.Empty)}";
            if (DateTime.TryParse(value, CultureInfo.CurrentUICulture, DateTimeStyles.None, out var parsedDateTime))
            {
                _uncommitedTimeSpan = parsedDateTime.TimeOfDay;
            }
        }

        private void PrepareAcceptButton()
        {
            _acceptButton = GetTemplateChild("AcceptButton") as Button;
            if (_acceptButton != null)
            {
                _acceptButton.Click -= OnAcceptButtonClick;
                _acceptButton.Click += OnAcceptButtonClick;
            }
        }

        private void PrepareClockIdentifierVisualStates()
        {
            VisualStateManager.GoToState(this, GetClockIdentifier() == TwelveHourClock ? "TwelveHourClock" : "TwentyFourHourClock", true);
        }

        private void PrepareDismissButton()
        {
            _dismissButton = GetTemplateChild("DismissButton") as Button;
            if (_dismissButton != null)
            {
                _dismissButton.Click -= OnDismissButtonClick;
                _dismissButton.Click += OnDismissButtonClick;
            }
        }

        private void PrepareFirstDataSource()
        {
            _firstItems = GetClockIdentifier() == TwelveHourClock ? TwelveHourDataSource.GetItems() : TwentyFourHourDataSource.GetItems();
            _firstLoopingSelector.ItemsSource = _firstItems;
        }

        private void PrepareFirstPickerHost()
        {
            _firstPickerHost = GetTemplateChild("FirstPickerHost") as Border;
            if (_firstPickerHost != null)
            {
                _firstLoopingSelector = new LoopingSelector();
                _firstPickerHost.Child = _firstLoopingSelector;
                PrepareFirstDataSource();
            }
        }

        private void PrepareFlyout()
        {
            _dismissLayer = new Grid
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };

            _dismissLayer.MouseDown -= OnDismissLayerMouseDown;
            _dismissLayer.MouseDown += OnDismissLayerMouseDown;

            ApplicationView.Current.PopupRoot.SizeChanged -= OnSizePopupRootChanged;
            ApplicationView.Current.PopupRoot.SizeChanged += OnSizePopupRootChanged;

            ApplicationView.Current.Deactivated -= OnApplicationViewDeactivated;
            ApplicationView.Current.Deactivated += OnApplicationViewDeactivated;

            ApplicationView.Current.PopupRoot.Children.Add(_dismissLayer);
            ApplicationView.Current.PopupRoot.Children.Add(this);

            UpdateFlyoutPosition();

            IsOpen = true;
        }

        private void PrepareSecondDataSource()
        {
            _secondItems = MinuteDataSource.GetItems(MinuteIncrement);
            _secondLoopingSelector.ItemsSource = _secondItems;
        }

        private void PrepareSecondPickerHost()
        {
            _secondPickerHost = GetTemplateChild("SecondPickerHost") as Border;
            if (_secondPickerHost != null)
            {
                _secondLoopingSelector = new LoopingSelector();
                _secondPickerHost.Child = _secondLoopingSelector;
                PrepareSecondDataSource();
            }
        }

        private void PrepareSelectionChanged()
        {
            if (_firstLoopingSelector != null && _secondLoopingSelector != null && _thirdLoopingSelector != null)
            {
                _firstLoopingSelector.SelectionChanged -= OnSelectionChanged;
                _firstLoopingSelector.SelectionChanged += OnSelectionChanged;

                _secondLoopingSelector.SelectionChanged -= OnSelectionChanged;
                _secondLoopingSelector.SelectionChanged += OnSelectionChanged;

                _thirdLoopingSelector.SelectionChanged -= OnSelectionChanged;
                _thirdLoopingSelector.SelectionChanged += OnSelectionChanged;
            }
        }

        private void PrepareThirdDataSource()
        {
            _thirdItems = AmPmDataSource.GetItems();
            _thirdLoopingSelector.ItemsSource = _thirdItems;
        }

        private void PrepareThirdPickerHost()
        {
            _thirdPickerHost = GetTemplateChild("ThirdPickerHost") as Border;
            if (_thirdPickerHost != null)
            {
                _thirdLoopingSelector = new LoopingSelector();
                _thirdPickerHost.Child = _thirdLoopingSelector;
                PrepareThirdDataSource();
            }
        }

        private void SetFirstPickerValue()
        {
            _isInternalSelection = true;

            var value = (DateTime.Today + Time).Hour;

            var item = _firstItems.FirstOrDefault(x => x.Value.Equals(value));
            if (item == null)
            {
                return;
            }

            var index = _firstItems.IndexOf(item);

            _firstLoopingSelector.SelectedIndex = index;
            _firstLoopingSelector.ScrollToSelectedItem(false);

            _isInternalSelection = false;
        }

        private void SetSecondPickerValue()
        {
            _isInternalSelection = true;

            var value = (DateTime.Today + Time).Minute;

            var item = _secondItems.FirstOrDefault(x => x.Value.Equals(value));
            if (item == null)
            {
                return;
            }

            var index = _secondItems.IndexOf(item);

            _secondLoopingSelector.SelectedIndex = index;
            _secondLoopingSelector.ScrollToSelectedItem(false);

            _isInternalSelection = false;
        }

        private void SetThirdPickerValue()
        {
            _isInternalSelection = true;

            var value = (DateTime.Today + Time).ToString("tt") == "AM" ? "AM" : "PM";

            var item = _thirdItems.FirstOrDefault(x => x.Value.Equals(value));
            if (item == null)
            {
                return;
            }

            var index = _thirdItems.IndexOf(item);

            _thirdLoopingSelector.SelectedIndex = index;
            _thirdLoopingSelector.ScrollToSelectedItem(false);

            _isInternalSelection = false;
        }

        private void UpdateFlyoutPosition()
        {
            var offSet = _targetElement.TransformToAncestor(ApplicationView.Current.PopupRoot).Transform(new Point(0, 0));

            var targetElementHeight = _targetElement.DesiredSize.Height / 2;

            var flyoutHeight = Math.Max(DesiredSize.Height, Height);
            var footerHeight = _acceptDismissHostGrid != null ? Math.Max(_acceptDismissHostGrid.DesiredSize.Height, _acceptDismissHostGrid.Height) : 0;

            var flyoutVerticalOffset = (flyoutHeight - footerHeight) / 2;

            var verticalOffset = offSet.Y + targetElementHeight - flyoutVerticalOffset;
            var horizontalOffset = offSet.X;

            PopupRoot.SetLeft(this, horizontalOffset);
            PopupRoot.SetTop(this, verticalOffset);
        }
    }
}
