using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Controls
{
    public class ApplicationViewButton : Button
    {
        public static readonly DependencyProperty HoverForegroundColorProperty =
            DependencyProperty.Register(nameof(HoverForegroundColor),
                typeof(Brush), typeof(ApplicationViewButton),
                new PropertyMetadata(null));

        public ApplicationViewButton()
        {
            DefaultStyleKey = typeof(ApplicationViewButton);
        }

        public Brush HoverForegroundColor
        {
            get => (Brush)GetValue(HoverForegroundColorProperty);
            set => SetValue(HoverForegroundColorProperty, value);
        }

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("HoverForegroundColorTemplate") is DiscreteObjectKeyFrame hoverForegroundColorTemplate)
            {
                var binding = new Binding
                {
                    Path = new PropertyPath(HoverForegroundColorProperty),
                    Source = this
                };

                BindingOperations.SetBinding(hoverForegroundColorTemplate,  ObjectKeyFrame.ValueProperty, binding);
            }
        }
    }
}
