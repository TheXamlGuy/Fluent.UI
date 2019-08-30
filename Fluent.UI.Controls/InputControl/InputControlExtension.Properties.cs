using System.Windows;
using System.Windows.Media;
using Fluent.UI.Core;

namespace Fluent.UI.Controls
{
    public partial class InputControlExtension : FrameworkElementExtension
    {
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description",
                typeof(object), typeof(InputControlExtension));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header",
                typeof(object), typeof(InputControlExtension),
                new PropertyMetadata(null, OnHeaderPropertyChanged));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.RegisterAttached("HeaderTemplate",
                typeof(DataTemplate), typeof(InputControlExtension),
                new PropertyMetadata(null, OnHeaderTemplatePropertyChanged));

        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.RegisterAttached("PlaceholderForeground",
                typeof(Brush), typeof(InputControlExtension));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached("PlaceholderText",
                typeof(string), typeof(InputControlExtension));

        public static object GetDescription(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(DescriptionProperty);
        }

        public static object GetHeader(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(HeaderProperty);
        }

        public static DataTemplate GetHeaderTemplate(DependencyObject dependencyObject)
        {
            return (DataTemplate) dependencyObject.GetValue(HeaderTemplateProperty);
        }

        public static Brush GetPlaceholderForeground(DependencyObject dependencyObject)
        {
            return (Brush) dependencyObject.GetValue(PlaceholderForegroundProperty);
        }

        public static string GetPlaceholderText(DependencyObject dependencyObject)
        {
            return (string) dependencyObject.GetValue(PlaceholderTextProperty);
        }

        public static void SetDescription(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(DescriptionProperty, value);
        }

        public static void SetHeader(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(HeaderProperty, value);
        }

        public static void SetHeaderTemplate(DependencyObject dependencyObject, DataTemplate value)
        {
            dependencyObject.SetValue(HeaderTemplateProperty, value);
        }

        public static void SetPlaceholderForeground(DependencyObject dependencyObject, Brush value)
        {
            dependencyObject.SetValue(PlaceholderForegroundProperty, value);
        }

        public static void SetPlaceholderText(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PlaceholderTextProperty, value);
        }
    }
}