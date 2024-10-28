using System.Reflection;

namespace Sc.Models.Attributes
{
    public class SkipDeleteAttribute : Attribute
    {
        public static bool IsDeclared(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(SkipDeleteAttribute)) != null;
    }
}
