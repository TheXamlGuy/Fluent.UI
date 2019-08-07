using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    [ContentProperty("Content")]
    public class SplitView : Control
    {
        public static readonly DependencyProperty CompactPaneLengthProperty =
            DependencyProperty.Register(nameof(CompactPaneLength),
                typeof(double), typeof(SplitView), 
                new PropertyMetadata(0d, OnPaneLengthChanged));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content),
                typeof(UIElement), typeof(SplitView), 
                new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(nameof(DisplayMode),
                typeof(SplitViewDisplayMode), typeof(SplitView),
                new PropertyMetadata(SplitViewDisplayMode.Overlay, OnStateChanged));

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register(nameof(IsPaneOpen), typeof(bool),
                typeof(SplitView), 
                new PropertyMetadata(false, OnIsPaneOpenChanged));

        public static readonly DependencyProperty LightDismissOverlayModeProperty =
            DependencyProperty.Register(nameof(LightDismissOverlayMode),
                typeof(LightDismissOverlayMode), typeof(SplitView),
                new PropertyMetadata(LightDismissOverlayMode.Auto));

        public static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register(nameof(OpenPaneLength),
                typeof(double), typeof(SplitView),
                new PropertyMetadata(0d, OnPaneLengthChanged));

        public static readonly DependencyProperty PaneBackgroundProperty =
            DependencyProperty.Register(nameof(PaneBackground),
                typeof(Brush), typeof(SplitView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty PanePlacementProperty =
            DependencyProperty.Register(nameof(PanePlacement),
                typeof(SplitViewPanePlacement), typeof(SplitView),
                new PropertyMetadata(SplitViewPanePlacement.Left, OnStateChanged));

        public static readonly DependencyProperty PaneProperty =
            DependencyProperty.Register(nameof(Pane), 
                typeof(UIElement), typeof(SplitView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TemplateSettingsProperty =
            DependencyProperty.Register(nameof(TemplateSettings),
                typeof(SplitViewTemplateSettings), typeof(SplitView),
                new PropertyMetadata(null));

        private RectangleGeometry _paneClipRectangle;

        public SplitView()
        {
            DefaultStyleKey = typeof(SplitView);
            TemplateSettings = new SplitViewTemplateSettings(this);

            Loaded -= OnLoaded;
            Loaded += OnLoaded;
        }

        public event TypedEventHandler<SplitView, object> PaneClosed;
        public event TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs> PaneClosing;

        public double CompactPaneLength
        {
            get => (double)GetValue(CompactPaneLengthProperty);
            set => SetValue(CompactPaneLengthProperty, value);
        }

        public UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public SplitViewDisplayMode DisplayMode
        {
            get => (SplitViewDisplayMode)GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        public bool IsPaneOpen
        {
            get => (bool)GetValue(IsPaneOpenProperty);
            set => SetValue(IsPaneOpenProperty, value);
        }

        public LightDismissOverlayMode LightDismissOverlayMode
        {
            get => (LightDismissOverlayMode)GetValue(LightDismissOverlayModeProperty);
            set => SetValue(LightDismissOverlayModeProperty, value);
        }

        public double OpenPaneLength
        {
            get => (double)GetValue(OpenPaneLengthProperty);
            set => SetValue(OpenPaneLengthProperty, value);
        }

        public UIElement Pane
        {
            get => (UIElement)GetValue(PaneProperty);
            set => SetValue(PaneProperty, value);
        }

        public Brush PaneBackground
        {
            get => (Brush)GetValue(PaneBackgroundProperty);
            set => SetValue(PaneBackgroundProperty, value);
        }

        public SplitViewPanePlacement PanePlacement
        {
            get => (SplitViewPanePlacement)GetValue(PanePlacementProperty);
            set => SetValue(PanePlacementProperty, value);
        }

        public SplitViewTemplateSettings TemplateSettings
        {
            get => (SplitViewTemplateSettings)GetValue(TemplateSettingsProperty);
            private set => SetValue(TemplateSettingsProperty, value);
        }

        public override void OnApplyTemplate()
        {
            _paneClipRectangle = GetTemplateChild("PaneClipRectangle") as RectangleGeometry;

            if (GetTemplateChild("LightDismissLayer") is UIElement lightDismissLayer)
            {
                lightDismissLayer.PreviewTouchDown += OnLightDismiss;
                lightDismissLayer.PreviewStylusDown += OnLightDismiss;
                lightDismissLayer.PreviewMouseDown += OnLightDismiss;
            }

            TemplateSettings.SetBindings();
            OnVisualStateChanged();
        }

        internal DependencyObject InternalGetTemplateChild(string childName)
        {
            return GetTemplateChild(childName);
        }

        private void OnIsPaneOpenChanged()
        {
            if (IsPaneOpen)
            {
                OnVisualStateChanged();
            }
            else
            {
                if (PaneClosing != null)
                {
                    var args = new SplitViewPaneClosingEventArgs();
                    foreach (var eventDelegate in PaneClosing.GetInvocationList())
                    {
                        var handler = (TypedEventHandler<SplitView, SplitViewPaneClosingEventArgs>)eventDelegate;
                        handler(this, args);
                        if (args.Cancel)
                        {
                            IsPaneOpen = true;
                            return;
                        }
                    }
                }

                OnVisualStateChanged();
                PaneClosed?.Invoke(this, null);
            }
        }

        private void OnLightDismiss()
        {
            if (LightDismissOverlayMode == LightDismissOverlayMode.On || LightDismissOverlayMode == LightDismissOverlayMode.Auto)
            {
                if (IsPaneOpen && DisplayMode != SplitViewDisplayMode.Inline)
                    IsPaneOpen = false;
            }
        }

        private void OnVisualStateChanged(bool animated = true)
        {
            if (_paneClipRectangle != null)
                _paneClipRectangle.Rect = new Rect(0, 0, OpenPaneLength, short.MaxValue);

            var state = string.Empty;
            if (IsPaneOpen)
            {
                state += "Open";
                switch (DisplayMode)
                {
                    case SplitViewDisplayMode.CompactInline:
                        state += "Inline";
                        break;

                    default:
                        state += DisplayMode.ToString();
                        break;
                }

                state += PanePlacement.ToString();
            }
            else
            {
                state += "Closed";
                if (DisplayMode == SplitViewDisplayMode.CompactInline ||
                    DisplayMode == SplitViewDisplayMode.CompactOverlay)
                {
                    state += "Compact";
                    state += PanePlacement.ToString();
                }
            }

            VisualStateManager.GoToState(this, state, animated);
        }

        private static void OnIsPaneOpenChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var splitView = dependencyObject as SplitView;
            splitView?.OnIsPaneOpenChanged();
        }

        private static void OnPaneLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var sender = dependencyObject as SplitView;
            sender?.OnPaneLengthChanged();
        }

        private void OnPaneLengthChanged()
        {
            TemplateSettings?.Update();
        }

        private static void OnStateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var sender = dependencyObject as SplitView;
            sender?.OnVisualStateChanged();
        }

        private void OnLightDismiss(object sender, MouseButtonEventArgs e)
        {
            OnLightDismiss();
        }

        private void OnLightDismiss(object sender, StylusDownEventArgs e)
        {
            OnLightDismiss();
        }

        private void OnLightDismiss(object sender, TouchEventArgs e)
        {
            OnLightDismiss();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            TemplateSettings.Update();
            OnVisualStateChanged(false);
        }
    }
}