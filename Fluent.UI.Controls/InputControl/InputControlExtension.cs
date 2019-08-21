using Fluent.UI.Core;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public partial class InputControlExtension : FrameworkElementExtension
    {
        private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (TryAttachTemplate(dependencyObject as TextBox, out AttachedTextBoxTemplate attachedTemplate))
            {
                attachedTemplate?.SetHeader((object)args.NewValue);
            }
        }

        private static void OnHeaderTemplatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (TryAttachTemplate(dependencyObject as TextBox, out AttachedTextBoxTemplate attachedTemplate))
            {
                attachedTemplate?.SetHeaderTemplate((DataTemplate)args.NewValue);
            }
        }
    }
}