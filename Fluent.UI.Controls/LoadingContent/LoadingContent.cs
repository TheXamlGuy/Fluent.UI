using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class LoadingContent : ContentControl
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading),
                typeof(bool), typeof(LoadingContent),
                new PropertyMetadata(false, OnIsLoadingPropertyChanged));

        public LoadingContent()
        {
            DefaultStyleKey = typeof(LoadingContent);
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public override void OnApplyTemplate()
        {
            OnChangeVisualStates();
        }

        private static void OnIsLoadingPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var sender = dependencyObject as LoadingContent;
            sender?.OnIsLoadingPropertyChanged();
        }

        private void OnChangeVisualStates()
        {
            VisualStateManager.GoToState(this, IsLoading ? "LoadingIn" : "LoadingOut", true);
        }

        private void OnIsLoadingPropertyChanged()
        {
            OnChangeVisualStates();
        }
    }
}