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
            application.Startup += (sender, args) =>
            {
                var objectType = Type.GetType("Fluent.UI.Controls.FrameworkElementExtension`2, Fluent.UI.Controls");

                var themeResource = new Uri($@"/Fluent.UI.Controls;component/Themes/ThemeResources.xaml", UriKind.Relative);
                application.Resources.MergedDictionaries.Insert(0, new SharedResourceDictionary { Source = themeResource });

                foreach (var type in Assembly.GetAssembly(objectType).GetTypes().Where(x => !x.IsAbstract && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == objectType))
                {
                    MergeResources(application, requestedTheme, type);
                }
            };
        }

        private static void MergeResources(Application application, ApplicationTheme requestedTheme, Type type)
        {
            var typeName = type.BaseType.GetGenericArguments()[0].Name;
            var typeNamespace = type.Namespace;
            var requestedThemeName = (requestedTheme == ApplicationTheme.Default || requestedTheme == ApplicationTheme.Dark) ? "Default" : "Light";

            var controlResource = new Uri($@"/{typeNamespace};component/{typeName}/{typeName}.xaml", UriKind.Relative);
            var controlThemeResource = new Uri($@"/{typeNamespace};component/{typeName}/{typeName}.{requestedThemeName}.xaml", UriKind.Relative);

            try
            {
                application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = controlThemeResource });
                application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = controlResource });
            }
            catch
            {
                // nothing
            }
        }
    }
}
