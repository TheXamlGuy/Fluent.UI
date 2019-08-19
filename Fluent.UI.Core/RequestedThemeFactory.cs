using System;
using System.Collections.Generic;
using System.Windows;

namespace Fluent.UI.Core
{
    public class RequestedThemeResolver : IThemeResolver
    {
        private static readonly object _lock = new object();
        private static RequestedThemeResolver _current = null;

        private RequestedThemeResolver()
        {
        }

        public static RequestedThemeResolver Current
        {
            get
            {
                lock (_lock)
                {
                    if (_current == null)
                    {
                        _current = new RequestedThemeResolver();
                    }
                    return _current;
                }
            }
        }

        public ResourceDictionary Resolve(Type targetType, ElementTheme requestedTheme)
        {
            var elementTypeName = targetType.Name;
            var extensionTypeNamespace = "Fluent.UI.Controls";
            var requestedThemeName = (requestedTheme == ElementTheme.Default || requestedTheme == ElementTheme.Dark) ? "Default" : "Light";

            ResourceDictionary resourceDictionary;
            try
            {
                var themeResource = new Uri($@"pack://application:,,,/{extensionTypeNamespace};component/{elementTypeName}/{elementTypeName}.{requestedThemeName}.xaml", UriKind.Absolute);
                resourceDictionary = new ResourceDictionary { Source = themeResource };
            }
            catch
            {
                return null;
            }

            return resourceDictionary;
        }
    }

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
                var resource = RequestedThemeResolver.Current.Resolve(targetType, requestedTheme);
                var style = resource[targetType] as Style;

                _themeCache[key] = style;
                return style;
            }
        }
    }
}
