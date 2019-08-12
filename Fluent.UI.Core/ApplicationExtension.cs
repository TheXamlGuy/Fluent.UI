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
                new PropertyMetadata(ApplicationTheme.Dark));

        public static void SetRequestedTheme(Application application, ApplicationTheme requestedTheme)
        {
            application.Startup += (sender, args) =>
            {
                var objectType = Type.GetType("Fluent.UI.Controls.ControlExtension`2, Fluent.UI.Controls");

                var themeResource = new Uri($@"Fluent.UI.Controls;component/Themes/ThemeResources.xaml", UriKind.Relative);
                application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeResource });

                foreach (var type in Assembly.GetAssembly(objectType).GetTypes().Where(x => x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == objectType))
                {
                    var typeName = type.BaseType.GetGenericArguments()[0].Name;
                    var typeNamespace = type.Namespace;
                    var requestedThemeName = requestedTheme == ApplicationTheme.Dark ? "Default" : "Light";

                    var controlResource = new Uri($@"{typeNamespace};component/{typeName}/{typeName}.xaml", UriKind.Relative);
                    var controlThemeResource = new Uri($@"{typeNamespace};component/{typeName}/{typeName}.{requestedThemeName}.xaml", UriKind.Relative);

                    application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = controlThemeResource });
                    application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = controlResource });
                }
            };
        }
    }
}
