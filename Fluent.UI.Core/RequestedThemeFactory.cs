using System;
using System.Collections.Generic;
using System.Windows;

namespace Fluent.UI.Core
{
    public class RequestedThemeFactory : IThemeFactory
    {
        private static readonly object _lock = new object();
        private static RequestedThemeFactory _current = null;
        private IDictionary<Tuple<Type, ElementTheme>, Style> _themeCache = new Dictionary<Tuple<Type, ElementTheme>, Style>();

        private RequestedThemeFactory()
        {
        }

        public static RequestedThemeFactory Current
        {
            get
            {
                lock (_lock)
                {
                    if (_current == null)
                    {
                        _current = new RequestedThemeFactory();
                    }
                    return _current;
                }
            }
        }

        public Style Create(Type targetType, ElementTheme requestedTheme)
        {
            var key = new Tuple<Type, ElementTheme>(targetType, requestedTheme);
            if (_themeCache.TryGetValue(key, out Style cachedStyle))
            {
                return cachedStyle;
            }
            else
            {
                var elementTypeName = targetType.Name;
                var extensionTypeNamespace = "Fluent.UI.Controls";
                var requestedThemeName = (requestedTheme == ElementTheme.Default || requestedTheme == ElementTheme.Dark) ? "Default" : "Light";

                try
                {
                    var themeResource = new Uri($@"pack://application:,,,/{extensionTypeNamespace};component/{elementTypeName}/{elementTypeName}.{requestedThemeName}.xaml", UriKind.Absolute);
                    var resourceDictionary = new SharedResourceDictionary { Source = themeResource };

                    var style = resourceDictionary[targetType] as Style;

                    _themeCache[key] = style;
                    return style;
                }
                catch
                {
                    return null;
                }
    
            }
        }
    }
}
