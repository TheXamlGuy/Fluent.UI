using Fluent.UI.Controls;
using System.Windows;

namespace Fluent.UI.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Button1.Click += Button_Click;
            Button2.Click += Button2_Click;
            Button3.Click += Button3_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new ContentDialog();
            contentDialog.PrimaryButtonText = "sf";
            contentDialog.ShowAsync();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogXAML.ShowAsync();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogInPlace.ShowAsync(ContentDialogPlacement.InPlace);
        }
    }
}
