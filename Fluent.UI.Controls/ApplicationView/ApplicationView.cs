using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shell;
using Fluent.UI.Core.Win32;

namespace Fluent.UI.Controls
{
    [ContentProperty("Content")]
    public class ApplicationView : Window
    {
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility),
                typeof(ApplicationViewBackButtonVisibility), typeof(ApplicationView),
                new PropertyMetadata(ApplicationViewBackButtonVisibility.Collapsed, OnBackButtonVisibilityPropertyChanged));

        public static readonly DependencyProperty ExtendViewIntoTitleBarProperty =
                    DependencyProperty.Register(nameof(ExtendViewIntoTitleBar),
                typeof(bool), typeof(ApplicationView),
                new PropertyMetadata(false, OnExtendViewIntoTitleBarPropertyChanged));

        public static readonly DependencyProperty TitleBarBackgroundColorProperty =
            DependencyProperty.Register(nameof(TitleBarBackgroundColor),
                typeof(Brush), typeof(ApplicationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarButtonBackgroundColorProperty =
            DependencyProperty.Register(nameof(TitleBarButtonBackgroundColor),
                typeof(Brush), typeof(ApplicationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarButtonForegroundColorProperty =
            DependencyProperty.Register(nameof(TitleBarButtonForegroundColor),
                typeof(Brush), typeof(ApplicationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarButtonHoverForegroundColorProperty =
            DependencyProperty.Register(nameof(TitleBarButtonHoverForegroundColor),
                typeof(Brush), typeof(ApplicationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarForegroundColorProperty =
            DependencyProperty.Register(nameof(TitleBarForegroundColor),
                typeof(Brush), typeof(ApplicationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(nameof(TitleBarHeight),
                typeof(double), typeof(ApplicationView),
                new PropertyMetadata(32d));

        private Button _backButton;
        private Button _closeButton;
        private ContentPresenter _contentPresenter;
        private bool _isInitialized;
        private Button _maximizeButton;
        private Button _minimizeButton;
        private Border _titleBar;
        private WindowChrome _windowChrome;

        public ApplicationView()
        {
            DefaultStyleKey = typeof(ApplicationView);
        }

        internal static ApplicationView Current => Application.Current.MainWindow as ApplicationView;

        public event EventHandler<ApplicationViewBackRequestedEventArgs> BackRequested;

        public ApplicationViewBackButtonVisibility BackButtonVisibility
        {
            get => (ApplicationViewBackButtonVisibility)GetValue(BackButtonVisibilityProperty);
            set => SetValue(BackButtonVisibilityProperty, value);
        }

        public bool ExtendViewIntoTitleBar
        {
            get => (bool)GetValue(ExtendViewIntoTitleBarProperty);
            set => SetValue(ExtendViewIntoTitleBarProperty, value);
        }

        public Brush TitleBarBackgroundColor
        {
            get => (Brush)GetValue(TitleBarBackgroundColorProperty);
            set => SetValue(TitleBarBackgroundColorProperty, value);
        }

        public Brush TitleBarButtonBackgroundColor
        {
            get => (Brush)GetValue(TitleBarButtonBackgroundColorProperty);
            set => SetValue(TitleBarButtonBackgroundColorProperty, value);
        }

        public Brush TitleBarButtonForegroundColor
        {
            get => (Brush)GetValue(TitleBarButtonForegroundColorProperty);
            set => SetValue(TitleBarButtonForegroundColorProperty, value);
        }

        public Brush TitleBarButtonHoverForegroundColor
        {
            get => (Brush)GetValue(TitleBarButtonHoverForegroundColorProperty);
            set => SetValue(TitleBarButtonHoverForegroundColorProperty, value);
        }

        public Brush TitleBarForegroundColor
        {
            get => (Brush)GetValue(TitleBarForegroundColorProperty);
            set => SetValue(TitleBarForegroundColorProperty, value);
        }

        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        internal PopupRoot PopupRoot;
        private StackPanel _buttonsHost;

        public override void OnApplyTemplate()
        {
            _contentPresenter = GetTemplateChild("ContentPresenter") as ContentPresenter;
            _minimizeButton = GetTemplateChild("MinimizeButton") as Button;
            _maximizeButton = GetTemplateChild("MaximizeButton") as Button;
            _closeButton = GetTemplateChild("CloseButton") as Button;
            _buttonsHost = GetTemplateChild("ButtonsHost") as StackPanel;
            _titleBar = GetTemplateChild("TitleBar") as Border;
            _backButton = GetTemplateChild("BackButton") as Button;
            PopupRoot = GetTemplateChild("PopupRoot") as PopupRoot;

            if (_minimizeButton != null)
            {
                _minimizeButton.Click -= OnMinimizeButtonClick;
                _minimizeButton.Click += OnMinimizeButtonClick;
            }

            if (_maximizeButton != null)
            {
                _maximizeButton.Click -= OnMaximizeButtonClick;
                _maximizeButton.Click += OnMaximizeButtonClick;
            }

            if (_closeButton != null)
            {
                _closeButton.Click -= OnCloseButtonClick;
                _closeButton.Click += OnCloseButtonClick;
            }

            if (_backButton != null)
            {
                _backButton.Click -= OnBackButtonClick;
                _backButton.Click += OnBackButtonClick;
            }

            MouseLeftButtonDown -= OnMouseLeftButtonDown;
            MouseLeftButtonDown += OnMouseLeftButtonDown;

            SizeChanged -= OnSizeChanged;
            SizeChanged += OnSizeChanged;

            InitializeWindow();
        }

        private static void OnBackButtonVisibilityPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as ApplicationView;
            sender?.OnBackButtonVisibilityChanged();
        }

        private static void OnExtendViewIntoTitleBarPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as ApplicationView;
            sender?.OnExtendViewIntoTitleBarChanged();
        }

        private void InitializeWindow()
        {
            _windowChrome = new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = new CornerRadius(0)
            };

            WindowChrome.SetWindowChrome(this, _windowChrome);

            SizeChanged -= OnSizeChanged;
            SizeChanged += OnSizeChanged;

            _isInitialized = true;

            OnExtendViewIntoTitleBarChanged();
            OnWindowChanged();
        }

        private void OnBackButtonClick(object o, RoutedEventArgs routedEventArgs)
        {
            BackRequested?.Invoke(this, new ApplicationViewBackRequestedEventArgs());
        }

        private void OnBackButtonVisibilityChanged()
        {
            VisualStateManager.GoToState(this, BackButtonVisibility == ApplicationViewBackButtonVisibility.Visible ? "BackButtonVisible" : "BackButtonCollapsed", true);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SystemCommands.CloseWindow(this);
        }

        private void OnExtendViewIntoTitleBarChanged()
        {
            if (!_isInitialized)
            {
                return;
            }

            _contentPresenter.Margin = ExtendViewIntoTitleBar ? new Thickness(0) : new Thickness(0, 29, 0, 0);
        }

        private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ResizeMode == ResizeMode.CanResizeWithGrip || ResizeMode == ResizeMode.CanResize)
            {
                if (WindowState == WindowState.Maximized)
                {
                    SystemCommands.RestoreWindow(this);
                }
                else
                {
                    SystemCommands.MaximizeWindow(this);
                }
            }
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(this);
            if (_titleBar.Height > 0 && point.Y < _titleBar.Height)
            {
                if (e.ClickCount == 2 && (ResizeMode == ResizeMode.CanResizeWithGrip || ResizeMode == ResizeMode.CanResize))
                {
                    OnMaximizeButtonClick(sender, e);
                }
                else
                {
                    var wpfPoint = PointToScreen(point);
                    int x = Convert.ToInt16(wpfPoint.X);
                    int y = Convert.ToInt16(wpfPoint.Y);
                    var lParam = Convert.ToInt32(Convert.ToInt32(x)) | (y << 16);

                    var windowHandle = new WindowInteropHelper(this).Handle;
                    NativeMethods.SendMessage(windowHandle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, lParam);
                }
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
           OnWindowChanged();
        }

        private void OnWindowChanged()
        {
            if (WindowState == WindowState.Maximized)
            {
                _windowChrome.GlassFrameThickness = new Thickness(0);
                _windowChrome.ResizeBorderThickness = new Thickness(0);
                _contentPresenter.Margin = new Thickness(6);
                _titleBar.Margin = new Thickness(6, 6, 6, 0);
                _buttonsHost.Margin = new Thickness(6, 6, 6, 0);
                _backButton.Margin = new Thickness(6, 6, 6, 0);
            }
            else
            {
                _windowChrome.GlassFrameThickness = new Thickness(1, 0, 0, 0);
                _windowChrome.ResizeBorderThickness = new Thickness(1);

                _contentPresenter.Margin = new Thickness(1);
                _titleBar.Margin = new Thickness(1, 1, 1, 0);
                _buttonsHost.Margin = new Thickness(1, 1, 1, 0);
                _backButton.Margin = new Thickness(1, 1, 1, 0);
            }

            PopupRoot.InvalidateArrange();
            PopupRoot.InvalidateMeasure();
            PopupRoot.UpdateLayout();
        }
    }
}