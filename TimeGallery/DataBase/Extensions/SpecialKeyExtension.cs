using System.Reflection;
using TimeGallery.DataBase.Attributes;

namespace TimeGallery.DataBase.Extensions
{
    public static class SpecialKeyExtension
    {
        public static bool IsSpecialKey<T>(this PropertyInfo propertyInfo) where T : SpecialKeyAttribute
        {
            return System.Attribute.GetCustomAttribute(propertyInfo, typeof(T), true) != null;
        }

        public static bool IsSpecialKey(this PropertyInfo propertyInfo, SpecialKeyAttribute specialKeyAttribute)
        {
            return System.Attribute.GetCustomAttribute(propertyInfo, specialKeyAttribute.GetType(), true) != null;
        }
    }
}