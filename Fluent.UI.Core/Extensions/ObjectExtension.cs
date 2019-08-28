using System;
using System.Reflection;

namespace Fluent.UI.Core.Extensions
{
    public static class ObjectExtension
    {
        public static TValue GetValue<TValue>(this object source, string propertyName, BindingFlags flags)
        {
            var propertyInfo = source?.GetType().GetProperty(propertyName, flags);
            return (TValue)propertyInfo?.GetMethod.Invoke(source, null);
        }
    }
}