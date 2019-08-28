using System;
using System.Linq;

namespace Fluent.UI.Core.Extensions
{
    public static class TypeExtension
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default;
        }


        public static TValue GetValue<TValue>(this Type target, object source, string propertyName)
        {
            var propertyInfo = target?.GetProperty(propertyName);
            return (TValue)propertyInfo?.GetMethod.Invoke(source, null);
        }
    }
}