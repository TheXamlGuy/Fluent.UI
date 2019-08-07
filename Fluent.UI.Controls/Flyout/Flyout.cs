using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Fluent.UI.Controls
{
    [ContentProperty("Content")]
    public class Flyout : FlyoutBase
    {
        public static DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content),
                typeof(UIElement), typeof(Flyout),
                new PropertyMetadata(null));

        public static DependencyProperty FlyoutPresenterStyleProperty =
            DependencyProperty.Register(nameof(FlyoutPresenterStyle),
                typeof(Style), typeof(Flyout),
                new PropertyMetadata(null));

        public UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public Style FlyoutPresenterStyle
        {
            get => (Style)GetValue(FlyoutPresenterStyleProperty);
            set => SetValue(FlyoutPresenterStyleProperty, value);
        }

        protected override Control CreatePresenter()
        {
            return new FlyoutPresenter
            {
                Content = Content
            };
        }
    }
}
