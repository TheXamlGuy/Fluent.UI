using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Fluent.UI.Core
{
    internal class AttachedFrameworkElementTemplateFactory
    {
        private static readonly Lazy<AttachedFrameworkElementTemplateFactory> _lazyFactory =  new Lazy<AttachedFrameworkElementTemplateFactory> (() => new AttachedFrameworkElementTemplateFactory());

        private AttachedFrameworkElementTemplateFactory()
        {

        }

        public static AttachedFrameworkElementTemplateFactory Current => _lazyFactory.Value;

        public IAttachedFrameworkElementTemplate Create(FrameworkElement source)
        {
            IAttachedFrameworkElementTemplate attachedTemplate;
            var sourceType = source.GetType();

            var assemblyType = Type.GetType("Fluent.UI.Controls.ButtonExtension, Fluent.UI.Controls");
            var extensionType = Type.GetType("Fluent.UI.Core.FrameworkElementExtension, Fluent.UI.Core");

            var attachedTemplateType = Assembly.GetAssembly(assemblyType).GetTypes().FirstOrDefault(x => typeof(IAttachedFrameworkElementTemplate<>).MakeGenericType(sourceType).IsAssignableFrom(x));

            if (attachedTemplateType != null)
            {
                attachedTemplate = Activator.CreateInstance(attachedTemplateType) as IAttachedFrameworkElementTemplate;
            }
            else
            {
                attachedTemplate = Activator.CreateInstance(typeof(AttachedFrameworkElementTemplate<>).MakeGenericType(sourceType)) as IAttachedFrameworkElementTemplate;
            }

            return attachedTemplate;
        }
    }
}