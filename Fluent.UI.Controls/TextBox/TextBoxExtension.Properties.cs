using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public partial class TextBoxExtension : FrameworkElementExtension
    {
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description",
                typeof(object), typeof(TextBoxExtension));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header",
                typeof(object), typeof(TextBoxExtension),
                new PropertyMetadata(null, OnHeaderPropertyChanged));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.RegisterAttached("HeaderTemplate",
                typeof(DataTemplate), typeof(TextBoxExtension),
                new PropertyMetadata(null, OnHeaderTemplatePropertyChanged));

        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.RegisterAttached("PlaceholderForeground",
                typeof(Brush), typeof(TextBoxExtension));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached("PlaceholderText",
                typeof(string), typeof(TextBoxExtension));

        public static object GetDescription(DependencyObject dependencyObject) => dependencyObject.GetValue(DescriptionProperty);

        public static object GetHeader(DependencyObject dependencyObject) => dependencyObject.GetValue(HeaderProperty);

        public static DataTemplate GetHeaderTemplate(DependencyObject dependencyObject) => (DataTemplate)dependencyObject.GetValue(HeaderTemplateProperty);

        public static Brush GetPlaceholderForeground(DependencyObject dependencyObject) => (Brush)dependencyObject.GetValue(PlaceholderForegroundProperty);

        public static string GetPlaceholderText(DependencyObject dependencyObject) => (string)dependencyObject.GetValue(PlaceholderTextProperty);

        public static void SetDescription(DependencyObject dependencyObject, object value) => dependencyObject.SetValue(DescriptionProperty, value);

        public static void SetHeader(DependencyObject dependencyObject, object value) => dependencyObject.SetValue(HeaderProperty, value);

        public static void SetHeaderTemplate(DependencyObject dependencyObject, DataTemplate value) => dependencyObject.SetValue(HeaderTemplateProperty, value);

        public static void SetPlaceholderForeground(DependencyObject dependencyObject, Brush value) => dependencyObject.SetValue(PlaceholderForegroundProperty, value);

        public static void SetPlaceholderText(DependencyObject dependencyObject, string value) => dependencyObject.SetValue(PlaceholderTextProperty, value);
    }
}