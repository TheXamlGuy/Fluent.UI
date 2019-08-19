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

                var assemblyType = Type.GetType("Fluent.UI.Controls.FrameworkElementExtension, Fluent.UI.Controls");
                var extensionType = Type.GetType("Fluent.UI.Core.FrameworkElementExtension`1, Fluent.UI.Core");

                foreach (var type in Assembly.GetAssembly(assemblyType).GetTypes().Where(x => !x.IsAbstract && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == extensionType))
                {
                    MergeThemeResource(application, (ElementTheme)requestedTheme, type);
                }
            };
        }

        private static void MergeThemeResource(Application application, ElementTheme requestedTheme, Type type)
        {
            var targetType = type.BaseType.GetGenericArguments()[0];

            var themeResource = RequestedThemeResolver.Current.Resolve(targetType, requestedTheme);
            if (themeResource != null)
            {
                application.Resources.MergedDictionaries.Add(themeResource);
            }
        }
    }
}