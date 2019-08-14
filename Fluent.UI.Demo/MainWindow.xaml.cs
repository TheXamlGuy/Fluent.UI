using Fluent.UI.Controls;
using Fluent.UI.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Button2.Click += Button2_Click;
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

        private void D_Checked(object sender, RoutedEventArgs e)
        {
            d.Content = "Switch to dark theme";

            foreach (var d in ThemeBackground.Children)
            {
                ButtonExtension.SetRequestedTheme(d as Button, ElementTheme.Dark);
            }
            ThemeBackground.Background = Brushes.Black;

        }

        private void D_Unchecked(object sender, RoutedEventArgs e)
        {
            d.Content = "Switch to light theme";
            foreach (var d in ThemeBackground.Children)
            {
                ButtonExtension.SetRequestedTheme(d as Button, ElementTheme.Light);
            }
            ThemeBackground.Background = Brushes.Transparent;
        }
    }
}
