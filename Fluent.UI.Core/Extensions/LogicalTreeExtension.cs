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

            if (element is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    var result = (child as FrameworkElement)?.FindChildByName(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in ((ItemsControl) element).Items)
                {
                    var result = (item as FrameworkElement)?.FindChildByName(name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                var result = (element.GetContentControl() as FrameworkElement)?.FindChildByName(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static T FindChild<T>(this FrameworkElement element) where T : FrameworkElement
        {
            if (element == null)
            {
                return null;
            }

            if (element is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is T)
                    {
                        return child as T;
                    }

                    var result = (child as FrameworkElement)?.FindChild<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in ((ItemsControl) element).Items)
                {
                    var result = (item as FrameworkElement)?.FindChild<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            else
            {
                var content = element.GetContentControl();

                if (content is T)
                {
                    return content as T;
                }

                var result = (content as FrameworkElement)?.FindChild<T>();
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static IEnumerable<T> FindChildren<T>(this FrameworkElement element) where T : FrameworkElement
        {
            if (element == null)
            {
                yield break;
            }

            if (element is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is T)
                    {
                        yield return child as T;
                    }

                    if (child is FrameworkElement childFrameworkElement)
                    {
                        foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
            else if (element is ItemsControl)
            {
                foreach (var item in ((ItemsControl) element).Items)
                {
                    if (item is FrameworkElement childFrameworkElement)
                    {
                        foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
            else
            {
                var content = element.GetContentControl();
                if (content is T)
                {
                    yield return content as T;
                }

                if (content is FrameworkElement childFrameworkElement)
                {
                    foreach (var childOfChild in childFrameworkElement.FindChildren<T>())
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static FrameworkElement FindParentByName(this FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            return name.Equals(element.Name, StringComparison.OrdinalIgnoreCase) ? element : (element.Parent as FrameworkElement)?.FindParentByName(name);
        }

        public static T FindParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            switch (element.Parent)
            {
                case null:
                    return null;
                case T _:
                    return element.Parent as T;
            }

            return (element.Parent as FrameworkElement).FindParent<T>();
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