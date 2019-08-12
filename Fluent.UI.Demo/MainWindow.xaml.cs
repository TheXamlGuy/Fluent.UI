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
            var lightTheme = new Uri($@"Fluent.UI.Controls;component/Button/Button.Light.xaml", UriKind.Relative);
            var defaultTheme = new Uri($@"Fluent.UI.Controls;component/Button/Button.Default.xaml", UriKind.Relative);

            var lightThemeResource = Application.LoadComponent(lightTheme) as ResourceDictionary;
            var defaultThemeResource = Application.LoadComponent(defaultTheme) as ResourceDictionary;

            var fromKeys = new Dictionary<object, object>();
            foreach (DictionaryEntry item in lightThemeResource.Cast<DictionaryEntry>().Where(x => x.Value.GetType().BaseType == typeof(Brush)))
            {
                fromKeys[item.Value.ToString()] = item.Key;
            }

            var toKeys = new Dictionary<object, object>();
            foreach (DictionaryEntry item in defaultThemeResource)
            {
                toKeys[item.Key] = item.Value;
            }

            var root = Button3.FindDescendant<Panel>();
            var visualStateGroups = (Collection<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(root);
            foreach (var vsg in visualStateGroups.Select(vsg => vsg.States.Cast<VisualState>().Where(x => x.Storyboard != null)))
            {
                foreach (Timeline timeline in vsg.SelectMany(vst => vst.Storyboard.Children))
                {
                    if (timeline is IKeyFrameAnimation keyFrameAnimation)
                    {
                        foreach (var dd in keyFrameAnimation.KeyFrames)
                        {
                            if (dd is DiscreteObjectKeyFrame f)
                            {
                                var from = fromKeys[f.Value.ToString()];
                                var to = toKeys[from];
                                f.Value = to;
                            }
                        }
                    }

                }
            }
        }
    }
}
