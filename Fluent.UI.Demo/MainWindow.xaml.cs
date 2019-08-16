using Fluent.UI.Controls;
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

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            Border.Background = Brushes.Black;
            ButtonExtension.SetRequestedTheme(Button, ElementTheme.Dark);
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            Border.Background = Brushes.Transparent;
            ButtonExtension.SetRequestedTheme(Button, ElementTheme.Light);
        }
    }
}
