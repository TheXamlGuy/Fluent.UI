using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public class DatePickerFlyoutPresenter : Control
    {
        internal static DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date),
                typeof(DateTime), typeof(DatePickerFlyoutPresenter),
                new PropertyMetadata(DateTime.Now));

        internal static DependencyProperty UncommitteddDateProperty =
            DependencyProperty.Register(nameof(UncommitteddDate),
                typeof(DateTime), typeof(DatePickerFlyoutPresenter),
                new PropertyMetadata(DateTime.Now));

        internal DatePickerExtension Owner;

        private Button _acceptButton;
        private Grid _acceptDismissHostGrid;
        private DayDataSource _dayDataSource;
        private Button _dismissButton;
        private Grid _dismissLayer;
        private LoopingSelector _firstLoopingSelector;
        private Border _firstPickerHost;
        internal bool IsOpen;
        private bool _isTemplateApplied;
        private MonthDataSource _monthDataSource;
        private LoopingSelector _secondLoopingSelector;
        private Border _secondPickerHost;
        private UIElement _targetElement;
        private LoopingSelector _thirdLoopingSelector;
        private Border _thirdPickerHost;
        private YearDataSource _yearDataSource;

        protected internal DatePickerFlyoutPresenter()
        {
            DefaultStyleKey = typeof(DatePickerFlyoutPresenter);
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ApplicationView.Current.PopupRoot.SizeChanged -= OnSizePopupRootChanged;
            ApplicationView.Current.Deactivated -= OnApplicationViewDeactivated;
        }

        internal DateTime Date
        {
            get => (DateTime)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        internal DateTime UncommitteddDate
        {
            get => (DateTime)GetValue(UncommitteddDateProperty);
            set => SetValue(UncommitteddDateProperty, value);
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

            _isTemplateApplied = true;
            UpdateFlyoutPosition();
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

        private void CommitDate()
        {
            SetValue(DateProperty, UncommitteddDate);
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CommitDate();
            CloseFlyout();
        }

        private void OnApplicationViewDeactivated(object sender, EventArgs eventArgs)
        {
            CloseFlyout();
        }

        private void OnDismissButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseFlyout();
        }

        private void OnDismissLayerMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            CloseFlyout();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            TryParseDate();
        }

        private void OnSizePopupRootChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateFlyoutPosition();
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
            SetValue(UncommitteddDateProperty, Date);

            _dayDataSource = new DayDataSource();

            var itemsPropertyBinding = new Binding
            {
                Source = _dayDataSource,
                Path = new PropertyPath(DayDataSource.ItemsProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_firstLoopingSelector, ItemsControl.ItemsSourceProperty, itemsPropertyBinding);

            var datePropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(UncommitteddDateProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_dayDataSource, DayDataSource.DateProperty, datePropertyBinding);

            var selectedItemPropertyBinding = new Binding
            {
                Source = _firstLoopingSelector,
                Path = new PropertyPath(Selector.SelectedItemProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_dayDataSource, DayDataSource.SelectedItemProperty, selectedItemPropertyBinding);

            _dayDataSource.SetDefaultItem(Date);
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
            _monthDataSource = new MonthDataSource();

            var itemsPropertyBinding = new Binding
            {
                Source = _monthDataSource,
                Path = new PropertyPath(MonthDataSource.ItemsProperty)
            };

            BindingOperations.SetBinding(_secondLoopingSelector, ItemsControl.ItemsSourceProperty, itemsPropertyBinding);

            var selectedItemPropertyBinding = new Binding
            {
                Source = _secondLoopingSelector,
                Path = new PropertyPath(Selector.SelectedItemProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_monthDataSource, MonthDataSource.SelectedItemProperty, selectedItemPropertyBinding);

            _monthDataSource.SetDefaultItem(Date);
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
            _yearDataSource = new YearDataSource();

            var itemsPropertyBinding = new Binding
            {
                Source = _yearDataSource,
                Path = new PropertyPath(YearDataSource.ItemsProperty)
            };

            BindingOperations.SetBinding(_thirdLoopingSelector, ItemsControl.ItemsSourceProperty, itemsPropertyBinding);

            var selectedItemPropertyBinding = new Binding
            {
                Source = _thirdLoopingSelector,
                Path = new PropertyPath(Selector.SelectedItemProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_yearDataSource, YearDataSource.SelectedItemProperty, selectedItemPropertyBinding);

            _yearDataSource.SetDefaultItem(Date);
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

        private void TryParseDate()
        {
            var day = _dayDataSource.SelectedItem;
            var month = _monthDataSource.SelectedItem;
            var year = _yearDataSource.SelectedItem;

            if (day == null || month == null || year == null)
            {
                return;
            }

            var value = $"{day.Value}/{month.Value}/{year.Value}";
            if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out var parsedDate))
            {
                SetValue(UncommitteddDateProperty, parsedDate);
               // _uncommitedDate = parsedDate;
            }
        }

        private void UpdateFlyoutPosition()
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            var offSet = _targetElement.TransformToAncestor(ApplicationView.Current.PopupRoot).Transform(new Point(0, 0));

            var targetElementHeight = _targetElement.DesiredSize.Height / 2;

            var flyoutHeight = Math.Max(DesiredSize.Height, Height);
            var footerHeight = _acceptDismissHostGrid != null ? Math.Max(_acceptDismissHostGrid.DesiredSize.Height , _acceptDismissHostGrid.Height) : 0;

            var flyoutVerticalOffset = (flyoutHeight - footerHeight) / 2;

            var verticalOffset = offSet.Y + targetElementHeight - flyoutVerticalOffset;
            var horizontalOffset = offSet.X;

            PopupRoot.SetLeft(this, horizontalOffset);
            PopupRoot.SetTop(this, verticalOffset);
        }
    }
}
