using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class NotificationFlyoutPresenter : ContentControl
    {
        internal static DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter),
                typeof(object), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand),
                typeof(ICommand), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty IsPrimaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsPrimaryButtonEnabled),
                typeof(bool), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(true));

        internal static DependencyProperty IsSecondaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsSecondaryButtonEnabled),
                typeof(bool), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(true));

        internal static DependencyProperty PrimaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommandParameter),
                typeof(object), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty PrimaryButtonCommandProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommand),
                typeof(ICommand), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty PrimaryButtonStyleProperty =
            DependencyProperty.Register(nameof(PrimaryButtonStyle),
                typeof(Style), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty PrimaryButtonTextProperty =
            DependencyProperty.Register(nameof(PrimaryButtonText),
                typeof(string), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty SecondaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommandParameter),
                typeof(object), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty SecondaryButtonCommandProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommand),
                typeof(ICommand), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty SecondaryButtonStyleProperty =
            DependencyProperty.Register(nameof(SecondaryButtonStyle),
                typeof(Style), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null, OnPrimaryButtonTextPropertChanged));

        internal static DependencyProperty SecondaryButtonTextProperty =
            DependencyProperty.Register(nameof(SecondaryButtonText),
                typeof(string), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null, OnSecondaryButtonTextPropertyChanged));

        internal static DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title),
                typeof(string), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal static DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(nameof(TitleTemplate),
                typeof(DataTemplate), typeof(NotificationFlyoutPresenter),
                new PropertyMetadata(null));

        internal NotificationFlyout Owner;

        private bool _isShowing;

        private bool _isShowRequest;

        private bool _isTemplateApplied;

        private Button _primaryButton;

        private Button _secondaryButton;

        protected internal NotificationFlyoutPresenter()
        {
            DefaultStyleKey = typeof(NotificationFlyoutPresenter);
        }

        public event TypedEventHandler<NotificationFlyoutPresenter, ContentDialogButtonClickEventArgs> PrimaryButtonClick;

        public event TypedEventHandler<NotificationFlyoutPresenter, ContentDialogButtonClickEventArgs> SecondaryButtonClick;

        internal ICommand CloseButtonCommand
        {
            get => (ICommand)GetValue(CloseButtonCommandParameterProperty);
            set => SetValue(CloseButtonCommandParameterProperty, value);
        }

        internal object CloseButtonCommandParameter
        {
            get => GetValue(CloseButtonCommandParameterProperty);
            set => SetValue(CloseButtonCommandParameterProperty, value);
        }

        internal bool IsPrimaryButtonEnabled
        {
            get => (bool)GetValue(IsPrimaryButtonEnabledProperty);
            set => SetValue(IsPrimaryButtonEnabledProperty, value);
        }

        internal bool IsSecondaryButtonEnabled
        {
            get => (bool)GetValue(IsSecondaryButtonEnabledProperty);
            set => SetValue(IsSecondaryButtonEnabledProperty, value);
        }

        internal ICommand PrimaryButtonCommand
        {
            get => (ICommand)GetValue(PrimaryButtonCommandProperty);
            set => SetValue(PrimaryButtonCommandProperty, value);
        }

        internal object PrimaryButtonCommandParameter
        {
            get => GetValue(PrimaryButtonCommandParameterProperty);
            set => SetValue(PrimaryButtonCommandParameterProperty, value);
        }

        internal Style PrimaryButtonStyle
        {
            get => (Style)GetValue(PrimaryButtonStyleProperty);
            set => SetValue(PrimaryButtonStyleProperty, value);
        }

        internal string PrimaryButtonText
        {
            get => (string)GetValue(PrimaryButtonTextProperty);
            set => SetValue(PrimaryButtonTextProperty, value);
        }

        internal ICommand SecondaryButtonCommand
        {
            get => (ICommand)GetValue(SecondaryButtonCommandProperty);
            set => SetValue(SecondaryButtonCommandProperty, value);
        }

        internal object SecondaryButtonCommandParameter
        {
            get => GetValue(SecondaryButtonCommandParameterProperty);
            set => SetValue(SecondaryButtonCommandParameterProperty, value);
        }

        internal Style SecondaryButtonStyle
        {
            get => (Style)GetValue(SecondaryButtonStyleProperty);
            set => SetValue(SecondaryButtonStyleProperty, value);
        }

        internal string SecondaryButtonText
        {
            get => (string)GetValue(SecondaryButtonTextProperty);
            set => SetValue(SecondaryButtonTextProperty, value);
        }

        internal object Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        internal DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("LayoutRoot") is Border layoutRoot)
            {
                var dialogShowingVisualTransition = layoutRoot.GetVisualTransition("Showing");
                if (dialogShowingVisualTransition != null)
                {
                    dialogShowingVisualTransition.Storyboard.Completed -= OnShowingCompleted;
                    dialogShowingVisualTransition.Storyboard.Completed += OnShowingCompleted;
                }

                var dialogClosingVisualTransition = layoutRoot.GetVisualTransition("Hidden");
                if (dialogClosingVisualTransition != null)
                {
                    dialogClosingVisualTransition.Storyboard.Completed -= OnClosingCompleted;
                    dialogClosingVisualTransition.Storyboard.Completed += OnClosingCompleted;
                }
            }

            if (GetTemplateChild("CloseButton") is Button closeButton)
            {
                closeButton.Click -= OnCloseButtonClick;
                closeButton.Click += OnCloseButtonClick;
            }

            ChangeVisualStates(false);
            _isTemplateApplied = true;

            PreparePrimaryButton();
            PrepareSecondaryButton();

            if (_isShowRequest)
            {
                Show();
            }
        }
        internal void Hide()
        {
            HideFlyout();
        }

        internal void Show()
        {
            ShowFlyout();
        }

        private static void OnPrimaryButtonTextPropertChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var notificationFlyoutPresenter = dependencyObject as NotificationFlyoutPresenter;
            notificationFlyoutPresenter?.SetButtonVisualStates();
        }

        private static void OnSecondaryButtonTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var notificationFlyoutPresenter = dependencyObject as NotificationFlyoutPresenter;
            notificationFlyoutPresenter?.SetButtonVisualStates();
        }

        private void ChangeVisualStates(bool useTransitions = true)
        {
            VisualStateManager.GoToState(this, _isShowing ? "Showing" : "Hidden", useTransitions);
        }

        private void HideFlyout()
        {
            if (!_isShowing)
            {
                return;
            }

            _isShowing = false;
            ChangeVisualStates();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            HideFlyout();
        }

        private void OnClosingCompleted(object sender, EventArgs eventArgs)
        {
            Owner?.FlyoutClosed();
        }

        private async void OnPrimaryButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (PrimaryButtonClick != null)
            {
                var eventArgs = new ContentDialogButtonClickEventArgs();
                foreach (var eventDelegate in PrimaryButtonClick.GetInvocationList())
                {
                    var handler = (TypedEventHandler<NotificationFlyoutPresenter, ContentDialogButtonClickEventArgs>)eventDelegate;
                    handler(this, eventArgs);

                    var deferral = eventArgs.GetCurrentDeferralAndReset();
                    if (deferral != null)
                    {
                        await deferral.WaitForCompletion();
                    }

                    if (eventArgs.Cancel)
                    {
                        return;
                    }
                }
            }

            PrimaryButtonCommand?.Execute(PrimaryButtonCommandParameter);
            HideFlyout();
        }

        private async void OnSecondaryButtonClick(object sender, RoutedEventArgs args)
        {
            if (SecondaryButtonClick != null)
            {
                var eventArgs = new ContentDialogButtonClickEventArgs();
                foreach (var eventDelegate in SecondaryButtonClick.GetInvocationList())
                {
                    var handler = (TypedEventHandler<NotificationFlyoutPresenter, ContentDialogButtonClickEventArgs>)eventDelegate;
                    handler(this, eventArgs);

                    var deferral = eventArgs.GetCurrentDeferralAndReset();
                    if (deferral != null)
                    {
                        await deferral.WaitForCompletion();
                    }

                    if (eventArgs.Cancel)
                    {
                        return;
                    }
                }
            }

            SecondaryButtonCommand?.Execute(SecondaryButtonCommandParameter);
            HideFlyout();
        }
        private void OnShowingCompleted(object sender, EventArgs eventArgs)
        {
            Owner?.FlyoutOpened();
        }

        private void PreparePrimaryButton()
        {
            _primaryButton = GetTemplateChild("PrimaryButton") as Button;
            if (_primaryButton != null)
            {
                _primaryButton.Click += OnPrimaryButtonClick;

                var primaryButtonTextBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(PrimaryButtonTextProperty)
                };

                var primaryButtonStyle = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(PrimaryButtonStyleProperty)
                };

                BindingOperations.SetBinding(_primaryButton, ContentProperty, primaryButtonTextBinding);
                BindingOperations.SetBinding(_primaryButton, StyleProperty, primaryButtonStyle);
            }
        }
        private void PrepareSecondaryButton()
        {
            _secondaryButton = GetTemplateChild("SecondaryButton") as Button;
            if (_secondaryButton != null)
            {
                _secondaryButton.Click += OnSecondaryButtonClick;

                var secondaryButtonTextBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(SecondaryButtonTextProperty)
                };

                var secondaryButtonStyle = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(SecondaryButtonStyleProperty)
                };

                BindingOperations.SetBinding(_secondaryButton, ContentProperty, secondaryButtonTextBinding);
                BindingOperations.SetBinding(_secondaryButton, StyleProperty, secondaryButtonStyle);
            }
        }
        private void SetButtonVisualStates()
        {
            var isPrimaryButtonVisible = !string.IsNullOrEmpty(PrimaryButtonText);
            var isSecondaryButtonVisible = !string.IsNullOrEmpty(SecondaryButtonText);

            string state;

            if (isPrimaryButtonVisible && isSecondaryButtonVisible)
            {
                state = "PrimaryAndSecondaryVisible";
            }

            else if (isPrimaryButtonVisible)
            {
                state = "PrimaryVisible";
            }
            else if (isSecondaryButtonVisible)
            {
                state = "SecondaryVisible";
            }
            else
            {
                state = "NoneVisible";
            }

            VisualStateManager.GoToState(this, state, true);
        }

        private void ShowFlyout()
        {
            if (_isShowing)
                return;

            if (!_isTemplateApplied)
            {
                _isShowRequest = true;
            }
            else
            {
                _isShowing = true;
                ChangeVisualStates();
            }
        }
    }
}
