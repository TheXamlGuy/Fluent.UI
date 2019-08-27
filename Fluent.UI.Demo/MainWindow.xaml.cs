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
            Button.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Foo.Children.Clear();
        }

        private void OnThemeChecked(object sender, RoutedEventArgs args)
        {
            Button.Click -= Button_Click;

            RootBorder.Background = Brushes.Black;
            FrameworkElementExtension.SetRequestedTheme(RootBorder, ElementTheme.Dark);
        }
    }
}