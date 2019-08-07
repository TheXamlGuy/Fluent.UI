using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Fluent.UI.Controls
{
    [ContentProperty("Inlines")]
    public class HeaderedTextBlock : Control
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header),
                typeof(string), typeof(HeaderedTextBlock),
                new PropertyMetadata(null, OnHeaderPropertyChanged));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate),
                typeof(DataTemplate), typeof(HeaderedTextBlock),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HideTextIfEmptyProperty =
            DependencyProperty.Register(nameof(HideTextIfEmpty),
                typeof(bool), typeof(HeaderedTextBlock),
                new PropertyMetadata(false, OnHideTextIfEmptyPropertyChanged));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
                typeof(Orientation), typeof(HeaderedTextBlock),
                new PropertyMetadata(Orientation.Vertical, OnOrientationPropertyChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text),
                typeof(string), typeof(HeaderedTextBlock),
                new PropertyMetadata(null, OnTextPropertyChanged));

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register(nameof(TextStyle),
                typeof(Style), typeof(HeaderedTextBlock),
                new PropertyMetadata(null));

        private ContentPresenter _headerContentPresenter;

        private TextBlock _textContent;

        public HeaderedTextBlock()
        {
            DefaultStyleKey = typeof(HeaderedTextBlock);
            Inlines = new ObservableCollection<Inline>();
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public bool HideTextIfEmpty
        {
            get => (bool)GetValue(HideTextIfEmptyProperty);
            set => SetValue(HideTextIfEmptyProperty, value);
        }

        public ObservableCollection<Inline> Inlines { get; }

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            _textContent = GetTemplateChild("TextContent") as TextBlock;

            UpdateVisibility();

            foreach (var inline in Inlines)
            {
                _textContent.Inlines.Add(inline);
            }

            UpdateForOrientation(Orientation);
        }

        private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var headeredTextBlock = dependencyObject as HeaderedTextBlock;
            headeredTextBlock?.UpdateVisibility();
        }

        private static void OnHideTextIfEmptyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var headeredTextBlock = dependencyObject as HeaderedTextBlock;
            headeredTextBlock?.UpdateVisibility();
        }

        private static void OnOrientationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var headeredTextBlock = dependencyObject as HeaderedTextBlock;
            headeredTextBlock?.OnOrientationPropertyChanged();
        }

        private static void OnTextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var headeredTextBlock = dependencyObject as HeaderedTextBlock;
            headeredTextBlock?.UpdateVisibility();
        }

        private void OnOrientationPropertyChanged()
        {
            UpdateForOrientation(Orientation);
        }
        private void UpdateForOrientation(Orientation orientationValue)
        {
            switch (orientationValue)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, "Vertical", true);
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, "Horizontal", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientationValue), orientationValue, null);
            }
        }

        private void UpdateVisibility()
        {
            if (_headerContentPresenter != null)
            {
                _headerContentPresenter.Visibility = _headerContentPresenter.Content == null
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }

            if (_textContent != null)
            {
                _textContent.Visibility = string.IsNullOrWhiteSpace(_textContent.Text) && HideTextIfEmpty
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }
    }
}