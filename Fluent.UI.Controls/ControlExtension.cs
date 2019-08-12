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
    public abstract class ControlExtension<TControl, TControlExtension> : DependencyObject where TControl : Control where TControlExtension : ControlExtension<TControl, TControlExtension>, new()
    {
        public static readonly DependencyProperty IsAttachedProperty =
           DependencyProperty.RegisterAttached("IsAttached",
               typeof(bool), typeof(ControlExtension<TControl, TControlExtension>),
               new PropertyMetadata(false, OnIsExtendedPropertyChanged));

        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ElementTheme), typeof(ControlExtension<TControl, TControlExtension>),
                new PropertyMetadata((ElementTheme)(int)ApplicationExtension.RequestedTheme, OnRequestedThemePropertyChanged));

        internal static DependencyProperty AttachedControlProperty =
            DependencyProperty.RegisterAttached("AttachedControl",
              typeof(ControlExtension<TControl, TControlExtension>), typeof(ControlExtension<TControl, TControlExtension>));

        internal static DependencyProperty IsRequestedThemeProperty =
            DependencyProperty.RegisterAttached("IsRequestedTheme",
                typeof(bool), typeof(ControlExtension<TControl, TControlExtension>));

        protected TControl AttachedControl;

        private DependencyPropertyChangedHandler _dependencyPropertyChangedHandler;

        public static bool GetIsAttached(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsAttachedProperty);

        public static bool GetIsRequestedTheme(DependencyObject dependencyObject) => (bool)dependencyObject.GetValue(IsRequestedThemeProperty);

        public static ElementTheme GetRequestedTheme(TControl control) => (ElementTheme)control.GetValue(RequestedThemeProperty);

        public static void SetIsAttached(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsAttachedProperty, value);

        public static void SetIsRequestedTheme(DependencyObject dependencyObject, bool value) => dependencyObject.SetValue(IsRequestedThemeProperty, value);
        public static void SetRequestedTheme(TControl control, ElementTheme value)
        {
            var attachedControl = GetAttachedControl(control);
            if (attachedControl != null)
            {

            }

            control.SetValue(IsRequestedThemeProperty, true);
            control.SetValue(RequestedThemeProperty, value);
        }

        internal static ControlExtension<TControl, TControlExtension> GetAttachedControl(TControl control) => (ControlExtension<TControl, TControlExtension>)control.GetValue(AttachedControlProperty);

        protected virtual void ChangeVisualState(bool useTransitions = true)
        {

        }

        protected virtual void DependencyPropertyChangedHandler(DependencyPropertyChangedHandler handler)
        {

        }

        protected void GoToVisualState(string stateName, bool useTransitions = true) => VisualStateManager.GoToState(AttachedControl, stateName, useTransitions);

        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            PrepareRequestedTheme();
        }

        private static void ClearAttachedControl(TControl control)
        {
            var extensionBase = control.GetValue(AttachedControlProperty) as ControlExtension<TControl, TControlExtension>;
            extensionBase?.RemoveAttachedControl();

            control.ClearValue(AttachedControlProperty);
        }

        private static void OnIsExtendedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((bool)args.NewValue != (bool)args.OldValue)
            {
                if ((bool)args.NewValue)
                {
                    SetAttachedControl(dependencyObject as TControl, new TControlExtension());
                }
                else
                {
                    ClearAttachedControl(dependencyObject as TControl);
                }
            }
        }

        private static void OnRequestedThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if ((ElementTheme)args.NewValue != (ElementTheme)args.OldValue)
            {
                SetRequestedTheme((TControl)dependencyObject, (ElementTheme)args.NewValue);
            }
        }

        private static void SetAttachedControl(TControl control, ControlExtension<TControl, TControlExtension> extensionBase)
        {
            control.SetValue(AttachedControlProperty, extensionBase);
            extensionBase.SetAttachedControl(control);
        }

        private IEnumerable<object> FindKeyFrames(Collection<VisualStateGroup> visualStateGroups)
        {
            foreach (var timeline in visualStateGroups.Select(vsg => vsg.States.Cast<VisualState>().Where(x => x.Storyboard != null)).SelectMany(visualStates => visualStates.SelectMany(sb => sb.Storyboard.Children)))
            {
                if (timeline is IKeyFrameAnimation keyFrameAnimation)
                {
                    foreach (var keyFrame in keyFrameAnimation.KeyFrames)
                    {
                        yield return keyFrame;
                    }
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            UnregisterEvents();
        }

        private void PrepareRequestedTheme()
        {
            var requestedApplicationTheme = ApplicationExtension.RequestedTheme;
            var requestedTheme = GetRequestedTheme(AttachedControl);
            if ((int)requestedTheme == (int)requestedApplicationTheme)
            {
                return;
            }

            var isRequestedTheme = GetIsRequestedTheme(AttachedControl);
            if (isRequestedTheme)
            {
                if (TryFindThemeResources(out Dictionary<object, object> fromKeys, out Dictionary<object, object> toKeys))
                {
                    var root = AttachedControl.FindDescendant<Panel>();
                    if (root == null)
                    {
                        return;
                    }

                    var visualStateGroups = (Collection<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(root);
                    if (visualStateGroups == null)
                    {
                        return;
                    }

                    var keyFrames = FindKeyFrames(visualStateGroups);
                    foreach (var keyFrame in keyFrames)
                    {
                        if (keyFrame is DiscreteObjectKeyFrame objectKeyFrame)
                        {
                            var from = fromKeys.FirstOrDefault(x => x.Value.ToString() == objectKeyFrame.Value.ToString());
                            var to = toKeys[from.Key];
                            objectKeyFrame.Value = to;
                        }
                    }

                    var properties = AttachedControl.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(Brush))
                        {
                            var propertyValue = property.GetValue(AttachedControl, null);
                            if (propertyValue == null)
                            {
                                return;
                            }

                            var from = fromKeys.FirstOrDefault(x => x.Value.ToString() == propertyValue.ToString());
                            var to = toKeys[from.Key];

                            property.SetValue(AttachedControl, to, null);
                        }
                    }
                }
            }
        }
        private void RegisterEvents()
        {
            AttachedControl.Unloaded += OnUnloaded;
            AttachedControl.Loaded += OnLoaded;
        }

        private void RemoveAttachedControl()
        {
            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler.Clear();
            _dependencyPropertyChangedHandler = null;

            AttachedControl = null;
        }

        private void SetAttachedControl(TControl control)
        {
            AttachedControl = control;

            UnregisterEvents();
            RegisterEvents();

            _dependencyPropertyChangedHandler = new DependencyPropertyChangedHandler();
            DependencyPropertyChangedHandler(_dependencyPropertyChangedHandler);
        }

        private bool TryFindThemeResources(out Dictionary<object, object> fromKeys, out Dictionary<object, object> toKeys)
        {
            var lightTheme = new Uri($@"Fluent.UI.Controls;component/Button/Button.Light.xaml", UriKind.Relative);
            var defaultTheme = new Uri($@"Fluent.UI.Controls;component/Button/Button.Default.xaml", UriKind.Relative);

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

            foreach (DictionaryEntry item in lightThemeResource.Cast<DictionaryEntry>())
            {
                fromKeys[item.Key] = item.Value;
            }

            foreach (DictionaryEntry item in defaultThemeResource.Cast<DictionaryEntry>())
            {
                toKeys[item.Key] = item.Value;
            }

            return true;
        }
        private void UnregisterEvents()
        {
            AttachedControl.Unloaded -= OnUnloaded;
            AttachedControl.Loaded -= OnLoaded;
        }
    }
}
