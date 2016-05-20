using System;
using System.Linq;
using System.Reflection;

namespace TimeGallery.DataBase.Attributes
{
    /// <summary>
    /// 标记一个列为自增主键值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementkeyAttribute : AutoValueAttribute
    {
    }

    public static class AutoIncrementkeyValueExtension
    {
        public static bool IsAutoIncrementkeyValue(this PropertyInfo propertyInfo)
        {
            return System.Attribute.GetCustomAttribute(propertyInfo, typeof(AutoIncrementkeyAttribute), true) != null;
        }

        public static bool TryGetAutoIncrementkeyValue<T>(this DataEntityBase<T> dataEntity, out long value) where T : class
        {
            value = 0;

            var autoIncrementkey =
                typeof(T).GetProperties(BindingFlags.Public).FirstOrDefault(s => s.IsAutoIncrementkeyValue());
            if (autoIncrementkey != null)
            {
                return autoIncrementkey.TryGetAutoIncrementkeyValue(dataEntity, out value);
            }

            return false;
        }

        public static bool TryGetAutoIncrementkeyValue(this PropertyInfo propertyInfo, object source, out long value)
        {
            value = 0;

            if (propertyInfo.IsAutoIncrementkeyValue())
            {
                return long.TryParse(propertyInfo.GetValue(source).ToString(), out value);
            }

            return false;
        }
    }
}