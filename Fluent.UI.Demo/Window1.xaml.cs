using Fluent.UI.Controls;
using System.Windows;

namespace Fluent.UI.Demo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            ContentDialog dss = new ContentDialog();
        }
    }
}