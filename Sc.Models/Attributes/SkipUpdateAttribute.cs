using System.Reflection;

namespace Sc.Models.Attributes
{
    public class SkipUpdateAttribute : Attribute
    {
        public static bool IsDeclared(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(SkipUpdateAttribute)) != null;
    }
}
