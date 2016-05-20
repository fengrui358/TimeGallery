using System;
using System.Reflection;

namespace TimeGallery.DataBase.Attributes
{
    /// <summary>
    /// 标记一个数据库的列为自动值，自动值无需程序写入控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoValueAttribute : System.Attribute
    {
    }

    public static class AutoValueExtension
    {
        public static bool IsAutoValue(this PropertyInfo propertyInfo)
        {
            return System.Attribute.GetCustomAttribute(propertyInfo, typeof(AutoValueAttribute), true) != null;
        }
    }
}