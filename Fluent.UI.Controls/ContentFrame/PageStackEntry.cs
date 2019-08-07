using System;
using System.Windows;

namespace Fluent.UI.Controls.ContentFrame
{
    public sealed class PageStackEntry : DependencyObject
    {
        public static DependencyProperty SourcePageTypeProperty =
            DependencyProperty.Register(nameof(SourcePageType),
                typeof(Type), typeof(PageStackEntry),
                new PropertyMetadata(null));

        public PageStackEntry(Type sourcePageType, object parameter)
        {
            SourcePageType = sourcePageType;
            Parameter = parameter;
        }
        public object Parameter { get; internal set; }

        public Type SourcePageType
        {
            get => (Type)GetValue(SourcePageTypeProperty);
            internal set => SetValue(SourcePageTypeProperty, value);
        }
    }
}
