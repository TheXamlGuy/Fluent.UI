using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Fluent.UI.Controls
{
    public abstract class FrameworkElementExtension<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached("IsAttached",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement>),
                new PropertyMetadata(false, OnIsAttachedPropertyChanged));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(FrameworkElementExtension<TFrameworkElement>),
                new PropertyMetadata(ElementTheme.Light, OnRequestedThemePropertyChanged));

        internal static readonly DependencyProperty IsRequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("IsRequestedThemePropagated",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement>));

        internal static readonly DependencyProperty RequestedThemePropagatedProperty =
            DependencyProperty.RegisterAttached("RequestedThemePropagated",
                typeof(ElementTheme), typeof(FrameworkElementExtension<TFrameworkElement>),
                new PropertyMetadata(ElementTheme.Default, OnRequestedThemePropagatedPropertyChanged));

        internal static DependencyProperty AttachedHandlerProperty =
            DependencyProperty.RegisterAttached("AttachedHandler",
              typeof(IFrameworkExtensionHandler), typeof(FrameworkElementExtension<TFrameworkElement>));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement>));

        public static ElementTheme GetRequestedTheme(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemeProperty);

        public static ElementTheme GetRequestedThemePropagated(DependencyObject dependencyObject) => (ElementTheme)dependencyObject.GetValue(RequestedThemePropagatedProperty);

        public static void SetIsAttached(TFrameworkElement dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

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

        internal static IFrameworkExtensionHandler GetAttachedHandler(DependencyObject dependencyObject) => (IFrameworkExtensionHandler)dependencyObject.GetValue(AttachedHandlerProperty);

        internal static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        internal static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);

        internal static bool GetIsRequestedThemePropagated(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemePropagatedProperty);

        internal static void SetAttachedHandler(DependencyObject dependencyObject, IFrameworkExtensionHandler extension) => dependencyObject.SetValue(AttachedHandlerProperty, extension);

        internal static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);
        
        internal static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);

        internal static void SetIsRequestedThemePropagated(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemePropagatedProperty, value);

        private static IFrameworkExtensionHandler AttachHandler(FrameworkElement frameworkElement)
        {
            var handler = GetAttachedHandler(frameworkElement);
            if (handler != null)
            {
                return handler;
            }

            var frameworkElementType = frameworkElement.GetType();
            var extensionType = Type.GetType("Fluent.UI.Controls.FrameworkElementExtension`1, Fluent.UI.Controls");
            var handlerType = Assembly.GetAssembly(extensionType).GetTypes().FirstOrDefault(x => typeof(IFrameworkExtensionHandler<>).MakeGenericType(frameworkElementType).IsAssignableFrom(x));

            if (handlerType != null)
            {
                handler = Activator.CreateInstance(handlerType) as IFrameworkExtensionHandler;
            }
            else
            {
                handler = Activator.CreateInstance(typeof(FrameworkElementExtensionHandler<>).MakeGenericType(frameworkElementType)) as IFrameworkExtensionHandler;
            }

            SetAttachedHandler(frameworkElement, handler);
            handler.SetAttachedControl(frameworkElement);

            return handler;
        }

        private static void DetachFrameworkElement(TFrameworkElement frameworkElement)
        {
            //var extension = GetAttachedFrameworkElement(frameworkElement);
            //extension.RemoveAttachedControl();

            //frameworkElement.ClearValue(AttachedFrameworkElementProperty);
        }

        private static void OnIsAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue != (bool)args.OldValue)
            {
                if ((bool)args.NewValue)
                {
                    AttachHandler(dependencyObject as FrameworkElement);
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
                if (TryAttachHandler(dependencyObject as FrameworkElement, out IFrameworkExtensionHandler handler))
                {
                    handler?.SetRequestedThemePropagated((ElementTheme)args.NewValue);
                }
            }
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                if (TryAttachHandler(dependencyObject as FrameworkElement, out IFrameworkExtensionHandler handler))
                {
                    handler?.SetRequestedTheme((ElementTheme)args.NewValue);
                }
            }
        }

        private static bool TryAttachHandler(FrameworkElement frameworkElement, out IFrameworkExtensionHandler extension)
        {
            extension = AttachHandler(frameworkElement);
            return extension != null;
        }
    }
}
