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
        ContentDialog d;
        public MainWindow()
        {
            InitializeComponent();
            d = new ContentDialog();

            Button.Click += Button_Click;
            d.PrimaryButtonText = "Primary button";
            d.SecondaryButtonText = "Secondary button";
            d.CloseButtonText = "Close button";


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            d.ShowAsync();
        }
    }
}
