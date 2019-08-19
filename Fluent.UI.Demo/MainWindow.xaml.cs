using Fluent.UI.Controls;
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

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Switch to dark theme";
            FrameworkElementExtension.SetRequestedTheme(Border1, ElementTheme.Dark);
            Border1.Background = Brushes.Black;

            FrameworkElementExtension.SetRequestedTheme(Border2, ElementTheme.Dark);
            Border2.Background = Brushes.Black;
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Switch to light theme";
            FrameworkElementExtension.SetRequestedTheme(Border1, ElementTheme.Light);
            Border1.Background = Brushes.Transparent;
            FrameworkElementExtension.SetRequestedTheme(Border2, ElementTheme.Light);
            Border2.Background = Brushes.Transparent;
        }
    }
}
