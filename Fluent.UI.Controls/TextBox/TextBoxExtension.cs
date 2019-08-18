using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public partial class TextBoxExtension : FrameworkElementExtension<TextBox>
    {
        private static void OnHeaderPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => OnHeaderPropertyChanged(dependencyObject as TextBox);

        private static void OnHeaderPropertyChanged(TextBox text)
        {
            //if (!text.IsLoaded)
            //{
            //    return;
            //}

            //var extension = GetAttachedFrameworkElement(text) as TextBoxExtension;
            //extension.ChangeHeaderVisualState();
        }

        private static void OnHeaderTemplatePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) => OnHeaderPropertyChanged(dependencyObject as TextBox);
    }
}
