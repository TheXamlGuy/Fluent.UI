using System.Windows;

namespace Fluent.UI.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button.Click -= Button_Click;
            Foo.Children.Clear();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            XamlContentDialog.ShowAsync();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Button.Click -= Button_Click;
            Foo.Children.Clear();
        }
    }
}