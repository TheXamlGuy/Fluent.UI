using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class ContentDialog : ContentControl
    {
        public static readonly DependencyProperty PopupBackgroundProperty =
            DependencyProperty.Register(nameof(PopupBackground),
                typeof(Brush), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty CloseButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseButtonCommandParameter),
                typeof(object), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty CloseButtonCommandProperty =
            DependencyProperty.Register(nameof(CloseButtonCommand),
                typeof(ICommand), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register(nameof(CloseButtonStyle),
                typeof(Style), typeof(ContentDialog),
                new PropertyMetadata(new Style(typeof(Button), (Style)Application.Current.FindResource(typeof(Button)))));

        public static DependencyProperty CloseButtonTextProperty =
            DependencyProperty.Register(nameof(CloseButtonText),
                typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null, OnCloseButtonTextPropertyChanged));

        public static DependencyProperty DefaultButtonProperty =
            DependencyProperty.Register(nameof(DefaultButton),
                typeof(ContentDialogButton), typeof(ContentDialog),
                new PropertyMetadata(ContentDialogButton.None, OnDefaultButtonPropertyChangek));

        public static DependencyProperty IsPrimaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsPrimaryButtonEnabled),
                typeof(bool), typeof(ContentDialog),
                new PropertyMetadata(true));

        public static DependencyProperty IsSecondaryButtonEnabledProperty =
            DependencyProperty.Register(nameof(IsSecondaryButtonEnabled),
                typeof(bool), typeof(ContentDialog),
                new PropertyMetadata(true));

        public static DependencyProperty PrimaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommandParameter),
                typeof(object), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty PrimaryButtonCommandProperty =
            DependencyProperty.Register(nameof(PrimaryButtonCommand),
                typeof(ICommand), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty PrimaryButtonStyleProperty =
            DependencyProperty.Register(nameof(PrimaryButtonStyle),
                typeof(Style), typeof(ContentDialog),
                new PropertyMetadata(new Style(typeof(Button), (Style)Application.Current.FindResource(typeof(Button)))));

        public static DependencyProperty PrimaryButtonTextProperty =
            DependencyProperty.Register(nameof(PrimaryButtonText),
                typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null, OnPrimaryButtonTextPropertChanged));

        public static DependencyProperty SecondaryButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommandParameter),
                typeof(object), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonCommandProperty =
            DependencyProperty.Register(nameof(SecondaryButtonCommand),
                typeof(ICommand), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty SecondaryButtonStyleProperty =
            DependencyProperty.Register(nameof(SecondaryButtonStyle),
                typeof(Style), typeof(ContentDialog),
                new PropertyMetadata(new Style(typeof(Button), (Style)Application.Current.FindResource(typeof(Button)))));

        public static DependencyProperty SecondaryButtonTextProperty =
            DependencyProperty.Register(nameof(SecondaryButtonText),
                typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null, OnSecondaryButtonTextPropertyChanged));

        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title),
                typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register(nameof(TitleTemplate),
                typeof(DataTemplate), typeof(ContentDialog),
                new PropertyMetadata(null));

        private ContentDialogAdorner _adornerDialog;
        private Button _closeButton;
        private Border _container;
        private bool _isOpen;
        private bool _isShowing;
        private bool _isTemplateReady;
        private Grid _layoutRoot;
        private ContentDialogPlacement _placement;
        private Button _primaryButton;
        private Button _secondaryButton;
        private TaskCompletionSource<object> _taskCompletionSource;
        public ContentDialog() => DefaultStyleKey = typeof(ContentDialog);

        public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> CloseButtonClick;

        public event TypedEventHandler<ContentDialog, ContentDialogClosedEventArgs> Closed;

        public event TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs> Closing;

        public event TypedEventHandler<ContentDialog, ContentDialogOpenedEventArgs> Opened;

        public event TypedEventHandler<ContentDialog, ContentDialogOpeningEventArgs> Opening;

        public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> PrimaryButtonClick;

        public event TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs> SecondaryButtonClick;

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

        public Style CloseButtonStyle
        {
            get => (Style)GetValue(CloseButtonStyleProperty);
            set => SetValue(CloseButtonStyleProperty, value);
        }

        public string CloseButtonText
        {
            get => (string)GetValue(CloseButtonTextProperty);
            set => SetValue(CloseButtonTextProperty, value);
        }

        public ContentDialogButton DefaultButton
        {
            get => (ContentDialogButton)GetValue(DefaultButtonProperty);
            set => SetValue(DefaultButtonProperty, value);
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

        public Brush PopupBackground
        {
            get => (Brush)GetValue(PopupBackgroundProperty);
            set => SetValue(PopupBackgroundProperty, value);
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
        public override void OnApplyTemplate()
        {
            _isTemplateReady = true;

            _layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            _container  = GetTemplateChild("Container") as Border;

            if (_container != null)
            {
                var dialogShowingVisualTransition = _container.FindVisualTransition("DialogShowing");
                if (dialogShowingVisualTransition != null)
                {
                    dialogShowingVisualTransition.Storyboard.Completed -= OnDialogShowingCompleted;
                    dialogShowingVisualTransition.Storyboard.Completed += OnDialogShowingCompleted;
                }

                var dialogClosingVisualTransition = _container.FindVisualTransition("DialogHidden");
                if (dialogClosingVisualTransition != null)
                {
                    dialogClosingVisualTransition.Storyboard.Completed -= OnDialogClosingCompleted;
                    dialogClosingVisualTransition.Storyboard.Completed += OnDialogClosingCompleted;
                }
            }

            PreparePrimaryButton();
            PrepareSecondaryButton();
            PrepareCloseButton();
            PrepareDefaultButton();

            UpdateButtonVisualStates();

            if (_isShowing)
            {
                UpdateShowingVisualStates();
            }
        }

        public async Task ShowAsync(ContentDialogPlacement placement)
        {
            _placement = placement;
            await ShowAsync();
        }

        public async Task ShowAsync()
        {
            PrepareOpening();

            _taskCompletionSource = new TaskCompletionSource<object>();
            await _taskCompletionSource.Task.ConfigureAwait(false);
        }

        private static void OnCloseButtonTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var contentDialog = dependencyObject as ContentDialog;
            contentDialog?.UpdateButtonVisualStates();
        }

        private static void OnDefaultButtonPropertyChangek(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var contentDialog = dependencyObject as ContentDialog;
            contentDialog?.OnDefaultButtonPropertyChange();
        }

        private static void OnPrimaryButtonTextPropertChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var contentDialog = dependencyObject as ContentDialog;
            contentDialog?.UpdateButtonVisualStates();
        }

        private static void OnSecondaryButtonTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var contentDialog = dependencyObject as ContentDialog;
            contentDialog?.UpdateButtonVisualStates();
        }

        private void FinalizeClosing()
        {
            _taskCompletionSource.SetResult(null);

            if (Closed != null)
            {
                var contentDialogClosedEventArgs = new ContentDialogClosedEventArgs();
                Closed.Invoke(this, contentDialogClosedEventArgs);
            }

            if (_placement == ContentDialogPlacement.Popup)
            {
                _adornerDialog.Remove(_layoutRoot);
                _container.Child = _layoutRoot;
                _adornerDialog = null;
            }


            _isOpen = false;
        }
        private void FinalizeOpening()
        {
            if (Opened != null)
            {
                var args = new ContentDialogOpenedEventArgs();
                Opened.Invoke(this, args);
            }

            _isShowing = false;
            _isOpen = true;
        }

        private async void OnCloseButtonClick(object sender, RoutedEventArgs args)
        {
            if (CloseButtonClick != null)
            {
                var eventArgs = new ContentDialogButtonClickEventArgs();
                foreach (var eventDelegate in CloseButtonClick.GetInvocationList())
                {
                    var handler = (TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>)eventDelegate;
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

            CloseButtonCommand?.Execute(CloseButtonCommandParameter);
            PrepareClosing();
        }

        private void OnDefaultButtonPropertyChange() => PrepareDefaultButton();

        private void OnDialogClosingCompleted(object sender, EventArgs args)
        {
            if (!_isShowing && _isOpen)
            {
                FinalizeClosing();
            }
        }

        private void OnDialogShowingCompleted(object sender, EventArgs args)
        {
            if (_isShowing)
            {
                FinalizeOpening();
            }      
        }

        private async void OnPrimaryButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (PrimaryButtonClick != null)
            {
                var eventArgs = new ContentDialogButtonClickEventArgs();
                foreach (var eventDelegate in PrimaryButtonClick.GetInvocationList())
                {
                    var handler = (TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>)eventDelegate;
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
            PrepareClosing();
        }

        private async void OnSecondaryButtonClick(object sender, RoutedEventArgs args)
        {
            if (SecondaryButtonClick != null)
            {
                var eventArgs = new ContentDialogButtonClickEventArgs();
                foreach (var eventDelegate in SecondaryButtonClick.GetInvocationList())
                {
                    var handler = (TypedEventHandler<ContentDialog, ContentDialogButtonClickEventArgs>)eventDelegate;
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
            PrepareClosing();
        }

        private void PrepareCloseButton()
        {
            _closeButton = GetTemplateChild("CloseButton") as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;

                var closeButtonTextBinding = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(CloseButtonTextProperty)
                };

                var closeButtonStyle = new Binding
                {
                    Source = this,
                    Path = new PropertyPath(CloseButtonStyleProperty)
                };

                BindingOperations.SetBinding(_closeButton, ContentProperty, closeButtonTextBinding);
                BindingOperations.SetBinding(_closeButton, StyleProperty, closeButtonStyle);
            }
        }

        private async void PrepareClosing()
        {
            if (Closing != null)
            {
                var args = new ContentDialogClosingEventArgs
                {
                    // Result = result
                };

                foreach (var eventDelegate in Closing.GetInvocationList())
                {
                    var handler = (TypedEventHandler<ContentDialog, ContentDialogClosingEventArgs>)eventDelegate;
                    handler(this, args);

                    var deferral = args.GetCurrentDeferralAndReset();
                    if (deferral != null)
                    {
                        await deferral.WaitForCompletion();
                    }

                    if (args.Cancel)
                    {
                        return;
                    }
                }
            }

            UpdateShowingVisualStates();
        }

        private void PrepareDefaultButton()
        {
            if (_primaryButton != null)
            {
                _primaryButton.IsDefault = false;
            }

            if (_secondaryButton != null)
            {
                _secondaryButton.IsDefault = true;
            }

            if (_closeButton != null)
            {
                _closeButton.IsDefault = false;
            }

            var state = "NoDefaultButton";

            switch (DefaultButton)
            {
                case ContentDialogButton.None:
                    break;
                case ContentDialogButton.Primary:
                    if (_primaryButton != null)
                    {
                        _primaryButton.IsDefault = true;
                        state = "PrimaryAsDefaultButton";
                    }
                    break;
                case ContentDialogButton.Secondary:
                    if (_secondaryButton != null)
                    {
                        _secondaryButton.IsDefault = true;
                        state = "SecondaryAsDefaultButton";
                    }
                    break;
                case ContentDialogButton.Close:
                    if (_closeButton != null)
                    {
                        _closeButton.IsDefault = true;
                        state = "CloseAsDefaultButton";
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            VisualStateManager.GoToState(this, state, true);
        }

        private void PrepareOpening()
        {
            if (Opening != null)
            {
                var args = new ContentDialogOpeningEventArgs();
                foreach (var eventDelegate in Opening.GetInvocationList())
                {
                    var handler = (TypedEventHandler<ContentDialog, ContentDialogOpeningEventArgs>)eventDelegate;
                    handler(this, args);
                    if (args.Cancel)
                    {
                        return;
                    }
                }
            }

            if (!_isTemplateReady)
            {
                ApplyTemplate();
            }

            if (_placement == ContentDialogPlacement.Popup)
            {
                ContentPresenter contentPresenter = null;
                if (Application.Current.MainWindow.IsLoaded)
                {
                    contentPresenter = Application.Current.MainWindow.FindDescendant<ContentPresenter>();
                    _adornerDialog = new ContentDialogAdorner(contentPresenter);
                }
                else
                {
                    Application.Current.MainWindow.Loaded += (sender, args) =>
                    {
                        contentPresenter = Application.Current.MainWindow.FindDescendant<ContentPresenter>();
                        _adornerDialog = new ContentDialogAdorner(contentPresenter);
                    };
                }

                _container.Child = null;
                _adornerDialog.Add(_layoutRoot);
            }

            _isShowing = true;

            UpdateShowingVisualStates();
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

        private void UpdateButtonVisualStates()
        {
            var isPrimaryButtonVisible = !string.IsNullOrEmpty(PrimaryButtonText);
            var isSecondaryButtonVisible = !string.IsNullOrEmpty(SecondaryButtonText);
            var isCloseButtonVisible = !string.IsNullOrEmpty(CloseButtonText);

            string state;

            if (isPrimaryButtonVisible && isSecondaryButtonVisible && isCloseButtonVisible)
            {
                state = "AllVisible";
            }
            else if (isPrimaryButtonVisible && isSecondaryButtonVisible)
            {
                state = "PrimaryAndSecondaryVisible";
            }
            else if (isPrimaryButtonVisible && isCloseButtonVisible)
            {
                state = "PrimaryAndCloseVisible";
            }
            else if (isSecondaryButtonVisible && isCloseButtonVisible)
            {
                state = "SecondaryAndCloseVisible";
            }
            else if (isPrimaryButtonVisible)
            {
                state = "PrimaryVisible";
            }
            else if (isSecondaryButtonVisible)
            {
                state = "SecondaryVisible";
            }
            else if (isCloseButtonVisible)
            {
                state = "CloseVisible";
            }
            else
            {
                state = "NoneVisible";
            }

            VisualStateManager.GoToState(this, state, true);
        }

        private void UpdateShowingVisualStates()
        {
            if (_isTemplateReady)
            {
                VisualStateManager.GoToState(this, _isShowing ? "DialogShowing" : "DialogHidden", true);
            }
        }
    }
}
