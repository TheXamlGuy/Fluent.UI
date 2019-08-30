using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fluent.UI.Core.Extensions
{
    public static class VisualTreeExtension
    {
        public static TDependencyObject FindAscendant<TDependencyObject>(this DependencyObject element) where TDependencyObject : DependencyObject
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(element);

                switch (parent)
                {
                    case null:
                        return null;
                    case TDependencyObject dependencyObject:
                        return dependencyObject;
                }

                element = parent;
            }
        }

        public static int FindAscendantCount<TDependencyParent, TDependencyGrandfather>(this DependencyObject dependencyChild) where TDependencyParent : DependencyObject where TDependencyGrandfather : DependencyObject
        {
            var count = 0;
            while (true)
            {
                var dependencyParent = VisualTreeHelper.GetParent(dependencyChild);
                if (dependencyParent == null)
                {
                    return count;
                }

                if (dependencyParent is TDependencyGrandfather)
                {
                    break;
                }

                if (dependencyParent is TDependencyParent)
                {
                    count++;
                }

                dependencyChild = dependencyParent;
            }

            return count;
        }


        public static FrameworkElement FindAscendantByName(this DependencyObject element, string name)
        {
            while (true)
            {
                if (element == null || string.IsNullOrWhiteSpace(name))
                {
                    return null;
                }

                var parent = VisualTreeHelper.GetParent(element);
                if (parent == null)
                {
                    return null;
                }

                if (name.Equals((parent as FrameworkElement)?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return parent as FrameworkElement;
                }

                element = parent;
            }
        }

        public static TDependencyObject FindDescendant<TDependencyObject>(this DependencyObject element) where TDependencyObject : DependencyObject
        {
            TDependencyObject retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is TDependencyObject type)
                {
                    retValue = type;
                    break;
                }

                retValue = FindDescendant<TDependencyObject>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }

        public static FrameworkElement FindDescendantByName(this DependencyObject element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (name.Equals((element as FrameworkElement)?.Name, StringComparison.OrdinalIgnoreCase))
            {
                return element as FrameworkElement;
            }

            var childCount = VisualTreeHelper.GetChildrenCount(element);
            for (var i = 0; i < childCount; i++)
            {
                var result = VisualTreeHelper.GetChild(element, i).FindDescendantByName(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public static IEnumerable<TDependencyObject> FindDescendants<TDependencyObject>(this DependencyObject element) where TDependencyObject : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is TDependencyObject type)
                {
                    yield return type;
                }

                foreach (var childOfChild in child.FindDescendants<TDependencyObject>())
                {
                    yield return childOfChild;
                }
            }
        }

        public static bool IsChildOf(this FrameworkElement child, FrameworkElement parent)
        {
            while (true)
            {
                if (child == null)
                {
                    return false;
                }

                if (!(child.Parent is FrameworkElement parentObject))
                {
                    return false;
                }

                if (ReferenceEquals(parent, parentObject))
                {
                    return true;
                }
                else
                {
                    child = parentObject;
                }
            }
        }
    }
}