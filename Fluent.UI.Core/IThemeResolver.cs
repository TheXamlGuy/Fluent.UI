using System;
using System.Windows;

namespace Fluent.UI.Core
{
    public interface IThemeResolver
    {
        ResourceDictionary Resolve(Type targetType, ElementTheme requestedTheme);
    }
}