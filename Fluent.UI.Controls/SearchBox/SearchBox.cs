using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public class SearchBox : TextBox
    {
        public static DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(nameof(PlaceholderText),
                typeof(string), typeof(SearchBox),
                new PropertyMetadata(null));

        private Button _deleteButton;

        public SearchBox()
        {
            DefaultStyleKey = typeof(SearchBox);
        }

        public event TypedEventHandler<SearchBox, SearchBoxQueryClearedEventArgs> QueryCleared;
        public event TypedEventHandler<SearchBox, SearchBoxQuerySubmittedEventArgs> QuerySubmitted;

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public override void OnApplyTemplate()
        {
            _deleteButton = GetTemplateChild("DeleteButton") as Button;
            if (_deleteButton != null)
            {
                _deleteButton.Click += OnClick;
            }

            base.OnApplyTemplate();
        }

        protected override void OnKeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
            {
                RaiseQuerySubmitted();
            }
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            RaiseQueryCleared();
        }

        private void RaiseQueryCleared()
        {
            QueryCleared?.Invoke(this, new SearchBoxQueryClearedEventArgs());
        }

        private void RaiseQuerySubmitted()
        {
            QuerySubmitted?.Invoke(this, new SearchBoxQuerySubmittedEventArgs(Text));
        }
    }
}