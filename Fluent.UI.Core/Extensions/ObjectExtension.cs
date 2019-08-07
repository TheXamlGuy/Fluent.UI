namespace Fluent.UI.Core.Extensions
{
    public static class ObjectExtension
    {
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            var propertyInfo = obj?.GetType().GetProperty(propertyName);
            return (T)propertyInfo?.GetValue(obj, null);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var propertyInfo = obj?.GetType().GetProperty(propertyName);
            return propertyInfo?.GetValue(obj, null);
        }
    }
}