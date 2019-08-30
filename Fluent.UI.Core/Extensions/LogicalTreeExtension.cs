using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Fluent.UI.Core.Extensions
{
    public static class LogicalTreeExtension
    {
        public static FrameworkElement FindChildByName(this FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
            {
                return element;
            }

            switch (element)
            {
                case Panel panel:
                    return panel.Children.Cast<object>().Select(child => (child as FrameworkElement)?.FindChildByName(name)).FirstOrDefault(result => result != null);
                case ItemsControl control:
                    return control.Items.Cast<object>().Select(item => (item as FrameworkElement)?.FindChildByName(name)).FirstOrDefault(result => result != null);
                default:
                {
                    var result = (element.GetContentControl() as FrameworkElement)?.FindChildByName(name);
                    if (result != null)
                    {
                        return result;
                    }

                    break;
                }
            }

            return null;
        }

        public static T FindChild<T>(this FrameworkElement element) where T : FrameworkElement
        {
            switch (element)
            {
                case null:
                    return null;
                case Panel panel:
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is T frameworkElement)
                        {
                            return frameworkElement;
                        }

                        var result = (child as FrameworkElement)?.FindChild<T>();
                        if (result != null)
                        {
                            return result;
                        }
                    }

                    break;
                }
                case ItemsControl control:
                    return control.Items.Cast<object>().Select(item => (item as FrameworkElement)?.FindChild<T>()).FirstOrDefault(result => result != null);
                default:
                {
                    var content = element.GetContentControl();

                    if (content is T frameworkElement)
                    {
                        return frameworkElement;
                    }

                    var result = (content as FrameworkElement)?.FindChild<T>();
                    if (result != null)
                    {
                        return result;
                    }

                    break;
                }
            }

            return null;
        }

        public static IEnumerable<T> FindChildren<T>(this FrameworkElement element) where T : FrameworkElement
        {
            switch (element)
            {
                case null:
                    yield break;
                case Panel panel:
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is T frameworkElement)
                        {
                            yield return frameworkElement;
                        }

                        if (child is FrameworkElement childFrameworkElement)
                        {
                            foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                            {
                                yield return childOfChild;
                            }
                        }
                    }

                    break;
                }
                case ItemsControl control:
                {
                    foreach (var item in control.Items)
                    {
                        if (item is FrameworkElement childFrameworkElement)
                        {
                            foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                            {
                                yield return childOfChild;
                            }
                        }
                    }

                    break;
                }
                default:
                {
                    var content = element.GetContentControl();
                    if (content is T frameworkElement)
                    {
                        yield return frameworkElement;
                    }

                    if (content is FrameworkElement childFrameworkElement)
                    {
                        foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                        {
                            yield return childOfChild;
                        }
                    }

                    break;
                }
            }
        }

        public static FrameworkElement FindParentByName(this FrameworkElement element, string name)
        {
            while (true)
            {
                if (element == null || string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }

                if (name.Equals(element.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return element;
                }
                element = element.Parent as FrameworkElement;
            }
        }

        public static T FindParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            while (true)
            {
                switch (element?.Parent)
                {
                    case null:
                        return null;

                    case T parent:
                        return parent;
                }

                element = element.Parent as FrameworkElement;
            }
        }

        public static UIElement GetContentControl(this FrameworkElement element)
        {
            var contentPropertyName = ContentPropertySearch(element.GetType());
            if (contentPropertyName != null)
            {
                return element.GetType().GetProperty(contentPropertyName)?.GetValue(element) as UIElement;
            }

            return null;
        }

        private static string ContentPropertySearch(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var attributeData = type.GetTypeInfo().CustomAttributes.FirstOrDefault((element) => element.AttributeType == typeof(ContentPropertyAttribute));
            if (attributeData != null)
            {
                return attributeData.NamedArguments?.FirstOrDefault().TypedValue.Value as string;
            }

            return ContentPropertySearch(type.GetTypeInfo().BaseType);
        }
    }
}