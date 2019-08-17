using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Fluent.UI.Controls
{
    public class FrameworkElementExtension : FrameworkElementExtension<FrameworkElement, FrameworkElementExtension>
    {

    }

    public abstract partial class FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension> : DependencyObject where TFrameworkElement : FrameworkElement where TFrameworkElementExtension : FrameworkElementExtension<TFrameworkElement, TFrameworkElementExtension>, new()
    {
        protected TFrameworkElement AttachedFrameworkElement;
        private static readonly object _themeRequestLock = new object();
        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        internal void PropagateRequestedTheme(ElementTheme requestedTheme)
        {
            ApplyRequestedTheme(requestedTheme);
        }

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

        private static TFrameworkElementExtension AttachFrameworkElement(FrameworkElement frameworkElement)
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

            SetAttachedFrameworkElement(frameworkElement, extension);
            SetIsAttached(frameworkElement, true);

            extension.PrepareAttachedControl(frameworkElement);
            return extension as TFrameworkElementExtension;
        }

        private static void DetachFrameworkElement(TFrameworkElement frameworkElement)
        {
            var extension = GetAttachedFrameworkElement(frameworkElement);
            extension.RemoveAttachedControl();

            frameworkElement.ClearValue(AttachedFrameworkElementProperty);
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

        private static void OnRequestedThemePropagatedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                if (TryAttachFrameworkElement(dependencyObject as FrameworkElement, out TFrameworkElementExtension extension))
                {
                    extension?.PrepareRequestedTheme();
                }
            }
        }

        private static bool TryAttachFrameworkElement(FrameworkElement frameworkElement, out TFrameworkElementExtension extension)
        {
            extension = AttachFrameworkElement(frameworkElement);
            return extension != null;
        }

        private void SetChildThemeRequest(UIElement frameworkElement, ElementTheme requestedTheme)
        {
            if (!GetIsAttached(frameworkElement))
            {
                FrameworkElementExtension.SetIsAttached(frameworkElement, true);
            }

            FrameworkElementExtension.SetRequestedThemePropagated(frameworkElement, requestedTheme);
        }

        private void ApplyRequestedTheme(ElementTheme requestedTheme)
        {
            if (AttachedFrameworkElement.TryIsThemeRequestSupported(out Type supportedType))
            {
                if (supportedType == typeof(Panel) && AttachedFrameworkElement is Panel panel)
                {
                    foreach (FrameworkElement child in panel.Children)
                    {
                        SetChildThemeRequest(child, requestedTheme);
                    }
                }

                if (supportedType == typeof(Decorator) && AttachedFrameworkElement is Decorator decorator)
                {
                    SetChildThemeRequest(decorator.Child, requestedTheme);
                }

                if (supportedType == typeof(Control))
                {
                    var elementType = AttachedFrameworkElement.GetType();
                    var extensionType = this.GetType();
                    var elementTypeName = elementType.Name;
                    var extensionTypeNamespace = extensionType.Namespace;
                    var requestedThemeName = (requestedTheme == ElementTheme.Default || requestedTheme == ElementTheme.Dark) ? "Default" : "Light";

                    var themeResource = new Uri($@"pack://application:,,,/{extensionTypeNamespace};component/{elementTypeName}/{elementTypeName}.{requestedThemeName}.xaml", UriKind.Absolute);
                    var resourceDictionary = new SharedResourceDictionary { Source = themeResource };

                    var style = resourceDictionary[typeof(Button)] as Style;
                    AttachedFrameworkElement.SetValue(FrameworkElement.StyleProperty, style);
                }
            }
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

        private void PrepareRequestedTheme()
        {
            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            ElementTheme requestedTheme;

            var isRequestedThemePropagated = GetIsRequestedThemePropagated(AttachedFrameworkElement);
            if (isRequestedThemePropagated)
            {
                requestedTheme = GetRequestedThemePropagated(AttachedFrameworkElement);
            }
            else
            {
                var isRequestedTheme = GetIsRequestedTheme(AttachedFrameworkElement);
                if (!isRequestedTheme)
                {
                    return;
                }

                var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
                requestedTheme = GetRequestedTheme(AttachedFrameworkElement);
                if (!isRequestedTheme && ((int)requestedTheme == (int)requestedApplicationTheme))
                {
                    return;
                }
            }

            ApplyRequestedTheme(requestedTheme);
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

        private void UnregisterEvents()
        {
            AttachedFrameworkElement.Unloaded -= OnUnloaded;
            AttachedFrameworkElement.Loaded -= OnLoaded;
        }
    }
}
