using Fluent.UI.Core;
using Fluent.UI.Core.Extensions;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Fluent.UI.Controls
{
    public class FrameworkElementExtensionHandler<TFrameworkElement> : IFrameworkExtensionHandler<TFrameworkElement> where TFrameworkElement : FrameworkElement
    {
        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        private bool _isRequestedTheme;
        private bool _isRequestedThemePropagated;
        private ElementTheme _requestedTheme;
        private ElementTheme _requestedThemePropagated;
        protected TFrameworkElement AttachedFrameworkElement { get; private set; }
        protected bool IsLoaded { get; private set; }

        public void SetAttachedControl(FrameworkElement frameworkElement)
        {
            AttachedFrameworkElement = frameworkElement as TFrameworkElement;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        public void SetRequestedTheme(ElementTheme requestedTheme)
        {
            _isRequestedThemePropagated = false;
            _isRequestedTheme = true;
            _requestedTheme = requestedTheme;

            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            PrepareRequestedTheme();
        }

        public void SetRequestedThemePropagated(ElementTheme requestedTheme)
        {
            _isRequestedThemePropagated = true;
            _requestedThemePropagated = requestedTheme;

            if (!AttachedFrameworkElement.IsLoaded)
            {
                return;
            }

            PrepareRequestedTheme();
        }

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

        protected TTemplateChild GetTemplateChild<TTemplateChild>(string name) where TTemplateChild : FrameworkElement => AttachedFrameworkElement.FindDescendantByName(name) as TTemplateChild;

        protected void GoToVisualState(string stateName, bool useTransitions = true)
        {

            var foo =VisualStateManager.GoToState(AttachedFrameworkElement, stateName, useTransitions);
            Debug.WriteLine(stateName + " = " + foo);
        }

        protected virtual void OnApplyTemplate()
        {

        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            OnApplyTemplate();
            ChangeVisualState(false);
            PrepareRequestedTheme();

            IsLoaded = true;
        }

        protected virtual void OnUnloaded()
        {

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
                    var extensionType = GetType();
                    var elementTypeName = elementType.Name;
                    var extensionTypeNamespace = extensionType.Namespace;
                    var requestedThemeName = (requestedTheme == ElementTheme.Default || requestedTheme == ElementTheme.Dark) ? "Default" : "Light";

                    var themeResource = new Uri($@"pack://application:,,,/{extensionTypeNamespace};component/{elementTypeName}/{elementTypeName}.{requestedThemeName}.xaml", UriKind.Absolute);
                    var resourceDictionary = new SharedResourceDictionary { Source = themeResource };

                    var style = resourceDictionary[elementType] as Style;

                    AttachedFrameworkElement.Style = style;
                    AttachedFrameworkElement.UpdateLayout();
                    
                    ChangeVisualState(true);
                    OnApplyTemplate();
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
            OnUnloaded();
        }

        private void PrepareRequestedTheme()
        {
            ElementTheme requestedTheme;
            if (_isRequestedThemePropagated)
            {
                requestedTheme = _requestedThemePropagated;
            }
            else
            {
                if (!_isRequestedTheme)
                {
                    return;
                }

                var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
                if (!_isRequestedTheme && ((int)_requestedTheme == (int)requestedApplicationTheme))
                {
                    return;
                }
                requestedTheme = _requestedTheme;
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
            OnUnloaded();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            AttachedFrameworkElement = null;
        }

        private void SetChildThemeRequest(UIElement frameworkElement, ElementTheme requestedTheme)
        {
            var foo = typeof(FrameworkElementExtension<>).MakeGenericType(frameworkElement.GetType());

            MethodInfo myStaticMethodInfo = foo.GetMethod("SetRequestedThemePropagated");
            myStaticMethodInfo.Invoke(null, new object[] { frameworkElement, requestedTheme });
        }

        private void UnregisterEvents()
        {
            AttachedFrameworkElement.Unloaded -= OnUnloaded;
            AttachedFrameworkElement.Loaded -= OnLoaded;
        }
    }
}
