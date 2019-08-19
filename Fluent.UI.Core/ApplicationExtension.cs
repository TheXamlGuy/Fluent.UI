using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Fluent.UI.Core
{
    public class ApplicationExtension
    {
        public static readonly DependencyProperty RequestedThemeProperty =
            DependencyProperty.RegisterAttached("RequestedTheme",
                typeof(ApplicationTheme), typeof(ApplicationExtension),
                new PropertyMetadata(ApplicationTheme.Default));

        public static ApplicationTheme RequestedTheme { get; private set; }

        public static void SetRequestedTheme(Application application, ApplicationTheme requestedTheme)
        {
            RequestedTheme = requestedTheme;

            var requestedThemeName = (requestedTheme == ApplicationTheme.Default || requestedTheme == ApplicationTheme.Dark) ? "Default" : "Light";

            application.Startup += (sender, args) =>
            {
                var themeResource = new Uri($"pack://application:,,,/Fluent.UI.Controls;component/Themes/ThemeResources.{requestedThemeName}.xaml", UriKind.Absolute);
                application.Resources.MergedDictionaries.Insert(0, new SharedResourceDictionary { Source = themeResource });

                var objectType = Type.GetType("Fluent.UI.Controls.FrameworkElementExtension`1, Fluent.UI.Controls");

                foreach (var type in Assembly.GetAssembly(objectType).GetTypes().Where(x => !x.IsAbstract && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == objectType))
                {
                    MergeThemeResource(application, requestedThemeName, type);
                }
            };
        }

        private static void MergeThemeResource(Application application, string requestedThemeName, Type type)
        {
            var typeName = type.BaseType.GetGenericArguments()[0].Name;
            var typeNamespace = type.Namespace;

            var themeResource = new Uri($"pack://application:,,,/{typeNamespace};component/{typeName}/{typeName}.{requestedThemeName}.xaml", UriKind.Absolute);

            try
            {
                application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeResource });
            }
            catch
            {
                // nothing
            }
        }
    }
}
