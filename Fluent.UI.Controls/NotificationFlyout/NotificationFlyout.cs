using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    [ContentProperty("Content")]
    public class NotificationFlyout : DependencyObject
    {
        public static DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content),
                typeof(object), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(Interval),
                typeof(TimeSpan), typeof(NotificationFlyout),
                new PropertyMetadata(TimeSpan.FromMilliseconds(1000)));

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title),
                typeof(string), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(nameof(TitleTemplate),
                typeof(DataTemplate), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter),
                typeof(object), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand),
                typeof(ICommand), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty IsPrimaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsPrimaryButtonEnabled),
                typeof(bool), typeof(NotificationFlyout),
                new PropertyMetadata(true));

        public static DependencyProperty IsSecondaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsSecondaryButtonEnabled),
                typeof(bool), typeof(NotificationFlyout),
                new PropertyMetadata(true));

        public static DependencyProperty PrimaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommandParameter),
                typeof(object), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty PrimaryButtonCommandProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommand),
                typeof(ICommand), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty PrimaryButtonStyleProperty =
            DependencyProperty.Register(nameof(PrimaryButtonStyle),
                typeof(Style), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty PrimaryButtonTextProperty =
            DependencyProperty.Register(nameof(PrimaryButtonText),
                typeof(string), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommandParameter),
                typeof(object), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonCommandProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommand),
                typeof(ICommand), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonStyleProperty =
            DependencyProperty.Register(nameof(SecondaryButtonStyle),
                typeof(Style), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonTextProperty =
            DependencyProperty.Register(nameof(SecondaryButtonText),
                typeof(string), typeof(NotificationFlyout),
                new PropertyMetadata(null));

        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private NotificationFlyoutPresenter _flyoutPresenter;

        private bool _isFlyoutPrepared;

        public event EventHandler<object> Closed;

        public event TypedEventHandler<NotificationFlyout, FlyoutBaseClosingEventArgs> Closing;

        public event EventHandler<object> Opened;

        public event EventHandler<object> Opening;

        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public TimeSpan Interval
        {
            get => (TimeSpan)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }

        public object Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public ICommand CloseButtonCommand
        {
            get => (ICommand)GetValue(CloseButtonCommandParameterProperty);
            set => SetValue(CloseButtonCommandParameterProperty, value);
        }

        public object CloseButtonCommandParameter
        {
            get => GetValue(CloseButtonCommandParameterProperty);
            set => SetValue(CloseButtonCommandParameterProperty, value);
        }

        public bool IsPrimaryButtonEnabled
        {
            get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
            set => SetValue(IsPrimaryButtonEnabledProperty, value);
        }

        public bool IsSecondaryButtonEnabled
        {
            get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
            set => SetValue(IsSecondaryButtonEnabledProperty, value);
        }

        public ICommand PrimaryButtonCommand
        {
            get => (ICommand)GetValue(PrimaryButtonCommandProperty);
            set => SetValue(PrimaryButtonCommandProperty, value);
        }

        public object PrimaryButtonCommandParameter
        {
            get => GetValue(PrimaryButtonCommandParameterProperty);
            set => SetValue(PrimaryButtonCommandParameterProperty, value);
        }

        public Style PrimaryButtonStyle
        {
            get => (Style)GetValue(PrimaryButtonStyleProperty);
            set => SetValue(PrimaryButtonStyleProperty, value);
        }

        public string PrimaryButtonText
        {
            get => (string)GetValue(PrimaryButtonTextProperty);
            set => SetValue(PrimaryButtonTextProperty, value);
        }

        public ICommand SecondaryButtonCommand
        {
            get => (ICommand)GetValue(SecondaryButtonCommandProperty);
            set => SetValue(SecondaryButtonCommandProperty, value);
        }

        public object SecondaryButtonCommandParameter
        {
            get => GetValue(SecondaryButtonCommandParameterProperty);
            set => SetValue(SecondaryButtonCommandParameterProperty, value);
        }

        public Style SecondaryButtonStyle
        {
            get => (Style)GetValue(SecondaryButtonStyleProperty);
            set => SetValue(SecondaryButtonStyleProperty, value);
        }

        public string SecondaryButtonText
        {
            get => (string)GetValue(SecondaryButtonTextProperty);
            set => SetValue(SecondaryButtonTextProperty, value);
        }

        public void Hide()
        {
            CloseFlyout();
        }

        public void Show()
        {
            OpenFlyout();
        }

        internal void FlyoutClosed()
        {
            RemoveFlyout();
        }

        internal void FlyoutOpened()
        {
            SetFlyout();
        }

        private void ClearPopupRoot()
        {
            ApplicationView.Current.PopupRoot.Children.Remove(_flyoutPresenter);
        }

        private void CloseFlyout()
        {
            if (Closing != null)
            {
                var args = new FlyoutBaseClosingEventArgs();
                foreach (var eventDelegate in Closing.GetInvocationList())
                {
                    var handler = (TypedEventHandler<NotificationFlyout, FlyoutBaseClosingEventArgs>)eventDelegate;
                    handler(this, args);
                    if (args.Cancel)
                    {
                        return;
                    }
                }
            }

            _flyoutPresenter.Hide();
        }

        private void OnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            _dispatcherTimer.Stop();
        }

        private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            _dispatcherTimer.Start();
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            _dispatcherTimer.Stop();
            CloseFlyout();
        }

        private void OpenFlyout()
        {
            Opening?.Invoke(this, null);

            PrepareFlyout();
            PreparePopupRoot();

            _flyoutPresenter.Show();
        }

        private void PrepareFlyout()
        {
            if (_isFlyoutPrepared)
                return;

            PrepareFlyoutPresenter();
            _isFlyoutPrepared = true;
        }

        private void PrepareFlyoutPresenter()
        {
            _flyoutPresenter = new NotificationFlyoutPresenter
            {
                Owner = this
            };

            var contentPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(ContentProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, ContentControl.ContentProperty, contentPropertyBinding);

            var titlePropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(TitleProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.TitleProperty, titlePropertyBinding);

            var titleTemplatePropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(TitleTemplateProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.TitleTemplateProperty, titleTemplatePropertyBinding);

            var closeButtonCommandPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(CloseButtonCommandProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.CloseButtonCommandProperty, closeButtonCommandPropertyBinding);

            var closeButtonCommandParameterPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(CloseButtonCommandParameterProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.CloseButtonCommandParameterProperty, closeButtonCommandParameterPropertyBinding);

            var isPrimaryButtonEnabledPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(IsPrimaryButtonEnabledProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.IsPrimaryButtonEnabledProperty, isPrimaryButtonEnabledPropertyBinding);

            var isSecondaryButtonEnabledPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(IsSecondaryButtonEnabledProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.IsSecondaryButtonEnabledProperty, isSecondaryButtonEnabledPropertyBinding);

            var primaryButtonCommandPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(PrimaryButtonCommandProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.PrimaryButtonCommandProperty, primaryButtonCommandPropertyBinding);

            var primaryButtonCommandParameterPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(PrimaryButtonCommandParameterProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.PrimaryButtonCommandParameterProperty, primaryButtonCommandParameterPropertyBinding);

            var primaryButtonStylePropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(PrimaryButtonStyleProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.PrimaryButtonStyleProperty, primaryButtonStylePropertyBinding);

            var primaryButtonTextPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(PrimaryButtonTextProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.PrimaryButtonTextProperty, primaryButtonTextPropertyBinding);

            var secondaryButtonCommandPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(SecondaryButtonCommandProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.SecondaryButtonCommandProperty, secondaryButtonCommandPropertyBinding);

            var secondaryButtonCommandParameterPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(SecondaryButtonCommandParameterProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.SecondaryButtonCommandParameterProperty, secondaryButtonCommandParameterPropertyBinding);

            var secondaryButtonStylePropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(SecondaryButtonStyleProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.SecondaryButtonStyleProperty, secondaryButtonStylePropertyBinding);

            var secondaryButtonTextPropertyBinding = new Binding
            {
                Source = this,
                Path = new PropertyPath(SecondaryButtonTextProperty),
                Mode = BindingMode.TwoWay
            };

            BindingOperations.SetBinding(_flyoutPresenter, NotificationFlyoutPresenter.SecondaryButtonTextProperty, secondaryButtonTextPropertyBinding);

            _flyoutPresenter.MouseEnter -= OnMouseEnter;
            _flyoutPresenter.MouseEnter += OnMouseEnter;

            _flyoutPresenter.MouseLeave -= OnMouseLeave;
            _flyoutPresenter.MouseLeave += OnMouseLeave;
        }

        private void PreparePopupRoot()
        {
            ApplicationView.Current.PopupRoot.Children.Add(_flyoutPresenter);
            UpdateFlyoutPosition();
        }

        private void RemoveFlyout()
        {
            ClearPopupRoot();
            Closed?.Invoke(this, null);

            _dispatcherTimer.Tick -= OnTick;
        }

        private void SetFlyout()
        {
            Opened?.Invoke(this, null);

            _dispatcherTimer.Tick -= OnTick;
            _dispatcherTimer.Tick += OnTick;
            _dispatcherTimer.Interval = Interval;
            _dispatcherTimer.Start();
        }

        private void UpdateFlyoutPosition()
        {
            PopupRoot.SetRight(_flyoutPresenter, 24);
            PopupRoot.SetBottom(_flyoutPresenter, 12);
        }
    }
}