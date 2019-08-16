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
            BorderExtension.SetRequestedTheme(Border, ElementTheme.Dark);
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            Border.Background = Brushes.Transparent;
            BorderExtension.SetRequestedTheme(Border, ElementTheme.Light);
        }
    }
}
