using System;
using System.Linq;
using System.Reflection;
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
                new PropertyMetadata(ElementTheme.Light, OnRequestedThemePropertyChanged));

        internal static readonly DependencyProperty IsRequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("IsRequestedThemePropagated",
                typeof(bool), typeof(FrameworkElementExtension));

        internal static readonly DependencyProperty RequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("RequestedThemePropagated",
                typeof(ElementTheme), typeof(FrameworkElementExtension),
                new PropertyMetadata(ElementTheme.Default, OnRequestedThemePropagatedPropertyChanged));

        internal static DependencyProperty AttachedTemplatetProperty =
            DependencyProperty.RegisterAttached("AttachedTemplate",
              typeof(IAttachedFrameworkElementTemplate), typeof(FrameworkElementExtension));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(FrameworkElementExtension));

        public static ElementTheme GetRequestedTheme(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemeProperty);

        public static ElementTheme GetRequestedThemePropagated(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemePropagatedProperty);

        internal static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        public static void SetRequestedTheme(DependencyObject dependencyObject, ElementTheme value)
        {
            dependencyObject.SetValue(IsRequestedThemeProperty, true);
            dependencyObject.SetValue(RequestedThemeProperty, value);
        }

        public static void SetRequestedThemePropagated(DependencyObject dependencyObject, ElementTheme value)
        {
            dependencyObject.SetValue(IsRequestedThemePropagatedProperty, true);
            dependencyObject.SetValue(RequestedThemePropagatedProperty, value);
        }

        public static IAttachedFrameworkElementTemplate GetAttachedTemplate(DependencyObject dependencyObject) => (IAttachedFrameworkElementTemplate)dependencyObject.GetValue(AttachedTemplatetProperty);

        public static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        internal static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);

        internal static bool GetIsRequestedThemePropagated(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemePropagatedProperty);

        internal static void SetAttachedTemplate(DependencyObject dependencyObject, IAttachedFrameworkElementTemplate extension) => dependencyObject.SetValue(AttachedTemplatetProperty, extension);

        internal static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);

        internal static void SetIsRequestedThemePropagated(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemePropagatedProperty, value);

        internal static IAttachedFrameworkElementTemplate AttachTemplate(FrameworkElement frameworkElement)
        {
            var attachedTemplate = GetAttachedTemplate(frameworkElement);
            if (attachedTemplate != null)
            {
                return attachedTemplate;
            }

            var frameworkElementType = frameworkElement.GetType();

            var assemblyType = Type.GetType("Fluent.UI.Controls.ButtonExtension, Fluent.UI.Controls");
            var extensionType = Type.GetType("Fluent.UI.Core.FrameworkElementExtension, Fluent.UI.Core");

            var attachedTemplateType = Assembly.GetAssembly(assemblyType).GetTypes().FirstOrDefault(x => typeof(IAttachedFrameworkElementTemplate<>).MakeGenericType(frameworkElementType).IsAssignableFrom(x));

            if (attachedTemplateType != null)
            {
                attachedTemplate = Activator.CreateInstance(attachedTemplateType) as IAttachedFrameworkElementTemplate;
            }
            else
            {
                attachedTemplate = Activator.CreateInstance(typeof(AttachedFrameworkElementTemplate<>).MakeGenericType(frameworkElementType)) as IAttachedFrameworkElementTemplate;
            }

            SetAttachedTemplate(frameworkElement, attachedTemplate);
            attachedTemplate.SetAttachedControl(frameworkElement);

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

        private static void OnRequestedThemePropagatedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                if (TryAttachTemplate(dependencyObject as FrameworkElement, out IAttachedFrameworkElementTemplate attachedTemplate))
                {
                    attachedTemplate?.SetRequestedThemePropagated((ElementTheme)args.NewValue);
                }
            }
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                if (TryAttachTemplate(dependencyObject as FrameworkElement, out IAttachedFrameworkElementTemplate attachedTemplate))
                {
                    attachedTemplate?.SetRequestedTheme((ElementTheme)args.NewValue);
                }
            }
        }
    }
}