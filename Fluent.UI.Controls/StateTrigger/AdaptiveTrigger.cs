using System.Windows;

namespace Fluent.UI.Controls
{
    public class AdaptiveTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty MinWindowHeightProperty =
            DependencyProperty.Register(nameof(MinWindowHeight),
                typeof(double), typeof(AdaptiveTrigger), new PropertyMetadata(0d));

        public static readonly DependencyProperty MinWindowWidthProperty =
            DependencyProperty.Register(nameof(MinWindowWidth),
                typeof(double), typeof(AdaptiveTrigger), new PropertyMetadata(0d));

        private bool _isActive;

        private Window _window;

        public AdaptiveTrigger()
        {
            if (Application.Current.MainWindow != null)
            {
                _window = Application.Current.MainWindow;
                _window.SizeChanged += OnSizeChanged;
            }
        }

        public bool IsActive
        {
            get => _isActive;
            private set
            {
                _isActive = value;
                SetActive(value);
            }
        }

        public double MinWindowHeight
        {
            get => (double) GetValue(MinWindowHeightProperty);
            set => SetValue(MinWindowHeightProperty, value);
        }

        public double MinWindowWidth
        {
            get => (double) GetValue(MinWindowWidthProperty);
            set => SetValue(MinWindowWidthProperty, value);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            IsActive = _window.Height >= MinWindowHeight && _window.Width >= MinWindowWidth;
        }
    }
}