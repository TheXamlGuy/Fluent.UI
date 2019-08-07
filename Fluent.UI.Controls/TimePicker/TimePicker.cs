using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Fluent.UI.Controls.Converters;

namespace Fluent.UI.Controls
{
    public class TimePicker : Control
    {
        public static DependencyProperty ClockIdentifierProperty =
            DependencyProperty.Register(nameof(ClockIdentifier),
                typeof(string), typeof(TimePicker),
                new PropertyMetadata(TwentyFourHourClock, OnClockIdentifierPropertyChanged));

        public static DependencyProperty MinuteIncrementProperty =
            DependencyProperty.Register(nameof(MinuteIncrement),
                typeof(int), typeof(TimePicker),
                new PropertyMetadata(1));

        public static DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time),
                typeof(TimeSpan), typeof(TimePicker),
                new PropertyMetadata(DateTime.Now.TimeOfDay));

        private const string TwelveHourClock = "12HourClock";
        private const string TwentyFourHourClock = "TwentyFourHourClock";

        private readonly TimeSpanToDateTimeConverter _converter = new TimeSpanToDateTimeConverter();
        private TimePickerFlyoutPresenter _flyoutPresenter;
        private TextBlock _hourTextBlock;
        private bool _isFlyoutPrepared;
        private TextBlock _minuteTextBlock;
        private TextBlock _periodTextBlock;
        private Button _flyoutButton;

        public TimePicker()
        {
            DefaultStyleKey = typeof(TimePicker);

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            PreviewMouseUp += OnMouseUp;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CloseFlyout();
        }

        public string ClockIdentifier
        {
            get => (string)GetValue(ClockIdentifierProperty);
            set => SetValue(ClockIdentifierProperty, value);
        }

        public int MinuteIncrement
        {
            get => (int)GetValue(MinuteIncrementProperty);
            set => SetValue(MinuteIncrementProperty, value);
        }

        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public override void OnApplyTemplate()
        {
            _hourTextBlock = GetTemplateChild("HourTextBlock") as TextBlock;
            if (_hourTextBlock != null)
            {
                PrepareHourTextBlock();
            }

            _minuteTextBlock = GetTemplateChild("MinuteTextBlock") as TextBlock;
            if (_minuteTextBlock != null)
            {
                PrepareHourTextlock();
            }

            _periodTextBlock = GetTemplateChild("PeriodTextBlock") as TextBlock;
            if (_periodTextBlock != null)
            {
                PreparePeriodTextBlock();
            }

            _flyoutButton = GetTemplateChild("FlyoutButton") as Button;

            SetClockIdentifierVisualStates();
        }

        internal void CloseFlyout()
        {
            _flyoutPresenter.Close();
        }

        private static void OnClockIdentifierPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var timePicker = dependencyObject as TimePicker;
            timePicker?.OnClockIdentifierPropertyChanged();
        }

        private string GetClockIdentifier()
        {
            if (ClockIdentifier == TwelveHourClock)
            {
                return TwelveHourClock;
            }

            return ClockIdentifier == TwentyFourHourClock ? TwentyFourHourClock : TwelveHourClock;
        }

        private void OnClockIdentifierPropertyChanged()
        {
            SetClockIdentifierVisualStates();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            PrepareFlyout();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ToggleFlyout();
        }

        private void OpenFlyout()
        {
            _flyoutPresenter.ShowAt(_flyoutButton);
        }

        private void PrepareFlyout()
        {
            if (_isFlyoutPrepared)
            {
                return;
            }

            PrepareFlyoutPresenter();

            _isFlyoutPrepared = true;
        }

        private void PrepareFlyoutPresenter()
        {
            _flyoutPresenter = new TimePickerFlyoutPresenter
            {
                Owner = this
            };

            var timePropertyBinding = new Binding
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(TimeProperty)
            };

            var clockIdentifierPropertyBinding = new Binding
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(ClockIdentifierProperty)
            };

            var minuteIncrementPropertyBinding = new Binding
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath(MinuteIncrementProperty)
            };

            BindingOperations.SetBinding(_flyoutPresenter, TimePickerFlyoutPresenter.TimeProperty, timePropertyBinding);
            BindingOperations.SetBinding(_flyoutPresenter, TimePickerFlyoutPresenter.ClockIdentifierProperty, clockIdentifierPropertyBinding);
            BindingOperations.SetBinding(_flyoutPresenter, TimePickerFlyoutPresenter.MinuteIncrementProperty, minuteIncrementPropertyBinding);

            _flyoutPresenter.ApplyTemplate();
        }

        private void PrepareHourTextlock()
        {
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath(TimeProperty),
                StringFormat = "mm"
            };

            BindingOperations.SetBinding(_minuteTextBlock, TextBlock.TextProperty, binding);
        }

        private void PrepareHourTextBlock()
        {
            var format = GetClockIdentifier() == TwelveHourClock ? "hh" : "HH";
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath(TimeProperty),
                StringFormat = format,
                Converter = _converter
            };

            BindingOperations.SetBinding(_hourTextBlock, TextBlock.TextProperty, binding);
        }

        private void PreparePeriodTextBlock()
        {
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath(TimeProperty),
                StringFormat = "tt",
                Converter = _converter
            };

            BindingOperations.SetBinding(_periodTextBlock, TextBlock.TextProperty, binding);
        }

        private void SetClockIdentifierVisualStates()
        {
            VisualStateManager.GoToState(this, GetClockIdentifier() == TwelveHourClock ? "TwelveHourClock" : "TwentyFourHourClock", true);
        }

        private void ToggleFlyout()
        {
            if (_flyoutPresenter.IsOpen)
            {
                CloseFlyout();
            }
            else
            {
                OpenFlyout();
            }
        }
    }
}
