using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Controls
{
    public abstract class FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> : DependencyObject where TFrameworkElement : FrameworkElement where TFrameworkElementExtension : FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>, new()
    {
        public static readonly DependencyProperty IsAttachedProperty =
           DependencyProperty.RegisterAttached("IsAttached",
               typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>),
               new PropertyMetadata(false, OnIsAttachedPropertyChanged));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>),
                new PropertyMetadata((ElementTheme)(int)ApplicationExtension.RequestedTheme, OnRequestedThemePropertyChanged));

        internal static DependencyProperty AttachedFrameworkElementProperty =
            DependencyProperty.RegisterAttached("AttachedFrameworkElement",
              typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>));

        protected TFrameworkElement AttachedFrameworkElement;

        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        public static ElementTheme GetRequestedTheme(TFrameworkElement control) => (ElementTheme)control.GetValue(RequestedThemeProperty);

        public static void SetRequestedTheme(TFrameworkElement control, ElementTheme value)
        {
            control.SetValue(IsRequestedThemeProperty, true);
            control.SetValue(RequestedThemeProperty, value);
        }

        internal static FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> GetAttachedFrameworkElement(TFrameworkElement control) => (FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>)control.GetValue(AttachedFrameworkElementProperty);

        internal static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        internal static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);
        
        internal static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        internal static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        protected virtual void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {

        }

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedFrameworkElement, stateName, useTransitions);

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            PrepareRequestedTheme();
        }

        private static void OnIsAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue != (bool)args.OldValue)
            {
                if ((bool)args.NewValue)
                {
                    AttachFrameworkElement(dependencyObject as TFrameworkElement);
                }
                else
                {
                    DetachFrameworkElement(dependencyObject as TFrameworkElement);
                }
            }
        }

        private static void DetachFrameworkElement(TFrameworkElement frameworkElement)
        {
            var extension = GetAttachedFrameworkElement(frameworkElement);
            extension.RemoveAttachedControl();

            frameworkElement.ClearValue(AttachedFrameworkElementProperty);
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                if (TryAttachFrameworkElement(dependencyObject as TFrameworkElement, out TFrameworkElementExtension extension))
                {
                    extension?.PrepareRequestedTheme();
                }
            }
        }

        private static bool TryAttachFrameworkElement(TFrameworkElement frameworkElement, out TFrameworkElementExtension extension)
        {
            extension = AttachFrameworkElement(frameworkElement);
            if (extension == null)
            {
                return false;
            }

            return true;
        }

        private static TFrameworkElementExtension AttachFrameworkElement(TFrameworkElement frameworkElement)     
        {
            var extension = GetAttachedFrameworkElement(frameworkElement) as TFrameworkElementExtension;
            if (extension != null)
            {
                return extension;
            }

            var attachedType = frameworkElement.GetType();
            if (!(typeof(Panel).IsAssignableFrom(attachedType) || typeof(Control).IsAssignableFrom(attachedType)))
            {
                return null;
            }

            extension = new TFrameworkElementExtension();

            SetIsAttached(frameworkElement, true);
            SetAttachedFrameworkElement(frameworkElement, extension);

            extension.PrepareAttachedControl(frameworkElement);

            return extension;
        }

        private static void SetAttachedFrameworkElement(TFrameworkElement control, FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> extension) => control.SetValue(AttachedFrameworkElementProperty, extension);


        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
        }

        private void PrepareAttachedControl(TFrameworkElement control)
        {
            AttachedFrameworkElement = control;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        private void PrepareRequestedTheme()
        {
            var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
            var requestedTheme = GetRequestedTheme(AttachedFrameworkElement);

            var isRequestedTheme = GetIsRequestedTheme(AttachedFrameworkElement);
            if (!isRequestedTheme && (int)requestedTheme == (int)requestedApplicationTheme)
            {
                return;
            }

            var frameworkElementType = AttachedFrameworkElement.GetType();

            if (TryFindThemeResources(requestedTheme, out Dictionary<object, object> fromKeys, out Dictionary<object, object> toKeys))
            {
                var root = AttachedFrameworkElement.FindDescendant<Panel>();
                if (root == null)
                {
                    return;
                }

                var visualStateCollection = root.FindVisualStateGroups();
                var keyFrames = visualStateCollection.FindKeyFrames();
                foreach (var keyFrame in keyFrames)
                {
                    if (keyFrame is DiscreteObjectKeyFrame objectKeyFrame)
                    {
                        var from = fromKeys.FirstOrDefault(x => x.Value.ToString() == objectKeyFrame.Value.ToString());
                        var to = toKeys[from.Key];
                        objectKeyFrame.Value = to;
                    }
                }

                var properties = AttachedFrameworkElement.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(Brush))
                    {
                        var propertyValue = property.GetValue(AttachedFrameworkElement, null);
                        if (propertyValue == null)
                        {
                            return;
                        }

                        var from = fromKeys.FirstOrDefault(x => x.Value.ToString() == propertyValue.ToString());
                        var to = toKeys[from.Key];

                        property.SetValue(AttachedFrameworkElement, to, null);
                    }
                }
            }
        }

        private void RegisterEvents()
        {
            AttachedFrameworkElement.Unloaded += OnUnloaded;
            AttachedFrameworkElement.Loaded += OnLoaded;
        }

        private void RemoveAttachedControl()
        {
            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            AttachedFrameworkElement = null;
        }

        private bool TryFindThemeResources(ElementTheme requestedTheme, out Dictionary<object, object> fromKeys, out Dictionary<object, object> toKeys)
        {
            var typeName = AttachedFrameworkElement.GetType().Name;

            var lightTheme = new Uri($@"Fluent.UI.Controls;component/{typeName}/{typeName}.Light.xaml", UriKind.Relative);
            var defaultTheme = new Uri($@"Fluent.UI.Controls;component/{typeName}/{typeName}.Default.xaml", UriKind.Relative);

            fromKeys = new Dictionary<object, object>();
            toKeys = new Dictionary<object, object>();

            ResourceDictionary lightThemeResource;
            ResourceDictionary defaultThemeResource;
            try
            {
                lightThemeResource = Application.LoadComponent(lightTheme) as ResourceDictionary;
                defaultThemeResource = Application.LoadComponent(defaultTheme) as ResourceDictionary;
            }
            catch
            {
                return false;
            }

            foreach (DictionaryEntry item in requestedTheme == ElementTheme.Light ? defaultThemeResource.Cast<DictionaryEntry>() : lightThemeResource.Cast<DictionaryEntry>())
            {
                fromKeys[item.Key] = item.Value;
            }

            foreach (DictionaryEntry item in requestedTheme == ElementTheme.Light ? lightThemeResource.Cast<DictionaryEntry>() : defaultThemeResource.Cast<DictionaryEntry>())
            {
                toKeys[item.Key] = item.Value;
            }

            return true;
        }

        private void UnregisterEvents()
        {
            AttachedFrameworkElement.Unloaded -= OnUnloaded;
            AttachedFrameworkElement.Loaded -= OnLoaded;
        }
    }
}
