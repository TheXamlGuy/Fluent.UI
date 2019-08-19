using System;
using System.Windows;

namespace Fluent.UI.Controls
{
    public interface IThemeFactory
    {
        Style Create(Type targetType, ElementTheme requestedTheme);
    }
}