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
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Switch to dark theme";
            BorderExtension.SetRequestedTheme(Border1, ElementTheme.Dark);
            Border1.Background = Brushes.Black;
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox1.Content = "Switch to light theme";
            BorderExtension.SetRequestedTheme(Border1, ElementTheme.Light);
            Border1.Background = Brushes.Transparent;
        }
    }
}
