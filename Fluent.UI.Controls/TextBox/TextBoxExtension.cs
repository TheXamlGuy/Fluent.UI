using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public partial class TextBoxExtension : FrameworkElementExtension<TextBox>
    {
        private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (TryAttachHandler(dependencyObject as TextBox, out TextBoxExtensionHandler handler))
            {
                handler?.SetHeader((object)args.NewValue);
            }
        }

        private static void OnHeaderTemplatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (TryAttachHandler(dependencyObject as TextBox, out TextBoxExtensionHandler handler))
            {
                handler?.SetHeaderTemplate((DataTemplate)args.NewValue);
            }
        }
    }
}