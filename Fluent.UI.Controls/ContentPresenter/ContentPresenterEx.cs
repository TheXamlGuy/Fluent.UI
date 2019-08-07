using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Fluent.UI.Core.Extensions;

namespace Fluent.UI.Controls
{
    public class ContentPresenterEx : ContentPresenter
    {
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(nameof(Foreground),
                typeof(Brush), typeof(ContentPresenterEx),
                new PropertyMetadata(null));

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public ContentPresenterEx()
        {
            Loaded += OnLoaded;
        }

        private DependencyProperty GetDependencyPropertyByName(Type dependencyObjectType, string dpName)
        {
            DependencyProperty dependencyProperty = null;
            var fieldInfo = dependencyObjectType.GetField(dpName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (fieldInfo != null)
                dependencyProperty = fieldInfo.GetValue(null) as DependencyProperty;
            return dependencyProperty;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dependencyObjects = this.FindDescendants<FrameworkElement>();

            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Foreground"),
                Mode = BindingMode.TwoWay
            };

            foreach (var dependencyObject in dependencyObjects)
            {
                var foregroundProperty = dependencyObject.GetType().GetProperty("Foreground");
                if (foregroundProperty != null)
                {
                    BindingOperations.SetBinding(dependencyObject, GetDependencyPropertyByName(dependencyObject.GetType(), "ForegroundProperty"), binding);
                }
            }
        }
    }
}
