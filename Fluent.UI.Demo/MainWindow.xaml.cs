using Fluent.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fluent.UI.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Button1.Click += Button_Click;
            Button2.Click += Button2_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new ContentDialog();
            contentDialog.ShowAsync();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogXAML.ShowAsync();
        }
    }
}
