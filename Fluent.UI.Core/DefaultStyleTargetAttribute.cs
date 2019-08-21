using System;

namespace Fluent.UI.Core
{
    public class DefaultStyleTargetAttribute : Attribute
    {
        public Type TargetType { get; set; }

        public DefaultStyleTargetAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
