using System.Windows;

namespace Fluent.UI.Controls
{
    public class FontIcon : IconElement
    {
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph),
                typeof(string), typeof(FontIcon),
                new PropertyMetadata(null));

        public FontIcon()
        {
            DefaultStyleKey = typeof(FontIcon);
        }

        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }
    }
}
