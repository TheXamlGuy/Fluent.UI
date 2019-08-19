using System;
using System.Windows;

namespace Fluent.UI.Core
{
    public interface IThemeFactory
    {
        Style Create(Type targetType, ElementTheme requestedTheme);
    }
}