using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class NavigationView : ContentControl
    {
        public static readonly DependencyProperty AttachedCommandBarProperty =
            DependencyProperty.RegisterAttached("AttachedCommandBar",
                typeof(CommandBar), typeof(NavigationView),
                new FrameworkPropertyMetadata(null, OnAttachedPropertyChanged));

        public static readonly DependencyProperty AttachedHeaderProperty =
            DependencyProperty.RegisterAttached("AttachedHeader",
                typeof(NavigationViewHeader), typeof(NavigationView),
                new PropertyMetadata(null, OnAttachedPropertyChanged));

        public static readonly DependencyProperty CommandBarProperty =
            DependencyProperty.Register(nameof(CommandBar),
                typeof(CommandBar), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CompactPaneLengthProperty =
            DependencyProperty.Register(nameof(CompactPaneLength),
                typeof(double), typeof(NavigationView),
                new PropertyMetadata((double) 48));

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(nameof(HeaderBackground),
                typeof(Brush), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header),
                typeof(object), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate),
                typeof(DataTemplate), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsPaneOpenProperty =
            DependencyProperty.Register(nameof(IsPaneOpen),
                typeof(bool), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsPaneToggleButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsPaneToggleButtonVisible),
                typeof(bool), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsSettingsVisibleProperty =
            DependencyProperty.Register(nameof(IsSettingsVisible),
                typeof(bool), typeof(NavigationView),
                new PropertyMetadata(true, OnIsSettingsVisiblePropertyChanged));

        public static readonly DependencyProperty IsViewExtendedIntoTitleBarProperty =
            DependencyProperty.Register(nameof(IsViewExtendedIntoTitleBar),
                typeof(bool), typeof(NavigationView),
                new PropertyMetadata(false, OnIsViewExtendedIntoTitleBarPropertyChanged));

        public static readonly DependencyProperty MenuItemContainerStyleProperty =
            DependencyProperty.Register(nameof(MenuItemContainerStyle),
                typeof(Style), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemContainerStyleSelectorProperty =
            DependencyProperty.Register(nameof(MenuItemContainerStyleSelector),
                typeof(StyleSelector), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems),
                typeof(ObservableCollection<object>), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemsSourceProperty =
            DependencyProperty.Register(nameof(MenuItemsSource),
                typeof(object), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemTemplateProperty =
            DependencyProperty.Register(nameof(MenuItemTemplate),
                typeof(DataTemplate), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MenuItemTemplateSelectorProperty =
            DependencyProperty.Register(nameof(MenuItemTemplateSelector),
                typeof(Style), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty OpenPaneLengthProperty =
            DependencyProperty.Register(nameof(OpenPaneLength),
                typeof(double), typeof(NavigationView),
                new PropertyMetadata((double)320));

        public static readonly DependencyProperty PaneFooterProperty =
            DependencyProperty.Register(nameof(PaneFooter),
                typeof(UIElement), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty PaneHeaderProperty =
            DependencyProperty.Register(nameof(PaneHeader),
                typeof(UIElement), typeof(NavigationView),
                new PropertyMetadata(null, OnPaneHeaderPropertyChanged));

        public static readonly DependencyProperty PaneToggleButtonStyleProperty =
            DependencyProperty.Register(nameof(PaneToggleButtonStyle),
                typeof(Style), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem),
                typeof(object), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SettingsContentProperty =
            DependencyProperty.Register(nameof(SettingsContent),
                typeof(object), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SettingsIconProperty =
            DependencyProperty.Register(nameof(SettingsIcon),
                typeof(IconElement), typeof(NavigationView),
                new PropertyMetadata(null));


        public static readonly DependencyProperty SettingsPageNameProperty =
            DependencyProperty.Register(nameof(SettingsPageName),
                typeof(string), typeof(NavigationView),
                new PropertyMetadata(null));

        private static readonly DependencyPropertyKey SettingsItemPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SettingsItem),
                typeof(object), typeof(NavigationView),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SettingsItemProperty = SettingsItemPropertyKey.DependencyProperty;

        private Grid _contentPaneTopPadding;

        private ContentControl _headerContent;

        private NavigationViewList _menuItemsHost;

        private SplitView _rootSplitView;

        private NavigationViewItem _settingsNavPaneItem;

        private Button _togglePaneButton;

        public NavigationView()
        {
            DefaultStyleKey = typeof(NavigationView);
            MenuItems = new ObservableCollection<object>();

            SizeChanged += OnSizeChanged;
        }

        public event TypedEventHandler<NavigationView, NavigationViewItemInvokedEventArgs> ItemInvoked;

        public event TypedEventHandler<NavigationView, NavigationViewSelectionChangedEventArgs> SelectionChanged;

        public CommandBar CommandBar
        {
            get => (CommandBar)GetValue(CommandBarProperty);
            set => SetValue(CommandBarProperty, value);
        }

        public double CompactPaneLength
        {
            get => (double)GetValue(CompactPaneLengthProperty);
            set => SetValue(CompactPaneLengthProperty, value);
        }

        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public Brush HeaderBackground
        {
            get => (Brush)GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public bool IsPaneOpen
        {
            get => (bool)GetValue(IsPaneOpenProperty);
            set => SetValue(IsPaneOpenProperty, value);
        }

        public bool IsPaneToggleButtonVisible
        {
            get => (bool)GetValue(IsPaneToggleButtonVisibleProperty);
            set => SetValue(IsPaneToggleButtonVisibleProperty, value);
        }

        public bool IsSettingsVisible
        {
            get => (bool)GetValue(IsSettingsVisibleProperty);
            set => SetValue(IsSettingsVisibleProperty, value);
        }

        public bool IsViewExtendedIntoTitleBar
        {
            get => (bool)GetValue(IsViewExtendedIntoTitleBarProperty);
            set => SetValue(IsViewExtendedIntoTitleBarProperty, value);
        }

        public Style MenuItemContainerStyle
        {
            get => (Style)GetValue(MenuItemContainerStyleProperty);
            set => SetValue(MenuItemContainerStyleProperty, value);
        }

        public StyleSelector MenuItemContainerStyleSelector
        {
            get => (StyleSelector)GetValue(MenuItemContainerStyleSelectorProperty);
            set => SetValue(MenuItemContainerStyleSelectorProperty, value);
        }

        public ObservableCollection<object> MenuItems
        {
            get => (ObservableCollection<object>)GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public object MenuItemsSource
        {
            get => GetValue(MenuItemsSourceProperty);
            set => SetValue(MenuItemsSourceProperty, value);
        }

        public DataTemplate MenuItemTemplate
        {
            get => (DataTemplate)GetValue(MenuItemTemplateProperty);
            set => SetValue(MenuItemTemplateProperty, value);
        }

        public DataTemplateSelector MenuItemTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(MenuItemTemplateSelectorProperty);
            set => SetValue(MenuItemTemplateSelectorProperty, value);
        }

        public double OpenPaneLength
        {
            get => (double)GetValue(OpenPaneLengthProperty);
            set => SetValue(OpenPaneLengthProperty, value);
        }

        public UIElement PaneFooter
        {
            get => (UIElement)GetValue(PaneFooterProperty);
            set => SetValue(PaneFooterProperty, value);
        }

        public UIElement PaneHeader
        {
            get => (UIElement)GetValue(PaneHeaderProperty);
            set => SetValue(PaneHeaderProperty, value);
        }

        public Style PaneToggleButtonStyle
        {
            get => (Style)GetValue(PaneToggleButtonStyleProperty);
            set => SetValue(PaneToggleButtonStyleProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public object SettingsContent
        {
            get => GetValue(SettingsContentProperty);
            set => SetValue(SettingsContentProperty, value);
        }

        public IconElement SettingsIcon
        {
            get => (IconElement)GetValue(SettingsItemProperty);
            set => SetValue(SettingsItemProperty, value);
        }

        public object SettingsItem
        {
            get => GetValue(SettingsItemProperty);
            protected set => SetValue(SettingsItemPropertyKey, value);
        }

        public string SettingsPageName
        {
            get => (string)GetValue(SettingsPageNameProperty);
            set => SetValue(SettingsPageNameProperty, value);
        }

        public static CommandBar GetAttachedCommandBar(DependencyObject dependencyObject)
        {
            return (CommandBar)dependencyObject.GetValue(AttachedCommandBarProperty);
        }

        public static NavigationViewHeader GetAttachedHeader(DependencyObject dependencyObject)
        {
            return (NavigationViewHeader)dependencyObject.GetValue(AttachedHeaderProperty);
        }

        public static void SetAttachedCommandBar(DependencyObject dependencyObject, CommandBar commandBar)
        {
            dependencyObject.SetValue(AttachedCommandBarProperty, commandBar);
        }

        public static void SetAttachedHeader(DependencyObject dependencyObject, NavigationViewHeader value)
        {
            dependencyObject.SetValue(AttachedHeaderProperty, value);
        }

        public override void OnApplyTemplate()
        {
            _headerContent = GetTemplateChild("HeaderContent") as ContentControl;
            _togglePaneButton = GetTemplateChild("TogglePaneButton") as Button;
            _rootSplitView = GetTemplateChild("RootSplitView") as SplitView;
            _contentPaneTopPadding = GetTemplateChild("ContentPaneTopPadding") as Grid;

            if (_togglePaneButton != null)
            {
                _togglePaneButton.Click -= OnTogglePaneButtonClick;
                _togglePaneButton.Click += OnTogglePaneButtonClick;
            }

            _menuItemsHost = GetTemplateChild("MenuItemsHost") as NavigationViewList;
            if (_menuItemsHost != null)
            {
                _menuItemsHost.MouseUp -= OnMenuItemsHostClick;
                _menuItemsHost.MouseUp += OnMenuItemsHostClick;

                _menuItemsHost.Loaded -= OnMenuItemsHostLoaded;
                _menuItemsHost.Loaded += OnMenuItemsHostLoaded;
                SetMenuItemsHost();
            }

            _settingsNavPaneItem = GetTemplateChild("SettingsNavPaneItem") as NavigationViewItem;
            if (_settingsNavPaneItem != null)
            {
                SettingsItem = _settingsNavPaneItem;

                _settingsNavPaneItem.MouseUp -= OnSettingsNavPaneItemMouseUp;
                _settingsNavPaneItem.MouseUp += OnSettingsNavPaneItemMouseUp;

                _settingsNavPaneItem.Selected -= OnSettingsNavPaneItemSelected;
                _settingsNavPaneItem.Selected += OnSettingsNavPaneItemSelected;
            }

            OnIsViewExtendedIntoTitleBarChanged();
            PrepareSettingsVisualState();
            PreparePaneHeader();
        }

        private static void OnAttachedFrameworkElementElementLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetAttachedProperty(sender as FrameworkElement);
        }

        private static void OnAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is FrameworkElement sender)
            {
                OnAttachedPropertyChanged(sender);
            }
        }

        private static void OnAttachedPropertyChanged(FrameworkElement frameworkElement)
        {
            if (!frameworkElement.IsLoaded)
            {
                frameworkElement.Loaded -= OnAttachedFrameworkElementElementLoaded;
                frameworkElement.Loaded += OnAttachedFrameworkElementElementLoaded;
            }
            else
            {
                SetAttachedProperty(frameworkElement);
            }
        }

        private static void OnIsSettingsVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as NavigationView;
            sender?.OnIsSettingsVisiblePropertyChanged();
        }

        private static void OnIsViewExtendedIntoTitleBarPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as NavigationView;
            sender?.OnIsViewExtendedIntoTitleBarChanged();
        }

        private static void OnPaneHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var navigationView = dependencyObject as NavigationView;
            navigationView?.OnPaneHeaderPropertyChanged();
        }

        private static void SetAttachedProperty(DependencyObject dependencyObject)
        {
            var navigationView = dependencyObject.FindAscendant<NavigationView>();
            if (navigationView != null)
            {
                var commandBar = GetAttachedCommandBar(dependencyObject);
                if (commandBar != null)
                {
                    navigationView.CommandBar = commandBar;
                }

                var header = GetAttachedHeader(dependencyObject);
                if (header != null)
                {
                    if (header.HeaderTemplate == null)
                    {
                        navigationView.ClearValue(HeaderTemplateProperty);
                        navigationView.SetValue(HeaderProperty, header.Header);
                    }
                    else
                    {
                        navigationView.SetValue(HeaderTemplateProperty, header.HeaderTemplate);
                        navigationView.SetValue(HeaderProperty, header.Header);
                    }
                }
            }
        }

        private void InvokeItem(object item, bool isSettingsInvoked = false)
        {
            ItemInvoked?.Invoke(this, new NavigationViewItemInvokedEventArgs(item, isSettingsInvoked));
        }

        private void InvokeSelectionChanged(object item, bool isSettingsSelected = false)
        {
            SelectionChanged?.Invoke(this, new NavigationViewSelectionChangedEventArgs(item, isSettingsSelected));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in args.NewItems)
                    {
                        _menuItemsHost.Items.Add(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in args.OldItems)
                    {
                        _menuItemsHost.Items.Remove(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnIsSettingsVisiblePropertyChanged()
        {
            PrepareSettingsVisualState();
        }

        private void OnIsViewExtendedIntoTitleBarChanged()
        {
            PrepareIsViewExtendedIntoTitleBarVisualState();
        }

        private void OnMenuItemsHostClick(object sender, MouseButtonEventArgs args)
        {
            var item = (sender as ItemsControl).GetListBoxItem(args.OriginalSource as DependencyObject);
            if (item != null)
            {
                _settingsNavPaneItem.IsSelected = false;
                InvokeItem(item);
            }
        }

        private void OnMenuItemsHostLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            PrepareSelectionChanged();
        }

        private void OnPaneHeaderPropertyChanged()
        {
            PreparePaneHeader();
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var item = _menuItemsHost.SelectedItem;
            if (item != null)
            {
                SelectedItem = item;

                _settingsNavPaneItem.IsSelected = false;
                InvokeSelectionChanged(item);
            }
        }

        private void OnSettingsNavPaneItemMouseUp(object sender, MouseButtonEventArgs args)
        {
            var item = SettingsItem;
            if (item != null)
            {
                SelectedItem = item;

                InvokeItem(item, true);
                InvokeSelectionChanged(item, true);
            }
        }

        private void OnSettingsNavPaneItemSelected(object o, RoutedEventArgs routedEventArgs)
        {
            _menuItemsHost.SelectedItem = null;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (_rootSplitView == null)
            {
                return;
            }

            string state;
            if (args.NewSize.Width > 1008)
            {
                _rootSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                IsPaneOpen = true;

                state = "Expanded";
            }
            else if (args.NewSize.Width > 641)
            {
                _rootSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                IsPaneOpen = false;

                state = "Compact";
            }
            else
            {
                _rootSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                IsPaneOpen = false;

                state = "Minimal";
            }

            VisualStateManager.GoToState(this, state, false);
        }

        private void OnTogglePaneButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private void PrepareIsViewExtendedIntoTitleBarVisualState()
        {
            VisualStateManager.GoToState(this, IsViewExtendedIntoTitleBar ? "TitleBarCollapsed" : "TitleBarVisible", true);
            if (_headerContent != null)
            {
                _headerContent.Height = IsViewExtendedIntoTitleBar ? 80 : 0;
            }
        }

        private void PreparePaneHeader()
        {
            if (_contentPaneTopPadding == null)
            {
                return;
            }

            var binding = new Binding
            {
                Source = PaneHeader,
                Path = new PropertyPath(ActualHeightProperty)
            };

            BindingOperations.SetBinding(_contentPaneTopPadding, HeightProperty, binding);
        }

        private void PrepareSelectionChanged()
        {
            _menuItemsHost.SelectionChanged -= OnSelectionChanged;
            _menuItemsHost.SelectionChanged += OnSelectionChanged;

            InvokeSelectionChanged(_menuItemsHost.SelectedItem);
        }

        private void PrepareSettingsVisualState()
        {
            VisualStateManager.GoToState(this, IsSettingsVisible ? "SettingsVisible" : "SettingsCollapsed", true);
        }

        private void SetMenuItemsHost()
        {
            foreach (var item in MenuItems)
            {
                _menuItemsHost.Items.Add(item);
            }

            ((INotifyCollectionChanged)MenuItems).CollectionChanged += OnCollectionChanged;
        }
    }
}
