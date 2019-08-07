using System.Windows;
using System.Windows.Media;

namespace Fluent.UI.Controls
{
    public class PathIcon : IconElement
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data),
                typeof(Geometry), typeof(PathIcon),
                new PropertyMetadata(null));

        public PathIcon()
        {
            DefaultStyleKey = typeof(PathIcon);
        }

        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
