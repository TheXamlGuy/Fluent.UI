using System.Windows;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class NavigationViewCommandBar : CommandBar
    {
        public static DependencyProperty IsSearchBoxVisibleProperty =
            DependencyProperty.Register(nameof(IsSearchBoxVisible),
                typeof(bool), typeof(NavigationViewCommandBar),
                new PropertyMetadata(true, OnIsSearchBoxVisiblePropertyChanged));

        public static DependencyProperty SearchBoxPlaceholderTextProperty =
            DependencyProperty.Register(nameof(SearchBoxPlaceholderText),
                typeof(string), typeof(NavigationViewCommandBar),
                new PropertyMetadata(null));

        public static DependencyProperty SearchBoxQueryTextProperty =
            DependencyProperty.Register(nameof(SearchBoxQueryText),
                typeof(string), typeof(NavigationViewCommandBar),
                new PropertyMetadata(null));

        private SearchBox _searchBox;
        public NavigationViewCommandBar()
        {
            DefaultStyleKey = typeof(NavigationViewCommandBar);
        }

        public event TypedEventHandler<NavigationViewCommandBar, SearchBoxQueryClearedEventArgs> QueryCleared;

        public event TypedEventHandler<NavigationViewCommandBar, SearchBoxQuerySubmittedEventArgs> QuerySubmitted;
        public bool IsSearchBoxVisible
        {
            get => (bool)GetValue(IsSearchBoxVisibleProperty);
            set => SetValue(IsSearchBoxVisibleProperty, value);
        }

        public string SearchBoxPlaceholderText
        {
            get => (string)GetValue(SearchBoxPlaceholderTextProperty);
            set => SetValue(SearchBoxPlaceholderTextProperty, value);
        }

        public string SearchBoxQueryText
        {
            get => (string)GetValue(SearchBoxQueryTextProperty);
            set => SetValue(SearchBoxQueryTextProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _searchBox = GetTemplateChild("SearchBox") as SearchBox;
            if (_searchBox != null)
            {
                _searchBox.QuerySubmitted -= OnQuerySubmitted;
                _searchBox.QuerySubmitted += OnQuerySubmitted;

                _searchBox.QueryCleared -= OnQueryCleared;
                _searchBox.QueryCleared += OnQueryCleared;
            }

            OnSearchBoxVisualStatesChanged();
        }

        private static void OnIsSearchBoxVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var navigationViewCommandBar = dependencyObject as NavigationViewCommandBar;
            navigationViewCommandBar?.OnIsSearchBoxVisiblePropertyChanged();
        }

        private void OnIsSearchBoxVisiblePropertyChanged()
        {
            OnSearchBoxVisualStatesChanged();
        }

        private void OnQueryCleared(SearchBox sender, SearchBoxQueryClearedEventArgs args)
        {
            QueryCleared?.Invoke(this, new SearchBoxQueryClearedEventArgs());
        }
        private void OnQuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            QuerySubmitted?.Invoke(this, new SearchBoxQuerySubmittedEventArgs(args.QueryText));
        }

        private void OnSearchBoxVisualStatesChanged()
        {
            VisualStateManager.GoToState(this, IsSearchBoxVisible ? "SearchBoxVisible" : "SearchBoxCollapsed", true);
        }
    }
}
