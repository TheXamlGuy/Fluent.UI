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

        public static ApplicationTheme RequestedTheme { get; private set; }

        public static void SetRequestedTheme(Application application, ApplicationTheme requestedTheme)
        {
            RequestedTheme = requestedTheme;

            application.Startup += (sender, args) =>
            {
                var objectType = Type.GetType("Fluent.UI.Controls.ControlExtension`2, Fluent.UI.Controls");

                var themeResource = new Uri($@"/Fluent.UI.Controls;component/Themes/ThemeResources.xaml", UriKind.Relative);
                application.Resources.MergedDictionaries.Insert(0, new SharedResourceDictionary { Source = themeResource });

                foreach (var type in Assembly.GetAssembly(objectType).GetTypes().Where(x => x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == objectType))
                {
                    var typeName = type.BaseType.GetGenericArguments()[0].Name;
                    var typeNamespace = type.Namespace;
                    var requestedThemeName = requestedTheme == ApplicationTheme.Dark ? "Default" : "Light";

                    var controlResource = new Uri($@"/{typeNamespace};component/{typeName}/{typeName}.xaml", UriKind.Relative);
                    var controlThemeResource = new Uri($@"/{typeNamespace};component/{typeName}/{typeName}.{requestedThemeName}.xaml", UriKind.Relative);


                    application.Resources.MergedDictionaries.Add(new SharedResourceDictionary { Source = controlThemeResource });
                    application.Resources.MergedDictionaries.Add(new SharedResourceDictionary { Source = controlResource });
                }

                //var GenericResource = new Uri($@"Fluent.UI.Controls;component/Themes/Generic.xaml", UriKind.Relative);
                //var skin = new ResourceDictionary { Source = GenericResource };


                //var assemblyName = objectType.Assembly.GetName().Name;
                //using var stream = objectType.Assembly.GetManifestResourceStream($"{assemblyName}.g.resources");
                //using var resourceReader = new ResourceReader(stream);

                //foreach (DictionaryEntry resource in resourceReader)
                //{
                //    if (new FileInfo(resource.Key.ToString()).Name == "generic.baml")
                //    {
                //        //Uri uri = new Uri("/" + assembly.GetName().Name + ";component/" + resource.Key.ToString().Replace(".baml", ".xaml"), UriKind.Relative);
                //        //ResourceDictionary skin = Application.LoadComponent(uri) as ResourceDictionary;
                //        //this.Resources.MergedDictionaries.Add(skin);
                //    }
                //}

                //var stream = objectType.Assembly.GetManifestResourceStream(objectType.Assembly.GetName().Name + ".g.resources");


                //var themeResource2 = new Uri($@"Fluent.UI.Controls;component/ContentDialog/ContentDialog.Light.xaml", UriKind.Relative);
                //application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeResource2 });
            };
        }
    }
}
