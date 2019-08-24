using Fluent.UI.Core;
using System.Windows;
using System.Windows.Media;

namespace Fluent.UI.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnThemeChecked(object sender, RoutedEventArgs args)
        {
            RootBorder.Background = Brushes.Black;
            FrameworkElementExtension.SetRequestedTheme(RootBorder, ElementTheme.Dark);
        }
    }
}