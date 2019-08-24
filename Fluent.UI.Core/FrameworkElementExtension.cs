using Fluent.UI.Core.Extensions;
using System.Windows;

namespace Fluent.UI.Core
{
    public abstract class FrameworkElementExtension
    {
        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached("IsAttached",
                typeof(bool), typeof(FrameworkElementExtension),
                new PropertyMetadata(false, OnIsAttachedPropertyChanged));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(FrameworkElementExtension),
                new PropertyMetadata(ElementTheme.Default, OnRequestedThemePropertyChanged));

        internal static DependencyProperty AttachedTemplatetProperty =
            DependencyProperty.RegisterAttached("AttachedTemplate",
              typeof(IAttachedFrameworkElementTemplate), typeof(FrameworkElementExtension));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(FrameworkElementExtension));

        public static ElementTheme GetRequestedTheme(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemeProperty);

        internal static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        public static void SetRequestedTheme(DependencyObject dependencyObject, ElementTheme value)
        {
            dependencyObject.SetValue(IsRequestedThemeProperty, true);
            dependencyObject.SetValue(RequestedThemeProperty, value);
        }

        public static IAttachedFrameworkElementTemplate GetAttachedTemplate(DependencyObject dependencyObject) => (IAttachedFrameworkElementTemplate)dependencyObject.GetValue(AttachedTemplatetProperty);

        public static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        internal static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);

        internal static void SetAttachedTemplate(DependencyObject dependencyObject, IAttachedFrameworkElementTemplate extension) => dependencyObject.SetValue(AttachedTemplatetProperty, extension);

        internal static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);

        internal static IAttachedFrameworkElementTemplate AttachTemplate(FrameworkElement source)
        {
            var attachedTemplate = GetAttachedTemplate(source);
            if (attachedTemplate != null)
            {
                return attachedTemplate;
            }

            attachedTemplate = AttachedFrameworkElementTemplateFactory.Current.Create(source);

            SetAttachedTemplate(source, attachedTemplate);
            attachedTemplate.SetAttachedControl(source);

            return attachedTemplate;
        }

        protected static bool TryAttachTemplate(FrameworkElement frameworkElement, out IAttachedFrameworkElementTemplate extension)
        {
            extension = AttachTemplate(frameworkElement);
            return extension != null;
        }

        protected static bool TryAttachTemplate<TAttachedTemplate>(FrameworkElement frameworkElement, out TAttachedTemplate attachedTemplate) where TAttachedTemplate : IAttachedFrameworkElementTemplate
        {
            attachedTemplate = (TAttachedTemplate)AttachTemplate(frameworkElement);
            return attachedTemplate != null;
        }

        //protected static void DetachFrameworkElement(TFrameworkElement frameworkElement)
        //{
        //    //var extension = GetAttachedFrameworkElement(frameworkElement);
        //    //extension.RemoveAttachedControl();

        //    //frameworkElement.ClearValue(AttachedFrameworkElementProperty);
        //}

        private static void OnIsAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue != (bool)args.OldValue)
            {
                if ((bool)args.NewValue)
                {
                    AttachTemplate(dependencyObject as FrameworkElement);
                }
                else
                {
                }
            }
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                RequestedThemeMessageBus.Current.Publish(dependencyObject as FrameworkElement, new RequestedThemeEventArgs((ElementTheme)args.NewValue));
            }
        }
    }
}