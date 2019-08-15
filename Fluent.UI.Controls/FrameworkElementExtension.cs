using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Controls
{
    public abstract partial class FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> : DependencyObject where TFrameworkElement : FrameworkElement where TFrameworkElementExtension : FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>, new()
    {
        private static readonly object _themeRequestLock = new object();

        protected TFrameworkElement AttachedFrameworkElement;

        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

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
            var extension = GetAttachedFrameworkElement(frameworkElement);
            if (extension != null)
            {
                return extension as TFrameworkElementExtension;
            }

            if (!frameworkElement.IsThemeRequestSupported())
            {
                return null;
            }

            extension = new TFrameworkElementExtension();

            SetIsAttached(frameworkElement, true);
            SetAttachedFrameworkElement(frameworkElement, extension);

            extension.PrepareAttachedControl(frameworkElement);
            return extension as TFrameworkElementExtension;
        }

        private void OnUnloaded(object sender, RoutedEventArgs args) => UnregisterEvents();

        private void PrepareAttachedControl(FrameworkElement frameworkElement)
        {
            AttachedFrameworkElement = frameworkElement as TFrameworkElement;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        internal void PropagateRequestedTheme(ElementTheme requestedTheme)
        {
            ApplyRequestedTheme(requestedTheme);
        }

        private void PrepareRequestedTheme()
        {
            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
            var requestedTheme = GetRequestedTheme(AttachedFrameworkElement);

            var isRequestedTheme = GetIsRequestedTheme(AttachedFrameworkElement);
            if (!isRequestedTheme && (int)requestedTheme == (int)requestedApplicationTheme)
            {
                return;
            }

            ApplyRequestedTheme(requestedTheme);
        }

        private void ApplyRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Panel))
                {
                    if (AttachedFrameworkElement is Panel panel)
                    {
                        foreach (FrameworkElement child in panel.Children)
                        {
                            //if (TryAttachFrameworkElement(child, out TFrameworkElementExtension extension))
                            //{
                            //    extension.PropagateRequestedTheme(requestedTheme);
                            //}
                        }
                    }
                }

                if (supportedType == typeof(Control))
                {
                    if (TryFindThemeResources(requestedTheme, out Dictionary<object, object> fromKeys, out Dictionary<object, object> toKeys))
                    {
                        var root = AttachedFrameworkElement.FindDescendant<Panel>();
                        if (root == null)
                        {
                            return;
                        }

                        lock (_themeRequestLock)
                        {
                            var visualStateCollection = root.FindVisualStateGroups();
                            var keyFrames = visualStateCollection.FindKeyFrames();
                            foreach (var keyFrame in keyFrames)
                            {
                                if (keyFrame is DiscreteObjectKeyFrame objectKeyFrame)
                                {
                                    var from = fromKeys.FirstOrDefault(x => x.Value.ToString() == objectKeyFrame.Value.ToString());
                                    if (from.Key != null)
                                    {
                                        var to = toKeys[from.Key];
                                        objectKeyFrame.Value = to;
                                    }
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

                                    if (from.Key != null)
                                    {
                                        var to = toKeys[from.Key];
                                        property.SetValue(AttachedFrameworkElement, to, null);
                                    }
                                }
                            }
                        }
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
